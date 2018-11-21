using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    private Animator anim;//声明一个动画状态机
    private int PauseID = Animator.StringToHash("ispause");//声明一个哈希值等于动画状态机中的ispause变量
    private GameObject bg;//声明游戏背景
    public GameObject pause;
    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        bg = GameObject.Find("bg");
        bg.SetActive(false);//背景默认关闭
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void retry()
    {
        SceneManager.LoadScene(2);//如果按下重玩键，加载场景2
    }
    public void home()
    {
        SceneManager.LoadScene(1);//如果按下返回键，加载场景1
    }
    public void Pause()//按下暂停键，游戏暂停
    {
        bg.SetActive(true);
        anim.SetBool(PauseID, true);
        pause.SetActive(false);//让暂停键消失
        if (GameManager._instance.birds.Count > 0)
        {
            if (GameManager._instance.birds[0].mouse == false)
            {
                GameManager._instance.birds[0].canmove = false;
            }
        }
    }
    public void resume()//按下继续键，游戏继续
    {
        StartCoroutine(Enumerable());
        anim.SetBool(PauseID, false);
        if (GameManager._instance.birds.Count > 0)
        {
            if (GameManager._instance.birds[0].mouse == false)
            {
                GameManager._instance.birds[0].canmove = true;
            }
        }
    }
    IEnumerator Enumerable()//声明一个协程函数，使背景关闭延迟一秒
    {
        yield return new WaitForSeconds(1f);
        bg.SetActive(false);
        pause.SetActive(true);//让暂停键出现
    }
}
