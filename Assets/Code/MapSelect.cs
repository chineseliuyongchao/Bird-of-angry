using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour {
    public GameObject Sourse;
    public int starsnum;//声明解锁该关卡需要的星星
    private bool islock = false;//声明判断是否解锁的变量
    public GameObject locks;
    public GameObject stars;
    public GameObject map;
    public GameObject level;
    public Text starstext;
    private int startNum = 1;//最小关卡
    private int endNum = 5;//最大关卡
    void Start ()
    {
        Sourse.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Volume");
        locks.SetActive(true);
        stars.SetActive(false);
        map.SetActive(true);
        level.SetActive(false);
        if(PlayerPrefs.GetInt("Totalnum", 0) >= starsnum)//判断总星星数量是否可以解锁该关卡
        {
            islock = true;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (islock)
        {
            locks.SetActive(false);//将此关卡解锁
            stars.SetActive(true);
            if (starsnum == 5)
            {
                startNum = 6;
                endNum = 10;
            }
            else if (starsnum == 15)
            {
                startNum = 11;
                endNum = 15;
            }
            int counts = 0;//暂时存贮当前总星星个数
            for(int i = startNum; i <= endNum; i++)//把所有关卡的星星总数加起来
            {
                counts += PlayerPrefs.GetInt("level" + i.ToString());
            }
            starstext.text = counts.ToString() + "/15";//在关卡文本前面显示出来
        }
	}
    public void MapSlect()
    {
        if (islock)
        {
            map.SetActive(false);//进入关卡
            level.SetActive(true);
        }
    }
    public void panelSlect()
    {
        map.SetActive(true);//退出关卡
        level.SetActive(false);
    }
    public void retry()
    {
        PlayerPrefs.DeleteAll();//清理所有数据
        print(PlayerPrefs.GetInt("Totalnum"));
        if (PlayerPrefs.GetInt("Totalnum") < starsnum)
        {
            islock = false;
            locks.SetActive(true);//将此关卡锁定
            stars.SetActive(false);
        }
    }
    public void sourse(float music)
    {
        PlayerPrefs.SetFloat("Volume", music);
        GameObject gameObject = GameObject.Find("Main Camera");
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("Volume");
    }
}
