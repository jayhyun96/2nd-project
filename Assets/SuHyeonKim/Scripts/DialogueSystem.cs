using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("�� Canvas �ڽ��� Dialougue�� �Ѱ� ��������")]
    [SerializeField] private GameObject dialogueObj;

    [Header("�� Canvas �ڽ��� Dialogue�� �ؽ�Ʈ�� �����ÿ�")]
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtSentence;    

    Queue<string> sentences = new Queue<string>();

    private bool isTyping;

    private string deqSentence;

    private DialogueTrigger lastCalled;

    private void Start()
    {
        isTyping = false;
    }

    public void Begin(DialogueTrigger dialogueTrigger)
    {
        lastCalled = dialogueTrigger;

        dialogueObj.SetActive(true);

        sentences.Clear();

        txtName.text = dialogueTrigger.name;

        foreach(var sentence in dialogueTrigger.info.sentences)
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
            isTyping=false;
            return;
        }            

        if(sentences.Count == 0)
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
        lastCalled.SetRead();
        txtSentence.text = string.Empty;
        dialogueObj.SetActive(false);
    }

    public bool isDialogueActivate()
    {
        if(dialogueObj.activeSelf)
            return true;
        else return false;

    }

}
