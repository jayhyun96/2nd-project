using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quiz
{
    [Header("문제 번호나 이름")]
    public string name;

    [Header("반드시 list[0] 문제, list[1] 정답, list[2] 오답")]
    public List<string> sentences;

    [Header("체크시 O가 정답, 미체크시 X가 정답")]
    public bool Answer;

}
