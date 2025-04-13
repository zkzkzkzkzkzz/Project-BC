using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }



    /// <summary>
    /// 
    /// </summary>
    public void BattleStart()
    {
        Debug.Log("전투 시작");
    }
}
