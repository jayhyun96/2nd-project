using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    //��� ���ǻ�Ȳ : info�� sentences�� 0���� ����, 1���� �����, 2���� ����� ������ ����Ѵ�.

    public Quiz info;

    private bool cleared;
    public bool Cleared { get => cleared; }

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

    [Header("������ ������ ����npc ����")]
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
            if (sentences.Count != 0)
                sentences.Dequeue();
        }
        else //�����̸�
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

        if (nextTrigger == null) //������ npc�� nextTrigger ����
            return;

        nextTrigger.SetActive(true);
    }

}
