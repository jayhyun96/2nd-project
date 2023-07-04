using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    //사용 주의상황 : info의 sentences의 0번이 문제, 1번이 정답시, 2번이 오답시 문장을 출력한다.

    public Quiz info;

    [Header("퀴즈 시스템을 넣으시오")]
    [SerializeField] private QuizSystem quiz;
    private bool cleared;
    public bool Cleared { get => cleared; }

    private void Start()
    {
        cleared = false;
    }

    public void Trigger()
    {
        //var system = FindObjectOfType<DialogueSystem>();
        var system = quiz;

        if (system.isQuizActivate())
            return;

        system.Begin(info, this);
    }

    public void OnMouseDown() //for mouse
    {
        Trigger();
    }

    public void SetCleared()
    {
        cleared = true;
    }
}
