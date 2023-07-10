using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimalMoveCtrl : MonoBehaviour
{
    public List<Transform> waypoints;   // 웨이포인트들을 저장할 리스트
    public float speed = 5f;            // 이동 속도

    [SerializeField] private int currentWaypointIndex = 0;   // 현재 웨이포인트 인덱스
    private bool isMoving = true;           // 이동 여부

    [SerializeField] private Vector3 lookAtPosition;

    [SerializeField] private AnimalAnimationCtrl anim = null;
    private AudioSource audioSource;
    public AudioClip[] animalSounds;
    [Header("회전 시간")]
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
        //디버깅용
        ++currentWaypointIndex;
        if (currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count)
        {

            AnimalSound();
            isMoving = true;
        }
        else
        {
            Debug.LogError("animal : 잘못된 웨이포인트 인덱스입니다.");
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
            // 현재 웨이포인트
            Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;
            // audioSource.transform.position = transform.position;

            // 웨이포인트에 도착한 경우 이동 중지
            if (Vector3.Distance(transform.position, currentWaypointPosition) < 0.1f)
            {

                isMoving = false;
                AnimalSound();
                anim.curAnimState = "Idle_A";
                anim.curShapeState = "Eyes_Annoyed";
                Debug.Log("도착!");
                timeCount = 0;

            }
            else
            {

                // 특정 지점을 향해 회전시킵니다.
                timeCount += Time.deltaTime;
                Vector3 rotateDirection = (currentWaypointPosition - this.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(rotateDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, timeCount);
                transform.rotation = new Quaternion(transform.rotation.x, targetRotation.y, transform.rotation.z, targetRotation.w);
                // transform.LookAt(currentWaypointPosition);
                // 타겟 방향으로 회전함
                //float angle = Mathf.Atan2(currentWaypointPosition.y, currentWaypointPosition.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                if (timeCount > rotationTime)
                {
                    // 웨이포인트로 이동
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