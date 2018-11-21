using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelAsync : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.SetResolution(1080, 810, false);//固定游戏分辨率
        SceneManager.LoadSceneAsync(1);//异步加载场景1
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
