using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    public bool isSelect = false;//声明一个判断关卡是否解锁的变量
    public Sprite levelbg;//声明一张精灵图片
    private Image image;
    public GameObject[] stars;//建立一个数组，存放所有星星
    void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.parent.GetChild(0).name == gameObject.name)
        {
            isSelect = true;//该关卡可以解锁
        }
        else
        {
            int beforenum = int.Parse(gameObject.name) - 1;//把字符串类型的数字转换为数字减一并且赋给beforenum（得到该关卡的前一关卡）
            if (PlayerPrefs.GetInt("level" + beforenum.ToString())>0)//如果前一关卡的星星个数大于0就解锁该关卡
            {
                isSelect = true;//该关卡可以解锁
            }
        }
        if (isSelect)//解锁操作
        {
            image.overrideSprite = levelbg;
            transform.Find("num").gameObject.SetActive(true);//寻找叫num的物体并将其激活
            int count = PlayerPrefs.GetInt("level" + gameObject.name);//获取现在关卡对应的名字，然后获得对应的星星个数
            if (count > 0)//判断该关卡显示几颗星星
            {
                for(int i = 0; i < count; i++)
                {
                    stars[i].SetActive(true);
                }
            }
        }
    }
    public void Selected()
    {
        if (isSelect)
        {
            PlayerPrefs.SetString("nowLevel", "level" + gameObject.name);//存储该关卡
            SceneManager.LoadScene(2);
        }
    }
}
