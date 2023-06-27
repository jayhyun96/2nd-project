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

    private AudioSource audioSource;
    public AudioClip dingdong;
    public AudioClip monorailMove;
    public AudioClip doorOpen;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void doorSound()
    {
        AudioSource.PlayClipAtPoint(doorOpen, transform.position);
    }
    public void MoveToWaypointIndex(int waypointIndex)
    {
        if (waypointIndex >= 0 && waypointIndex < waypoints.Count)
        {
            currentWaypointIndex = waypointIndex;
            doorAnim.SetBool("isOpen", false);
            doorSound();
            isMoving = true;
            StartMovingSound();
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
        doorSound();
    }

    public void CloseDoor()
    {
        doorAnim.SetBool("isOpen", false);
        doorSound();
    }

    public void StartMovingSound()
    {
        // �Ҹ� ����ϱ�
        audioSource.clip = monorailMove;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMovingSound()
    {
        // �Ҹ� �����ϱ�
        audioSource.Stop();

    }

    private void Update()
    {
        if (isMoving)
        {
            // ���� ��������Ʈ
            Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;
            audioSource.transform.position = transform.position;

            // ��������Ʈ�� ������ ��� �̵� ����
            if (Vector3.Distance(transform.position, currentWaypointPosition) < 0.1f)
            {
                isMoving = false;
                StopMovingSound();
                AudioSource.PlayClipAtPoint(dingdong, transform.position);
                monorailAnim.SetBool("IsMoving", false);
                doorAnim.SetBool("isOpen", true);
                Debug.Log("����!");
                Invoke("CloseDoor", 4f);
                Invoke("doorSound", 4f);
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