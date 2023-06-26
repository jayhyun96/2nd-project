using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TelescopeInteraction : XRSimpleInteractable
{
    public Camera playerCamera; // �÷��̾� ī�޶�
    public Camera telescopeCamera; // ������ ī�޶�
    public float telescopeFOV = 30f; // ������ ���� Ȯ�� ��

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

        // �÷��̾� ī�޶� ������ ī�޶� ��ġ�� ȸ������ �̵�
        playerCamera.transform.SetPositionAndRotation(telescopeCamera.transform.position, telescopeCamera.transform.rotation);

        // Ȯ�� ����
        telescopeCamera.fieldOfView = telescopeFOV;
        // �ٸ� ȯ�� ������...

        isTelescopeView = true;
    }

    public void ExitTelescopeView()
    {
        // ������ ī�޶� ��Ȱ��ȭ
        telescopeCamera.enabled = false;

        // �÷��̾� ī�޶� ���� ��ġ�� ȸ������ �ǵ���
        playerCamera.transform.SetPositionAndRotation(originalCameraPosition, originalCameraRotation);

        // Ȯ�� ���� �ʱ�ȭ
        telescopeCamera.fieldOfView = originalTelescopeFOV;
        // �ٸ� ȯ�� ���� �ʱ�ȭ...

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
        if (isTelescopeView && Input.GetButtonDown("Fire1"))
        {
            // Fire1 ��ư�� Ŭ���Ǹ� ������ ���� ����
            StopTelescopeView();
        }
    }
}
