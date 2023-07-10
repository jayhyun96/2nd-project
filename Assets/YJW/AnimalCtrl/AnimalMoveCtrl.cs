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
    public AudioClip[] animalSounds;
    [Header("ȸ�� �ð�")]
    [SerializeField] private float rotationTime = 0f;
    [SerializeField] private float timeCount = 0f;
    [SerializeField] private float idleTimeCount = 0f;

    Vector3 rotateDirection = Vector3.zero;
    Quaternion lookRotation = Quaternion.identity;
    Quaternion targetRotation = Quaternion.identity;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<AnimalAnimationCtrl>();
    }

    public void AnimalSound()
    {
        //  AudioSource.PlayClipAtPoint(animalSounds[Random.Range(0, animalSounds.Length - 1)], transform.position);
    }
    public void MoveToWaypointIndex()
    {
        if (currentWaypointIndex == waypoints.Count - 1)
            return;
        //������
        ++currentWaypointIndex;
        if (currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count)
        {

            AnimalSound();
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
        if (!isMoving)
        {
            if (transform.rotation.y == targetRotation.y)
            {
                idleTimeCount = 0f;
                return;
            }

            lookAtPosition = ((GameObject.FindGameObjectWithTag("MainCamera").transform.position));
            idleTimeCount += Time.deltaTime;
            rotateDirection = (lookAtPosition - this.transform.position).normalized;
            lookRotation = Quaternion.LookRotation(rotateDirection);
            targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, idleTimeCount);
            transform.rotation = new Quaternion(transform.rotation.x, targetRotation.y, transform.rotation.z, targetRotation.w);
        }
        if (isMoving)
        {
            // ���� ��������Ʈ
            Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;
            // audioSource.transform.position = transform.position;

            // ��������Ʈ�� ������ ��� �̵� ����
            if (Vector3.Distance(transform.position, currentWaypointPosition) < 0.1f)
            {

                isMoving = false;
                AnimalSound();
                anim.curAnimState = "Idle_A";
                anim.curShapeState = "Eyes_Annoyed";
                Debug.Log("����!");
                timeCount = 0;

            }
            else
            {

                // Ư�� ������ ���� ȸ����ŵ�ϴ�.
                timeCount += Time.deltaTime;
                Vector3 rotateDirection = (currentWaypointPosition - this.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(rotateDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, timeCount);
                transform.rotation = new Quaternion(transform.rotation.x, targetRotation.y, transform.rotation.z, targetRotation.w);
                // transform.LookAt(currentWaypointPosition);
                // Ÿ�� �������� ȸ����
                //float angle = Mathf.Atan2(currentWaypointPosition.y, currentWaypointPosition.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                if (timeCount > rotationTime)
                {
                    // ��������Ʈ�� �̵�
                    anim.curAnimState = "Run";
                    anim.curShapeState = "Eyes_Dead";
                    Vector3 direction = (currentWaypointPosition - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;

                }
            }
            //transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform.position);
        }


    }



    private void OnTriggerEnter(Collider other)
    {
        if (isMoving) { return; }
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