using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonorailController : MonoBehaviour
{
    public List<Transform> waypoints;   // ��������Ʈ���� ������ ����Ʈ
    public float speed = 5f;            // �̵� �ӵ�

    private int currentWaypointIndex = 0;   // ���� ��������Ʈ �ε���
    private bool isMoving = true;           // �̵� ����

    [SerializeField]
    private int targetWaypointIndex = 0;     // �̵��� ��������Ʈ�� �ε���

    public void MoveToWaypointIndex()
    {
        if (targetWaypointIndex >= 0 && targetWaypointIndex < waypoints.Count)
        {
            currentWaypointIndex = targetWaypointIndex;
            isMoving = true;
        }
        else
        {
            Debug.LogError("�߸��� ��������Ʈ �ε����Դϴ�.");
            return;
        }

        if (isMoving)
        {
            // ���� ��������Ʈ
            Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;

            // ��������Ʈ�� ������ ��� �̵� ����
            if (Vector3.Distance(transform.position, currentWaypointPosition) < 0.1f)
            {
                isMoving = false;
                //Debug.Log("����!");
            }
            else
            {
                // ��������Ʈ�� �̵�
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