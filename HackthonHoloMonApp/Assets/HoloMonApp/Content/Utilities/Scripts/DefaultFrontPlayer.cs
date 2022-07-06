using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultFrontPlayer : MonoBehaviour
{
    public float Distance = 1.0f;

    void OnEnable()
    {
        // 有効時にプレイヤーのフロント位置に再設定する
        this.transform.position = Camera.main.transform.position
            + Camera.main.transform.forward * Distance;
        this.transform.LookAt(Camera.main.transform);
    }

    void OnDisable()
    {
        
    }
}
