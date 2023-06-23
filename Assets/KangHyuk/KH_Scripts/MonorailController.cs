using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonorailController : MonoBehaviour
{
    public List<Transform> waypoints;   // 웨이포인트들을 저장할 리스트
    public float speed = 5f;            // 이동 속도

    private int currentWaypointIndex = 0;   // 현재 웨이포인트 인덱스
    private bool isMoving = true;           // 이동 여부

    [SerializeField]
    private int targetWaypointIndex = 0;     // 이동할 웨이포인트의 인덱스

    public void MoveToWaypointIndex()
    {
        if (targetWaypointIndex >= 0 && targetWaypointIndex < waypoints.Count)
        {
            currentWaypointIndex = targetWaypointIndex;
            isMoving = true;
        }
        else
        {
            Debug.LogError("잘못된 웨이포인트 인덱스입니다.");
            return;
        }

        if (isMoving)
        {
            // 현재 웨이포인트
            Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;

            // 웨이포인트에 도착한 경우 이동 중지
            if (Vector3.Distance(transform.position, currentWaypointPosition) < 0.1f)
            {
                isMoving = false;
                //Debug.Log("도착!");
            }
            else
            {
                // 웨이포인트로 이동
                Vector3 direction = (currentWaypointPosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    private void Update()
    {
        MoveToWaypointIndex();
    }
}