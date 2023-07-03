using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    //��� ���ǻ�Ȳ : info�� sentences�� 0���� ����, 1���� �����, 2���� ����� ������ ����Ѵ�.

    public Quiz info;

    [SerializeField] private QuizSystem quiz;

    public void Trigger()
    {
        //var system = FindObjectOfType<DialogueSystem>();
        var system = quiz;

        if (system.isQuizActivate())
            return;

        system.Begin(info);
    }

    public void OnMouseDown()
    {
        Trigger();
    }
}
