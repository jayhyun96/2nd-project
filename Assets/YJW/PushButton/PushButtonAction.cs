using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButtonAction : MonoBehaviour
{

    [Header("버튼의 원래위치")]
    [SerializeField] Vector3 popPosition = Vector3.zero;
    [Header("버튼을 눌렀을때 위치")]
    [SerializeField] Vector3 pickPosition = Vector3.zero;

    [Header("버튼이 내려갈 높이")]
    [Header("default 0.05f")]
    [SerializeField, Range(0, 0.1f)] float pickPositionY = 0f;

    [Header("버튼의 속도")]
    [Header("default: 10f")]
    [SerializeField, Range(0, 10f)] float buttonSpeed = 0f;


    [Header("버튼이 대기시간")]
    [Header("default: 1f")]
    [SerializeField, Range(0, 1f)] float buttonDealy = 0f;

    [SerializeField] private bool isPick = false;


    private void Awake()
    {
        popPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        pickPosition = new Vector3(transform.localPosition.x, pickPositionY, transform.localPosition.z);

    }

    public void StartCo()
    {
        Coroutine co = StartCoroutine(PushButton());
    }


    IEnumerator PushButton()
    {
        if (transform.position == popPosition && isPick == false)
        {
            isPick = true;
            while (transform.localPosition != pickPosition)
            {
                yield return new WaitForSeconds(0.01f);
                this.transform.position = Vector3.MoveTowards(transform.localPosition, pickPosition, buttonSpeed * Time.deltaTime);
                //this.transform.position = Vector3.Lerp(transform.localPosition, pickPosition, lerpRange);
            }
        }
        else if(transform.position == pickPosition && isPick == false)
        {
            while (transform.localPosition != popPosition)
            {
                isPick = true;
                yield return new WaitForSeconds(0.01f);
                this.transform.position = Vector3.MoveTowards(transform.localPosition, popPosition, buttonSpeed * Time.deltaTime);
                //this.transform.position = Vector3.Lerp(transform.localPosition, popPosition, lerpRange);
            }
        }
        yield return new WaitForSeconds(buttonDealy);
        isPick = false;

    }


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
