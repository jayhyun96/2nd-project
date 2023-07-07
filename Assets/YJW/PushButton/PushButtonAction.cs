using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PushButtonAction : MonoBehaviour
{

    [Header("��ư�� ������ġ")]
    [SerializeField] Vector3 popPosition = Vector3.zero;
    [Header("��ư�� �������� ��ġ")]
    [SerializeField] Vector3 pickPosition = Vector3.zero;

    [Header("��ư�� ������ ����")]
    [Header("default 0.05f")]
    [SerializeField, Range(0, 0.1f)] float pickPositionY = 0f;

    [Header("��ư�� �ӵ�")]
    [Header("default: 10f")]
    [SerializeField, Range(0, 10f)] float buttonSpeed = 0f;


    [Header("��ư�� ���ð�")]
    [Header("default: 1f")]
    [SerializeField, Range(0, 1f)] float buttonDealy = 0f;

    [SerializeField] private bool isPushing = false;
    public bool isOn = false;


    private void Awake()
    {
        popPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        pickPosition = new Vector3(transform.localPosition.x, pickPositionY, transform.localPosition.z);

    }

    //public void StartCo()
    //{
    //    Coroutine co = StartCoroutine(PushButton());
    //}

    public void PushButton()
    {
        if (isOn)
            return;

        if (!isPushing)
        {
            
            isPushing = true;
            if (transform.localPosition == popPosition)
            {
                Coroutine pushOn = StartCoroutine(PushOn());
            }
            //else if (transform.localPosition == pickPosition)
            //{
            //    Coroutine pushOff = StartCoroutine(PushOff());
            //}
        }
    }

    public IEnumerator PushOn()
    {
       // isPushing = true;
        while (transform.localPosition != pickPosition)
        {
            yield return new WaitForSeconds(0.01f);
            this.transform.localPosition = Vector3.MoveTowards(transform.localPosition, pickPosition, buttonSpeed * Time.deltaTime);
            //this.transform.position = Vector3.Lerp(transform.localPosition, pickPosition, lerpRange);
        }
        isPushing = false;
        isOn = true;
    }

    public IEnumerator PushOff()
    {
        //isPushing = true;
        while (transform.localPosition != popPosition)
        {  
            yield return new WaitForSeconds(0.01f);
            this.transform.localPosition = Vector3.MoveTowards(transform.localPosition, popPosition, buttonSpeed * Time.deltaTime);
            //this.transform.position = Vector3.Lerp(transform.localPosition, popPosition, lerpRange);
        }
        isPushing = false;
        isOn = false;
    }

    //IEnumerator PushButton()
    //{
    //    if (!isPushing)
    //    {
    //        if (transform.localPosition == popPosition)
    //        {
    //            isPushing = true;
    //            while (transform.localPosition != pickPosition)
    //            {
    //                yield return new WaitForSeconds(0.01f);
    //                this.transform.localPosition = Vector3.MoveTowards(transform.localPosition, pickPosition, buttonSpeed * Time.deltaTime);
    //                //this.transform.position = Vector3.Lerp(transform.localPosition, pickPosition, lerpRange);
    //            }
    //        }
    //        else if (transform.localPosition == pickPosition)
    //        {
    //            while (transform.localPosition != popPosition)
    //            {
    //                isPushing = true;
    //                yield return new WaitForSeconds(0.01f);
    //                this.transform.localPosition = Vector3.MoveTowards(transform.localPosition, popPosition, buttonSpeed * Time.deltaTime);
    //                //this.transform.position = Vector3.Lerp(transform.localPosition, popPosition, lerpRange);
    //            }
    //        }
    //    }
    //    yield return new WaitForSeconds(buttonDealy);
    //    isPushing = false;

    //}


    //public void ButtonPosition()
    //{
    //    if (transform.position == popPosition && isPick == false)
    //    {
    //        isPick = true;
    //        while (transform.localPosition != pickPosition)
    //        {

    //            this.transform.position = Vector3.MoveTowards(transform.localPosition, pickPosition, buttonSpeed);
    //            //this.transform.position = Vector3.Lerp(transform.localPosition, pickPosition, lerpRange);
    //        }
    //        isPick = false;
    //    }
    //    else
    //    {
    //        while (transform.localPosition != popPosition && isPick == false)
    //        {
    //            isPick = true;
    //            this.transform.position = Vector3.MoveTowards(transform.localPosition, popPosition, buttonSpeed);
    //            //this.transform.position = Vector3.Lerp(transform.localPosition, popPosition, lerpRange);
    //        }
    //        isPick = false;
    //    }
    //}
}
