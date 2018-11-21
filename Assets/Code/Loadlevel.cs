using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loadlevel : MonoBehaviour
{
    private void Awake()
    {
        Instantiate(Resources.Load(PlayerPrefs.GetString("nowLevel")));//加载该关卡对应场景
    }
}
//       正常   受伤    最大值    最小值    碰撞体    x偏  y偏
//红鸟   159    66                          
//黄鸟   143    158                         0.23      0.01 -0.08
//绿鸟   106    121                         0.25      -0.1 -0.1
//白鸟   155    99                          0.3       0.1  -0.1
//小猪   103    104     7         4
//大猪   1      6       12        6
//木条   90     94      6         3
//木圈   4      11      6         3
//木块   89     160     6         3
//石块   50     53      10        5
//石条   106    108     10        5
//
//pause  63.5  -46
//