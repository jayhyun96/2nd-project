using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue info;

    [SerializeField] private DialogueSystem dial;

    public void Trigger()
    {
        //var system = FindObjectOfType<DialogueSystem>();
        var system = dial;

        if (system.isDialogueActivate())
            return;

        system.Begin(info);
    }

    public void OnMouseDown()
    {
        Trigger();
    }
}
