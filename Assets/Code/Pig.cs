using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    private SpriteRenderer Renderer;//声明一个SpriteRenderer组件
    public float MaxSpeed = 7;//声明死亡速度
    public float MixSpeed = 4;//声明受伤速度
    public Sprite Hurt;       //声明受伤图片
    public GameObject boom;   //声明爆炸效果
    public GameObject Score;  //声明击毁分数

    public bool Ispig = false;

    public AudioClip pigcollis1;//声明各种音效
    public AudioClip pigdead;
    public AudioClip woodcollis;
    public AudioClip wooddead;
    // Use this for initialization
    void Start()
    {
        Renderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)//碰撞检测
    {
        if (collision.relativeVelocity.magnitude >= MaxSpeed)//如果碰撞速度大于死亡速度
        {
            Death();
        }
        else if (collision.relativeVelocity.magnitude < MaxSpeed && collision.relativeVelocity.magnitude >= MixSpeed)//如果碰撞速度小于死亡速度，大于受伤速度
        {
            Renderer.sprite = Hurt;//更换受伤图片
            if(Ispig)
            {
                AudioPlay(pigcollis1);
            }
            else
            {
                AudioPlay(woodcollis);
            }
        }
    }
    public void Death()
    {
        Destroy(this.gameObject);//销毁游戏物体
        Instantiate(boom, this.transform.position,Quaternion.identity);//实例化猪爆炸的效果
        GameObject go = Instantiate(Score, this.transform.position+new Vector3(0,0.5f,0), Quaternion.identity);//实例化分数效果
        Destroy(go, 1.5f);//让分数在1.5秒以后消失
        if (Ispig)//如果真的是猪的话
        {
            GameManager._instance.pigs.Remove(this);//将这个猪从列表中移除
            AudioPlay(pigdead);
        }
        else
        {
            AudioPlay(wooddead);
        }
    }
    private void AudioPlay(AudioClip c)//声明一个音效播放函数，调用该函数时传递一个音效参数
    {
        AudioSource.PlayClipAtPoint(c, this.transform.position);//让该音效在这个物体的位置播放
    }
}
