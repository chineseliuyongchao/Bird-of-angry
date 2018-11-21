using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour {
    private bool Mouse;//鼠标是否按下
    [HideInInspector]//下一行变量在面板里面看不见
    public bool mouse;//鼠标是否按下过并且松开了
    [HideInInspector]//下一行变量在面板里面看不见
    public bool canmove;//判断小鸟能不能飞
    private bool collisaudio = true;//与物体碰撞的音效能否播放
    public bool isyellowbird;//是不是黄色小鸟
    public bool isgreenbird;//是不是绿色小鸟
    public bool isblackbird;//是不是黑色小鸟
    public List<Pig> blocks = new List<Pig>();//建立一个待销毁列表
    private bool isfly = false;//能否释放技能(正在飞行中并且技能只能释放一次)

    private SpriteRenderer Renderer;//声明一个SpriteRenderer组件
    public Sprite Hurt;//声明受伤图片
    private float MaxDis = 1.8f;//弹弓能拉长的最大距离
    [HideInInspector]//下一行变量在面板里面看不见
    public SpringJoint2D sp;//声明弹簧接头
    private Rigidbody2D rg;//声明游戏刚体

    public GameObject LeftPos;//弹弓皮筋在弹弓上的连接点
    public LineRenderer left; //弹弓的皮筋效果
    public GameObject RightPos;//弹弓皮筋在弹弓上的连接点
    public LineRenderer right; //弹弓的皮筋效果
    private bool Line = true;//声明一个判断皮筋效果是否可以打开的布尔变量，并且默认可以打开

    public GameObject boom;
    public AudioClip select;//声明各种音效
    public AudioClip flying;
    public AudioClip collis;

    // Use this for initialization
    void Start () {
        sp = this.GetComponent<SpringJoint2D>();
        rg = this.GetComponent<Rigidbody2D>();
        rg.constraints = RigidbodyConstraints2D.FreezeRotation;//默认锁定物体的旋转
        Renderer = this.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (EventSystem.current.IsPointerOverGameObject())//判断是否点击ui界面
        {
            return;
        }
		if(Mouse&&mouse==false&&canmove)//如果按下鼠标并且没有松开过
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);//将鼠标的屏幕坐标转化为世界坐标并赋值给小鸟的坐标
            this.transform.position += new Vector3(0, 0, -Camera.main.transform.position.z);//将小鸟的坐标减去照相机坐标的偏移
            if (Vector3.Distance(this.transform.position, LeftPos.transform.position) > MaxDis)//判断弹弓皮筋长度是否超过最大拉长距离
            {
                Vector3 vector = (this.transform.position - LeftPos.transform.position).normalized;//计算小鸟与皮筋在弹弓连接点的单位向量
                vector *= MaxDis;//计算出小鸟与皮筋在弹弓连接点的向量
                this.transform.position = LeftPos.transform.position + vector;//计算出小鸟应该的位置
            }
            Render();
        }
        float posX = this.transform.position.x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Mathf.Clamp(posX, 0, 15), Camera.main.transform.position.y, Camera.main.transform.position.z), 2f * Time.deltaTime);
        //控制主相机移动
        //Vector3.Lerp(原来的位置，目标位置，平滑移动)   Mathf.Clamp(期望等于的值，最小限度，最大限度)
        if (Input.GetMouseButtonDown(0))//如果按下鼠标左键
        {
            if (isyellowbird && isfly)//如果是黄色小鸟并且正在飞行
            {
                rg.velocity *= 2;//速度乘2
            }
            else if(isgreenbird&&isfly)//如果是绿色小鸟并且正在飞行
            {
                Vector3 speed = rg.velocity;//x轴速度反向并且稍微加速
                speed.x *= -1.3f;
                rg.velocity = speed;
            }
            else if (isblackbird && blocks.Count >= 0 && isfly)//如果是黑色小鸟并且正在飞行
            {
                for (int i = 0; i < blocks.Count; i++)//让触发列表中的所有物体死亡
                {
                    blocks[i].Death();
                }
                rg.velocity = Vector3.zero;//让自己销毁
                Instantiate(boom, transform.position, Quaternion.identity);
                Renderer.enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;
            }
            isfly = false;
        }
    }
    private void OnMouseDown()//鼠标按下
    {
        Mouse = true;
        if (mouse==false&&canmove)
        {
            if (Line == true)//判断皮筋是否可以打开
            {
                right.enabled = true;//打开皮筋效果
                left.enabled = true;
            }
            rg.isKinematic = true;//关闭物理学影响
            AudioPlay(select);
        }
    }
    private void OnMouseUp()//鼠标抬起
    {
        Mouse = false;
        if (mouse==false&&canmove)
        {
            Line = false;//当前小鸟脱离弹弓以后，皮筋不可以打开
            right.enabled = false;//关闭皮筋效果
            left.enabled = false;
            rg.isKinematic = false;//打开物理学影响
            Invoke("Fly", 0.1f);//在0.1秒以后调用这个函数
            AudioPlay(flying);
            canmove = false;
            mouse = true;
            isfly = true;
        }
    }
    private void Fly()
    {
        sp.enabled = false;//关闭弹簧接头
        if (isgreenbird && isfly)//如果是绿色小鸟并且正在飞行
        {
            rg.velocity *= 1.5f;
        }
        Invoke("Next", 5);
    }
    private void Render()
    {
        right.SetPosition(0, RightPos.transform.position);//控制LineRenderer组件的始点和终点
        right.SetPosition(1, this.transform.position);
        left.SetPosition(0, LeftPos.transform.position);
        left.SetPosition(1, this.transform.position);
    }
    private void Next()
    {
        GameManager._instance.birds.Remove(this);//将这个小鸟从列表中移除
        Destroy(gameObject);
        Instantiate(boom, this.transform.position, Quaternion.identity);//在这个地方生成爆炸效果
        GameManager._instance.NextBird();//调用判断游戏是否胜利的函数
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(mouse)//如果松开了鼠标
        {
            if(collision.collider)//如果碰撞到了物体
            {
                Renderer.sprite = Hurt;//更换受伤图片
                isfly = false;
                rg.constraints -= RigidbodyConstraints2D.FreezeRotation;//当小鸟弹出并撞到物体后解锁小鸟的旋转
                if (collisaudio)//如果没有播放过该音效
                {
                    AudioPlay(collis);
                    collisaudio = false;
                }
            }
        }
    }
    public void AudioPlay(AudioClip c)//声明一个音效播放函数，调用该函数时传递一个音效参数
    {
        AudioSource.PlayClipAtPoint(c, this.transform.position);//让该音效在这个物体的位置播放
    }
    private void OnTriggerEnter2D(Collider2D collision)//是否进入触发检测范围
    {
        if(collision.gameObject.tag=="Annmy"&&isblackbird)
        {
            blocks.Add(collision.gameObject.GetComponent<Pig>());//将触发检测到的Annmy放入待销毁列表
        }
    }
    private void OnTriggerExit2D(Collider2D collision)//是否离开触发检测范围
    {

        if (collision.gameObject.tag == "Annmy" && isblackbird)
        {
            blocks.Remove(collision.gameObject.GetComponent<Pig>());//将触发检测到的Annmy移出待销毁列表
        }
    }
}
