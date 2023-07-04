using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TelescopeInteraction : XRSimpleInteractable
{
    public Camera playerCamera; // �÷��̾� ī�޶�
    public Camera telescopeCamera; // ������ ī�޶�

    private bool isTelescopeView = false; // ���� ������ �������� ����
    private Vector3 originalCameraPosition; // ������ �������� ��ȯ�ϱ� �� �÷��̾� ī�޶��� ��ġ
    private Quaternion originalCameraRotation; // ������ �������� ��ȯ�ϱ� �� �÷��̾� ī�޶��� ȸ��
    private float originalTelescopeFOV; // ���� ������ ������ Ȯ�� ��

    private void Start()
    {
        originalTelescopeFOV = telescopeCamera.fieldOfView;
        telescopeCamera.enabled = false; // ���� �� ������ ī�޶� ��Ȱ��ȭ
        // �ʱ�ȭ ���� ��...
    }

    public void EnterTelescopeView()
    {
        // ������ ī�޶� Ȱ��ȭ
        telescopeCamera.enabled = true;

        // �÷��̾� ī�޶��� �ʱ� ��ġ�� ȸ���� ����
        originalCameraPosition = playerCamera.transform.position;
        originalCameraRotation = playerCamera.transform.rotation;

        // �ٸ� ȯ�� ������...
        isTelescopeView = true;
    }

    public void ExitTelescopeView()
    {
        // ������ ī�޶� ��Ȱ��ȭ
        telescopeCamera.enabled = false;

        // �÷��̾� ī�޶� ���� ��ġ�� ȸ������ �ǵ���
        playerCamera.transform.SetPositionAndRotation(originalCameraPosition, originalCameraRotation);

        isTelescopeView = false;
    }

    public void StartTelescopeView()
    {
        if (!isTelescopeView)
        {
            // ��ȣ�ۿ��� ���۵� �� ������ �������� ��ȯ
            EnterTelescopeView();
        }
    }

    public void StopTelescopeView()
    {
        if (isTelescopeView)
        {
            // ��ȣ�ۿ��� ����� �� �÷��̾� ī�޶�� ���ƿ�
            ExitTelescopeView();
        }
    }

    private void Update()
    {
        if (isTelescopeView)
        {
            // �÷��̾��� �Ӹ� ȸ�� ���� ������ ī�޶� ����
            telescopeCamera.transform.rotation = playerCamera.transform.rotation;

            if (Input.GetButtonDown("Fire1"))
            {
                // ��Ʈ�ѷ��� Fire1 ��ư�� ������ �� ������ ���� ����
                StopTelescopeView();
            }
        }
    }
}