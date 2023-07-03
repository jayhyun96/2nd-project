using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Header("NPC 이름")]    
    public string name;

    [Header("한번에 보여줄 문장 리스트")]
    public List<string> sentences;

}
