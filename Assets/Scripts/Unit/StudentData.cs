using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Academy
{
    Millennium,
    Gehenna,
    Trinity,
}

public enum Club
{
    Seminar,
}

[CreateAssetMenu(fileName = "NewStudentData", menuName = "Unit/Student")]
public class StudentData : ScriptableObject
{
    [Header("기본 정보")]
    public string studentName;
    public Sprite icon;
    public GameObject prefab;
    public int starGrade;   // 1 ~ 3성

    [Header("스탯")]
    public int MaxHP;
    public int MaxMP;
    public int ATK;
    public int DEF;
    public float AtkSpeed;
    public float MoveSpeed;
    public float CritRate;
    public float CritDamage;

    [Header("시너지")]
    public Academy academy;
    public Club club;
}
