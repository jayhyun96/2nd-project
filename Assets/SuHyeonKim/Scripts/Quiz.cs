using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quiz
{
    [Header("���� ��ȣ�� �̸�")]
    public string name;

    [Header("�ݵ�� list[0] ����, list[1] ����, list[2] ����")]
    public List<string> sentences;

    [Header("üũ�� O�� ����, ��üũ�� X�� ����")]
    public bool Answer;

}
