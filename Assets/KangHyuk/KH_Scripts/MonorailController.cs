using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonorailController : MonoBehaviour
{
    public List<Transform> waypoints;   // 웨이포인트들을 저장할 리스트
    public float speed = 5f;            // 이동 속도

    private int currentWaypointIndex = 0;   // 현재 웨이포인트 인덱스
    private bool isMoving = true;           // 이동 여부

    public Animator monorailAnim;
    public Animator doorAnim;

    public void MoveToWaypointIndex(int waypointIndex)
    {
        if (waypointIndex >= 0 && waypointIndex < waypoints.Count)
        {
            currentWaypointIndex = waypointIndex;
            doorAnim.SetBool("isOpen", false);
            isMoving = true;
            monorailAnim.SetBool("IsMoving", true);
        }
        else
        {
            Debug.LogError("잘못된 웨이포인트 인덱스입니다.");
            return;
        }
    }

    public void OpenDoor()
    {
        if (isMoving)
        {
            return;
        }

        doorAnim.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        doorAnim.SetBool("isOpen", false);
    }

    private void Update()
    {
        if (isMoving)
        {
            // 현재 웨이포인트
            Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;

            // 웨이포인트에 도착한 경우 이동 중지
            if (Vector3.Distance(transform.position, currentWaypointPosition) < 0.1f)
            {
                isMoving = false;
                monorailAnim.SetBool("IsMoving", false);
                doorAnim.SetBool("isOpen", true);
                Debug.Log("도착!");
                Invoke("CloseDoor", 5f);
            }
            else
            {
                // 웨이포인트로 이동
                Vector3 direction = (currentWaypointPosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }
}