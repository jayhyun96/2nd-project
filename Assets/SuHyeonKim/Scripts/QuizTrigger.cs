using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    //사용 주의상황 : info의 sentences의 0번이 문제, 1번이 정답시, 2번이 오답시 문장을 출력한다.

    public Quiz info;

    private bool cleared;
    public bool Cleared { get => cleared; }

    [Header("각 Canvas 자식의 Quiz를 켜고 끄기위함")]
    [SerializeField] private GameObject quizObj;

    [Header("각 Canvas 자식의 Quiz의 텍스트를 넣으시오")]
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtSentence;

    Queue<string> sentences = new Queue<string>();

    private bool isTyping;
    private bool isAnswer;
    private bool opened;

    private Quiz answerCheck;
    private string deqSentence;

    [Header("다음에 보여줄 퀴즈npc 설정")]
    [SerializeField] GameObject nextTrigger;

    private void Init()
    {
        isTyping = false;
        isAnswer = false;
        opened = false;
    }

    private void Start()
    {
        Init();
        cleared = false;
    }

    public void Begin()
    {
        Init();

        quizObj.SetActive(true);

        sentences.Clear();

        txtName.text = info.name;

        foreach (var sentence in info.sentences)
        {
            sentences.Enqueue(sentence);
        }

        answerCheck = info;

        Next();
    }

    public void Next() //텍스트를 넘기기 위한 버튼 클릭함수
    {

        if (isTyping)
        {
            StopAllCoroutines();
            txtSentence.text = deqSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            End();
            return;
        }

        if (!opened || isAnswer)
        {
            txtSentence.text = string.Empty;
            StopAllCoroutines();

            deqSentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(deqSentence));

            opened = true;
        }



        if (isAnswer)
        {
            sentences.Clear();
        }
    }

    public void Correct()//Button O에 넣기
    {
        isAnswer = true;

        Result(true);
    }

    public void Wrong()//Button X에 넣기
    {
        isAnswer = true;

        Result(false);
    }

    public void Result(bool answer)
    {
        if (!answerCheck.Answer.Equals(answer)) //정답이 아니면
        {
            if (sentences.Count != 0)
                sentences.Dequeue();
        }
        else //정답이면
        {
            SetCleared();
        }

        Next();
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        foreach (var letter in sentence)
        {
            txtSentence.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    public void End()
    {
        txtSentence.text = string.Empty;
        quizObj.SetActive(false);
    }

    public bool isQuizActivate()
    {
        if (quizObj.activeSelf)
            return true;
        else return false;

    }

    public void Trigger()
    {
        if (isQuizActivate())
            return;

        Begin();
    }

    public void OnMouseDown() //for mouse
    {
        Trigger();
    }

    public void SetCleared()
    {
        cleared = true;

        Invoke("ReadAfterDisapear", 8.0f);
    }

    public void ReadAfterDisapear()
    {
        gameObject.SetActive(false);

        if (nextTrigger == null) //마지막 npc는 nextTrigger 없음
            return;

        nextTrigger.SetActive(true);
    }

}
