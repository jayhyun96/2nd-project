using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TelescopeInteraction : XRSimpleInteractable
{
    public Camera playerCamera; // 플레이어 카메라
    public Camera telescopeCamera; // 망원경 카메라

    private bool isTelescopeView = false; // 현재 망원경 시점인지 여부
    private Vector3 originalCameraPosition; // 망원경 시점으로 전환하기 전 플레이어 카메라의 위치
    private Quaternion originalCameraRotation; // 망원경 시점으로 전환하기 전 플레이어 카메라의 회전
    private float originalTelescopeFOV; // 원래 망원경 시점의 확대 값

    private void Start()
    {
        originalTelescopeFOV = telescopeCamera.fieldOfView;
        telescopeCamera.enabled = false; // 시작 시 망원경 카메라를 비활성화
        // 초기화 로직 등...
    }

    public void EnterTelescopeView()
    {
        // 망원경 카메라를 활성화
        telescopeCamera.enabled = true;

        // 플레이어 카메라의 초기 위치와 회전을 저장
        originalCameraPosition = playerCamera.transform.position;
        originalCameraRotation = playerCamera.transform.rotation;

        // 다른 환경 설정들...
        isTelescopeView = true;
    }

    public void ExitTelescopeView()
    {
        // 망원경 카메라를 비활성화
        telescopeCamera.enabled = false;

        // 플레이어 카메라를 이전 위치와 회전으로 되돌림
        playerCamera.transform.SetPositionAndRotation(originalCameraPosition, originalCameraRotation);

        isTelescopeView = false;
    }

    public void StartTelescopeView()
    {
        if (!isTelescopeView)
        {
            // 상호작용이 시작될 때 망원경 시점으로 전환
            EnterTelescopeView();
        }
    }

    public void StopTelescopeView()
    {
        if (isTelescopeView)
        {
            // 상호작용이 종료될 때 플레이어 카메라로 돌아옴
            ExitTelescopeView();
        }
    }

    private void Update()
    {
        if (isTelescopeView)
        {
            // 플레이어의 머리 회전 값을 망원경 카메라에 적용
            telescopeCamera.transform.rotation = playerCamera.transform.rotation;

            if (Input.GetButtonDown("Fire1"))
            {
                // 컨트롤러의 Fire1 버튼을 눌렀을 때 망원경 시점 종료
                StopTelescopeView();
            }
        }
    }
}