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
        //디버깅용
        ++currentWaypointIndex;
        if (currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count)
        {

            CatSound();
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
        transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform.position);

        if (isMoving)
        {
            // 현재 웨이포인트
            Vector3 currentWaypointPosition = waypoints[currentWaypointIndex].position;
            audioSource.transform.position = transform.position;

            // 웨이포인트에 도착한 경우 이동 중지
            if (Vector3.Distance(transform.position, currentWaypointPosition) < 0.1f)
            {
                
                isMoving = false;
                CatSound();
                anim.curAnimState = "Idle_A";
                anim.curShapeState = "Eyes_Annoyed";
                Debug.Log("도착!");


            }
            else
            { 
                transform.LookAt(currentWaypointPosition);
                // 웨이포인트로 이동
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