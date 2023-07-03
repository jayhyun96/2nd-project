using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

enum STAMP //배열이나 리스트 인덱스용
{
    ZERO,
    TIMEATTACK = 0,
    MONORAIL,
    OBSERVATION,
    PICTURE,
    QUIZ,
    SIZE
}

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    private bool[] stampContentsFinish;
    public bool[] StampContentsFinish { get => stampContentsFinish; }

    private float bestTime;
    private TextMeshProUGUI bestTimeText; //보여줄 곳 만들어야함

    ///모든 퀴즈와 대화를 완료했는지 체크 관리하는 변수들
    //모든 퀴즈 오브젝트 관리
    [SerializeField] private QuizTrigger[] quizTriggers;
    //모든 벽화 대화 오브젝트 관리
    [SerializeField] private DialogueTrigger[] dialogueTriggers;

    private void Start()
    {
        stampContentsFinish = new bool[(int)STAMP.SIZE]; //size is 5

        bestTime = float.PositiveInfinity;
        bestTimeText.text = "99:99";

        quizTriggers = FindObjectsOfType<QuizTrigger>(); //게임 하이에라키에 있는 해당 스크립트를 가진 변수들을 저장
        dialogueTriggers = FindObjectsOfType<DialogueTrigger>();

    }

    public void RenewalBestTime(float time)
    {
        if(bestTime > time) 
        {
            bestTime = time;

            int minutes = Mathf.FloorToInt(bestTime / 60f);
            int seconds = Mathf.FloorToInt(bestTime % 60f);
            bestTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void SetQuizClear()
    {
        //quizTriggers
    }

}

