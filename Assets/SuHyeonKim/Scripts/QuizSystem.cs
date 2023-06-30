using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class QuizSystem : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtSentence;

    [SerializeField] private GameObject quizObj;

    Queue<string> sentences = new Queue<string>();

    private bool isTyping;
    private bool isAnswer;
    private bool opened;

    private Quiz answerCheck;
    private string deqSentence;

    private void Init()
    {
        isTyping = false;
        isAnswer = false;
        opened = false;
    }

    private void Start()
    {
        Init();
    }    

    public void Begin(Quiz info)
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
               
        if(!opened || isAnswer)
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
        if (!answerCheck.Answer.Equals(answer))
        {
            sentences.Dequeue();           
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
}
