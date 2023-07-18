using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue info;

    private bool read;
    public bool Read { get => read; }

    [Header("�� Canvas �ڽ��� Dialougue�� �Ѱ� ��������")]
    [SerializeField] private GameObject dialogueObj;

    [Header("�� Canvas �ڽ��� Dialogue�� �ؽ�Ʈ�� �����ÿ�")]
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtSentence;

    Queue<string> sentences = new Queue<string>();

    private bool isTyping;

    private string deqSentence;

    [Header("������ ������ ��ȭ npc ����")]
    [SerializeField] GameObject nextTrigger;


    private void Start()
    {
        isTyping = false;
        read = false;
    }

    public void Begin()
    {
        dialogueObj.SetActive(true);

        sentences.Clear();

        txtName.text = info.name;

        foreach (var sentence in info.sentences)
        {
            sentences.Enqueue(sentence);
        }

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

        txtSentence.text = string.Empty;
        StopAllCoroutines();

        deqSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(deqSentence));

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
        SetRead();
        txtSentence.text = string.Empty;
        dialogueObj.SetActive(false);
    }

    public bool isDialogueActivate()
    {
        if (dialogueObj.activeSelf)
            return true;
        else return false;

    }

    public void Trigger()
    {
        if (isDialogueActivate())
            return;

        Begin();
    }

    public void OnMouseDown()
    {
        Trigger();
    }

    public void SetRead()
    {
        read = true;

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
