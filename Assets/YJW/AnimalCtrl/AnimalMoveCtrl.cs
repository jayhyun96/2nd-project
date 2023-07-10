using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimalMoveCtrl : MonoBehaviour
{
    public List<Transform> waypoints;   // ��������Ʈ���� ������ ����Ʈ
    public float speed = 5f;            // �̵� �ӵ�

    [SerializeField] private int currentWaypointIndex = 0;   // ���� ��������Ʈ �ε���
    private bool isMoving = true;           // �̵� ����

    [SerializeField] private Vector3 lookAtPosition;

    [SerializeField] private AnimalAnimationCtrl anim = null;
    private AudioSource audioSource;
    public AudioClip[] catSounds;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<AnimalAnimationCtrl>();
    }

    public void CatSound()
    {
      //  AudioSource.PlayClipAtPoint(catSounds[Random.Range(0, catSounds.Length - 1)], transform.position);
    }
    public void MoveToWaypointIndex()
    {
        if (currentWaypointIndex == waypoints.Count - 1)
            return;
        //������
        ++currentWaypointIndex;
        if (currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count)
        {

            CatSound();
            isMoving = true;
        }
        else
        {
            Debug.LogError("animal : �߸��� ��������Ʈ �ε����Դϴ�.");
            return;
        }
        
    }

    private void Update()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform.position);

        if (isMoving)
        {
            // ���� ��������Ʈ
            Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;
            audioSource.transform.position = transform.position;

            // ��������Ʈ�� ������ ��� �̵� ����
            if (Vector3.Distance(transform.position, currentWaypointPosition) < 0.1f)
            {
                
                isMoving = false;
                CatSound();
                anim.curAnimState = "Idle_A";
                anim.curShapeState = "Eyes_Annoyed";
                Debug.Log("����!");


            }
            else
            { 
                transform.LookAt(currentWaypointPosition);
                // ��������Ʈ�� �̵�
                anim.curAnimState = "Run";
                anim.curShapeState = "Eyes_Dead";
                Vector3 direction = (currentWaypointPosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (currentWaypointIndex == waypoints.Count - 1)
            return;

        if (other.CompareTag("Player"))
        {
            MoveToWaypointIndex();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lookAtPosition = waypoints[currentWaypointIndex].position;
        }
    }

}