using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("각 Canvas 자식의 Dialogue의 텍스트를 넣으시오")]
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtSentence;

    [Header("각 Canvas 자식의 Dialougue를 켜고 끄기위함")]
    [SerializeField] private GameObject dialogueObj;

    Queue<string> sentences = new Queue<string>();

    private bool isTyping;

    private string deqSentence;

    private void Start()
    {
        isTyping = false;
    }

    public void Begin(Dialogue info)
    {
        dialogueObj.SetActive(true);

        sentences.Clear();

        txtName.text = info.name;

        foreach(var sentence in info.sentences)
        {
            sentences.Enqueue(sentence);
        }

        Next();
    }

    public void Next() //텍스트를 넘기기 위한 버튼 클릭함수
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
