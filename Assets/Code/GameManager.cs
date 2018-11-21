using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public List<Bird> birds;//建立一个小鸟的列表，存放所有小鸟
    public List<Pig> pigs;  //建立一个猪的列表，存放所有猪
    public static GameManager _instance;//创建一个游戏管理器
    public GameObject win; //声明一个胜利的游戏面板
    public GameObject lose;//声明一个失败的游戏面板
    public GameObject[] stars;//建立一个数组，存放所有星星
    private Vector3 vector3;
    private int starsNum = 0;
    private int totalNum = 10;
    private void Awake()
    {
        _instance= this;
        vector3 = birds[0].transform.position;//让三维向量等于第一只小鸟的坐标
    }
    private void Start()
    {
        Initialized();
        win.SetActive(false);//失败胜利的面板默认关闭
        lose.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            stars[i].SetActive(false);
        }
    }
    private void Initialized()//判断每只小鸟的代码和弹簧组件是否应该打开
    {
        for(int i = 0; i < birds.Count; i++)
        {
            if (i == 0)
            {
                birds[i].transform.position = vector3;
                birds[i].enabled = true;
                birds[i].sp.enabled = true;
                birds[i].canmove = true;
            }
            else
            {
                birds[i].enabled = false;
                birds[i].sp.enabled = false;
                birds[i].canmove = false;
            }
        }
    }
    public void NextBird()//判断游戏是否胜利
    {
        if (pigs.Count > 0)
        {
            if (birds.Count > 0)
            {
                Initialized();
            }
            else//游戏失败
            {
                lose.SetActive(true);
            }
        }
        else//游戏胜利
        {
            win.SetActive(true);
            StartCoroutine("show");//调用协程函数
        }
    }
    IEnumerator show()
    {
        for(;starsNum<birds.Count+1;starsNum++)//通过判断剩余小鸟的数量来判断显示几颗星星
        {
            if(starsNum>=3)
            {
                break;
            }
            yield return new WaitForSeconds(0.5f);//通过协程让星星延迟显示0.5秒
            stars[starsNum].SetActive(true);
        }
    }
    public void retry()
    {
        SceneManager.LoadScene(2);//切换到场景2
        SaveData();
    }
    public void home()
    {
        SceneManager.LoadScene(1);//切换到场景1
        SaveData();
    }
    public void SaveData()
    {
        if (starsNum > PlayerPrefs.GetInt(PlayerPrefs.GetString("nowLevel")))//判断是否需要更新存储的星星数量
        {
            PlayerPrefs.SetInt(PlayerPrefs.GetString("nowLevel"), starsNum);//将该关卡获得的星星数量存储到该关卡中
        }
        int sum = 0;
        for(int i = 0; i < totalNum; i++)//存储星星总数
        {
            sum += PlayerPrefs.GetInt("level" + i.ToString());
            PlayerPrefs.SetInt("Totalnum", sum);
        }
    }
}
