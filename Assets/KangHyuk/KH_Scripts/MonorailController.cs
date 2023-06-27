using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonorailController : MonoBehaviour
{
    public List<Transform> waypoints;   // ��������Ʈ���� ������ ����Ʈ
    public float speed = 5f;            // �̵� �ӵ�

    private int currentWaypointIndex = 0;   // ���� ��������Ʈ �ε���
    private bool isMoving = true;           // �̵� ����

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
            Debug.LogError("�߸��� ��������Ʈ �ε����Դϴ�.");
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
            // ���� ��������Ʈ
            Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;

            // ��������Ʈ�� ������ ��� �̵� ����
            if (Vector3.Distance(transform.position, currentWaypointPosition) < 0.1f)
            {
                isMoving = false;
                monorailAnim.SetBool("IsMoving", false);
                doorAnim.SetBool("isOpen", true);
                Debug.Log("����!");
                Invoke("CloseDoor", 5f);
            }
            else
            {
                // ��������Ʈ�� �̵�
                Vector3 direction = (currentWaypointPosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }
}