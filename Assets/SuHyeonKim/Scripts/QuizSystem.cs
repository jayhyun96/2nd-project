using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class QuizSystem : MonoBehaviour
{
    [Header("�� Canvas �ڽ��� Quiz�� �Ѱ� ��������")]
    [SerializeField] private GameObject quizObj;

    [Header("�� Canvas �ڽ��� Quiz�� �ؽ�Ʈ�� �����ÿ�")]
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtSentence;    

    Queue<string> sentences = new Queue<string>();

    private bool isTyping;
    private bool isAnswer;
    private bool opened;

    private Quiz answerCheck;
    private string deqSentence;

    private QuizTrigger lastCalled;

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

    public void Begin(QuizTrigger quizTrigger)
    {
        lastCalled = quizTrigger;

        Init();

        quizObj.SetActive(true);

        sentences.Clear();

        txtName.text = quizTrigger.info.name;

        foreach (var sentence in quizTrigger.info.sentences)
        {
            sentences.Enqueue(sentence);
        }

        answerCheck = quizTrigger.info;

        Next();
    }

    public void Next() //�ؽ�Ʈ�� �ѱ�� ���� ��ư Ŭ���Լ�
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

    public void Correct()//Button O�� �ֱ�
    {
        isAnswer = true;

        Result(true);
    }

    public void Wrong()//Button X�� �ֱ�
    {
        isAnswer = true;

        Result(false);   
    }

    public void Result(bool answer)
    {
        if (!answerCheck.Answer.Equals(answer)) //������ �ƴϸ�
        {
            sentences.Dequeue();           
        }
        else //�����̸�
        {
            lastCalled.SetCleared();
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
