using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue info;

    [Header("���̾�α� �ý����� �����ÿ�")]
    [SerializeField] private DialogueSystem dial;
    private bool read;
    public bool Read { get => read; }

    private void Start()
    {
        read = false;
    }

    public void Trigger()
    {
        //var system = FindObjectOfType<DialogueSystem>();
        var system = dial;

        if (system.isDialogueActivate())
            return;

        system.Begin(info, this);
    }

    public void OnMouseDown()
    {
        Trigger();
    }

    public void SetRead()
    {
        read = true;
    }
}
