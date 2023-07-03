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
    private TextMeshProUGUI bestTimeText; //������ �� ��������

    ///��� ����� ��ȭ�� �Ϸ��ߴ��� üũ �����ϴ� ������
    //��� ���� ������Ʈ ����
    [SerializeField] private QuizTrigger[] quizTriggers;
    //��� ��ȭ ��ȭ ������Ʈ ����
    [SerializeField] private DialogueTrigger[] dialogueTriggers;

    private void Start()
    {
        stampContentsFinish = new bool[(int)STAMP.SIZE]; //size is 5

        bestTime = float.PositiveInfinity;
        bestTimeText.text = "99:99";

        quizTriggers = FindObjectsOfType<QuizTrigger>(); //���� ���̿���Ű�� �ִ� �ش� ��ũ��Ʈ�� ���� �������� ����
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

