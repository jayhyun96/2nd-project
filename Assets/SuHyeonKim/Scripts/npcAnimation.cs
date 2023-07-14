using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcAnimation : MonoBehaviour
{
    private Animator animator;

    [Header("NPC�� ���� Canvas�� �ڽ� ui�� �����ÿ�")]
    [SerializeField] private GameObject dial;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if(animator != null) 
        {
            if(dial.activeSelf)
            {
                animator.SetBool("Talk", true);
            }
            else
            {
                animator.SetBool("Talk", false);
            }
        }
    }
}
