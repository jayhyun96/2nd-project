using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

enum STAMP //�迭�̳� ����Ʈ �ε�����
{
    ZERO,
    TIMEATTACK = 0,
    MONORAIL,
    OBSERVATION,
    DIALOGUE,
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
    [SerializeField] private HandUIController HandUI;

    [SerializeField] private bool[] stampContentsFinish; //�����ִ°� �ƴ� ����ȭ ����
    public bool[] StampContentsFinish { get => stampContentsFinish; }

    private float bestTime;
    public TextMeshProUGUI bestTimeText; //������ �� ��������

    ///��� ����� ��ȭ�� �Ϸ��ߴ��� üũ �����ϴ� ������
    //��� ���� ������Ʈ ����
    [SerializeField] private QuizTrigger[] quizTriggers;
    //��� ��ȭ ��ȭ ������Ʈ ����
    [SerializeField] private DialogueTrigger[] dialogueTriggers;

    private void Awake()
    {
        quizTriggers = FindObjectsOfType<QuizTrigger>(); //���� ���̿���Ű�� �ִ� �ش� ��ũ��Ʈ�� ���� �������� ����
        dialogueTriggers = FindObjectsOfType<DialogueTrigger>();
    }

    private void Start()
    {
        stampContentsFinish = new bool[(int)STAMP.SIZE]; //size is 5

        bestTime = float.PositiveInfinity;
        bestTimeText.text = "99:99";           

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

    private void CheckAllQuizClear()
    {
        if (quizTriggers == null)
            return;

        foreach(var check in quizTriggers)
        {
            if(!check.Cleared) //�ϳ��� �ȸ���� ������
            {
                return;
            }
        }

        //stampContentsFinish[(int)STAMP.QUIZ] = true; //���� ������ ���� ����
        StampClear((int)STAMP.QUIZ);
    }

    private void CheckAllDialogueRead()
    {
        if (dialogueTriggers == null) 
            return;

        foreach(var check in dialogueTriggers)
        {
            if(!check.Read)
            {
                return;
            }
        }

        //stampContentsFinish[(int)STAMP.DIALOGUE] = true;
        StampClear((int)STAMP.DIALOGUE);
    }

    private void Update()
    {
        CheckAllQuizClear();
        CheckAllDialogueRead();
        HandUI.CollectStamp();
    }

    public void StampClear(int stampIdx)
    {
        stampContentsFinish[stampIdx] = true;
    }
}

