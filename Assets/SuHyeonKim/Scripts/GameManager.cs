using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.PostProcessing;
using System;

enum STAMP //배열이나 리스트 인덱스용
{
    ZERO,
    TIMEATTACK = 0,
    MONORAIL,
    OBSERVATION,
    DIALOGUE,
    QUIZ,
    SIZE
}

enum SKY
{
    ZERO,
    DAY = 0,
    EVENING,
    NIGHT,
    SIZE
}

enum BGM
{
    ZERO,
    DAY = 0,
    EVENING,
    NIGHT,
    RUNNING,
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

    [SerializeField] private bool[] stampContentsFinish; //직접넣는거 아님 직렬화 뺄것
    public bool[] StampContentsFinish { get => stampContentsFinish; }

    private float bestTime;
    public TextMeshProUGUI bestTimeText; //보여줄 곳 만들어야함

    ///모든 퀴즈와 대화를 완료했는지 체크 관리하는 변수들
    //모든 퀴즈 오브젝트 관리
    [SerializeField] private QuizTrigger[] quizTriggers;
    //모든 벽화 대화 오브젝트 관리
    [SerializeField] private DialogueTrigger[] dialogueTriggers;

    [Header("포스트 프로세싱 관련")]
    [SerializeField] LightMngr lightMngr;
    public LightMngr LightMngr { get => lightMngr; }
    [SerializeField] private Light DLight;
    [SerializeField] private GameObject ppVolume;

    [Header("음악 아침 저녁 밤 끌어다 놓기")]
    [SerializeField] List<AudioClip> bgmList;
    public List<AudioClip> BgmList { get => bgmList; }
    private AudioSource bgm;

    private void Awake()
    {
        quizTriggers = FindObjectsOfType<QuizTrigger>(); //게임 하이에라키에 있는 해당 스크립트를 가진 변수들을 저장
        dialogueTriggers = FindObjectsOfType<DialogueTrigger>();
        bgm = gameObject.AddComponent<AudioSource>();

        //npc들 배열 순서정렬
        Array.Sort(quizTriggers, (a, b) =>
        {
            return a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex());
        });

        Array.Sort(dialogueTriggers, (a, b) =>
        {
            return a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex());
        });

    }

    private void Start()
    {
        stampContentsFinish = new bool[(int)STAMP.SIZE]; //size is 5

        bestTime = float.PositiveInfinity;
        bestTimeText.text = "99:99";
        ppVolume.SetActive(false);
        PlayBGM(BgmList[lightMngr.RotationSwitch]);

    }

    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmList.Count <= 0) return;
        else
        {
            bgm.clip = bgmClip;
            bgm.Play();
        }
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
            if(!check.Cleared) //하나라도 안맞춘게 있으면
            {
                return;
            }
        }

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

        StampClear((int)STAMP.DIALOGUE);
    }

    private void Update()
    {
        CheckAllQuizClear();
        CheckAllDialogueRead();
       
        if (HandUI != null)
        {
            HandUI.CollectStamp();
        }

        switch (lightMngr.RotationSwitch)
        {
            case (int)SKY.DAY:
                DLight.transform.eulerAngles = new Vector3(50, -30, 0);
                ppVolume.SetActive(false);
                break;
            case (int)SKY.EVENING:
                break;
            case (int)SKY.NIGHT:
                DLight.transform.eulerAngles = new Vector3(0, -30, 0);
                ppVolume.SetActive(true);
                break;
            default: break;
        }
    }

    public void StampClear(int stampIdx)
    {
        stampContentsFinish[stampIdx] = true;
    }
}

