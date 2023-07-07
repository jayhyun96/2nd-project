using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class LightMngr : MonoBehaviour
{
    [Header("==��� ������Ƽ�� ���콺�� ������ ��� ���� ���ɴϴ�.==")]
    [Space(30)]

    [Header("Skybox ���׸���")]
    [Tooltip("������ ������ ���׸����� �����ÿ�.")]
    [SerializeField] private Material dayBox = null;
    [Tooltip("���ķ� ������ ���׸����� �����ÿ�.")]
    [SerializeField] private Material noonBox = null;
    [Tooltip("������ ������ ���׸����� �����ÿ�.")]
    [SerializeField] private Material nightBox = null;

    [Space(20)]
    [Header("AddListener")]
    [SerializeField] private TelescopeInteraction telescopeInteraction = null;

    [Space(20)]

    [Header("Directional Light ���� ������ | default (0, 80, 0)")]
    [Tooltip("���� �� DirectionalLight�� ��ġ���� ����.")]
    [SerializeField] private Vector3 lightPosition = Vector3.zero;

    [Header("Directional Light ���� �����̼� | default (50, -30, 0)")]
    [Tooltip("���� �� DirectionalLight�� ȸ������ ����.")]
    [SerializeField] private Vector3 lightRotation = Vector3.zero; 

    [Header("Directional Light �����Ӵ� �����̼� ���� | default : 1")]
    [Tooltip("DirectionalLight�� Skybox�� ȸ���ӵ�. �������� ū������ ���� �������̳��� �������� �ε巯��")]
    [SerializeField, Range(0, 5f)] private float degreePerTime = 1f;

    //[Header("Directional Light �����̼� ���� ������ |  default : 0.03f")]
    //[Tooltip("�ʴ� �����ֱ⸦ ����. �������� ������, �������� ����")]
    //[SerializeField, Range(0, 10f)] private float rotateDealy = 0.03f;
    private float rotateDealy = 0f;
    [Header("Directional Light �����̼� ���� ������ |  default : 2.4f")]
    [Tooltip("�ʴ� �����ֱ⸦ ����. �������� ������, �������� ����")]
    [SerializeField, Range(0, 10f)] private float rotateDealySpeed = 0.03f;


    [Space(20)]
    [Header("=============== �� -> ���� ��ȯ property ===============")]
    [Header("���� skybox ���� �� | default : 4f")]
    [Tooltip("NoonSkybox�� ���Ⱚ�� ����. �������� ��� ���� ���� ��ο�")]
    [SerializeField, Range(0, 8)] private float noonboxExposure = 0f;
    [Header("���� skybox ��ǥ ���� | default : 5f")]
    [Tooltip("NoonSkybox�� ������ �������� ���� �� �ְ� ����")]
    [SerializeField, Range(1f, 20f)] private float dayToNoonboxTargetLightDegree = 0f;
    [Header("���� skybox ��ȯ ���� | default : 40f")]
    [Tooltip("DaySkybox�� NoonSkybox�� ��ȯ�� Ÿ�̹��� ����. x �����̼� ���� �������� ����")]
    [SerializeField, Range(20f, 50f)] private float dayToNoonboxChaneDegree = 0f;

    [Space(20)]
    [Header("=============== �� -> �� ��ȯ property ===============")]
    [Header("�� skybox ���� �� | default : 4f")]
    [Tooltip("DaySkybox�� ���Ⱚ�� ����. �������� ��� ���� ���� ��ο�")]
    [SerializeField, Range(0, 8)] private float dayboxExposure = 0f;
    [Header("�� skybox ��ȭ ���� �ð� | default : 1.4f")]
    [Tooltip("Skybox�� ��ȯ�Ǵ� �� �ð� ����")]
    [SerializeField, Range(0, 3f)] private float nightToDayboxProcessTime = 0f;
    [Header("�� skybox ��ȯ Ÿ�̹� | default : 0.5f")]
    [Tooltip("nightToDayboxProcessTime �� ������ �ð��� ��ī�̹ڽ��� ��ü��. nightToDayboxProcessTime ���ٴ� ���ƾ���")]
    [SerializeField, Range(1, 2f)] private float nightToDayboxChaneTime = 0f;

    [Space(20)]
    [Header("=============== ���� -> �� ��ȯ property ===============")]
    [Header("�� skybox ���� �� | default : 0f")]
    [Tooltip("NightSkybox�� ���Ⱚ�� ����. �������� ��� ���� ���� ��ο�")]
    [SerializeField, Range(0, 8)] private float nightboxExposure = 0f;
    [Header("�� skybox ��ȭ ���� �ð� | default : 1.45f")]
    [Tooltip("Skybox�� ��ȯ�Ǵ� �� �ð� ����")]
    [SerializeField, Range(0, 3f)] private float noonToNightboxProcessTime = 0f;
    [Header("�� skybox ��ȯ Ÿ�̹� | default : 0.04f")]
    [Tooltip("noonToNightboxProcessTime �� ������ �ð��� ��ī�̹ڽ��� ��ü��. noonToNightboxProcessTime ���ٴ� ���ƾ���")]
    [SerializeField, Range(0, 3f)] private float noonToNightboxChaneTime = 0f;

    [Space(20)]
    [Header("������================")]
    [Header("localrotation | ���۰���")]
    [SerializeField] float startRotationY = 0f;
    [Header("rotateTimer | �����̼� Ÿ�̸�")]
    [SerializeField] float rotateTimer = 0f;
    [Header("���� �۵��� Ȯ��")]
    [SerializeField] private bool isRotate = false;
    //[Header("���� �۵��� Ȯ��")]
    //[SerializeField] float curRotationX = 0f;

    [Header("���� ���൵ | ���� : 0 / ���� : 1 / �� : 2")]
    [SerializeField] int rotationSwitch = 0;
    public int RotationSwitch { get => rotationSwitch; }

    //[SerializeField] float compStartRotationY = 0f;
    //[SerializeField] float rotationResult = 0f;

    private void Awake()
    {
        this.transform.position = lightPosition;
        this.transform.rotation = Quaternion.Euler(lightRotation);
        dayBox.SetFloat("_Exposure", 1);
        noonBox.SetFloat("_Exposure", 1);
        nightBox.SetFloat("_Exposure", 1);

        telescopeInteraction.selectEntered.AddListener(StartCo);
    }
    private void Update()
    {
        // curRotationX = transform.eulerAngles.x; 
    }
    public void StartCo()
    {
        Coroutine co = StartCoroutine(RotateLight());
    }
    public void StartCo(SelectEnterEventArgs args)
    {
        Coroutine co = StartCoroutine(RotateLight());
    }

    IEnumerator RotateLight()
    {

        if (!isRotate)
        {
            isRotate = true;
            switch (rotationSwitch)
            {
                case 0:
                    Coroutine _coNoon = StartCoroutine(TurnNoonLight());
                    break;
                case 1:
                    Coroutine _coNight = StartCoroutine(TurnNightLight());
                    break;
                case 2:
                    Coroutine _coDay = StartCoroutine(TurnDayLight());
                    break;
            }
            rotateTimer = 0f;
        }
        yield return null;
    }



    private IEnumerator TurnDayLight()
    {
        dayBox.SetFloat("_Exposure", dayboxExposure);
        while (rotateTimer < nightToDayboxProcessTime)
        {
            rotateDealy = rotateDealySpeed * Time.deltaTime;
            float skyboxExposure = RenderSettings.skybox.GetFloat("_Exposure");

            if (skyboxExposure > dayboxExposure - 1f)
            {
                RenderSettings.skybox = dayBox;
                rotationSwitch = 0;
                nightBox.SetFloat("_Exposure", 0f);
            }

            if (RenderSettings.skybox == nightBox && rotateTimer > nightToDayboxChaneTime)
            {
                if (skyboxExposure < dayboxExposure)
                {
                    yield return new WaitForSeconds(rotateDealy);
                    skyboxExposure += 0.4f;
                    RenderSettings.skybox.SetFloat("_Exposure", skyboxExposure);
                }
            }

            if (RenderSettings.skybox == dayBox)
            {
                if (skyboxExposure > 1.2f)
                {
                    yield return new WaitForSeconds(rotateDealy);
                    skyboxExposure -= 0.2f;
                    RenderSettings.skybox.SetFloat("_Exposure", skyboxExposure);
                }
            }
            rotateTimer += Time.fixedDeltaTime;
            yield return new WaitForSeconds(rotateDealy);
            this.transform.Rotate(Vector3.left * degreePerTime);
            float skyboxRotate = RenderSettings.skybox.GetFloat("_Rotation");
            skyboxRotate += 1f;
            RenderSettings.skybox.SetFloat("_Rotation", skyboxRotate);

        }
        Debug.Log("rotate 2");
    

        isRotate = false;
    }

    private IEnumerator TurnNoonLight()
    {
        
        noonBox.SetFloat("_Exposure", noonboxExposure);
        while (transform.eulerAngles.x > dayToNoonboxTargetLightDegree)
        {
            rotateDealy = rotateDealySpeed * Time.deltaTime;
            float skyboxExposure = RenderSettings.skybox.GetFloat("_Exposure");

            if (skyboxExposure > noonboxExposure -1)
            {
                RenderSettings.skybox = noonBox;
                ++rotationSwitch;
                dayBox.SetFloat("_Exposure", 1f);
            }

            if (RenderSettings.skybox == dayBox && transform.eulerAngles.x < dayToNoonboxChaneDegree)
            {
                if (skyboxExposure < noonboxExposure)
                {
                    yield return new WaitForSeconds(rotateDealy);
                    skyboxExposure += 0.4f;
                    RenderSettings.skybox.SetFloat("_Exposure", skyboxExposure);
                }
            }

            if (RenderSettings.skybox == noonBox)
            {
                if (skyboxExposure > 1.3f)
                {
                    yield return new WaitForSeconds(rotateDealy);
                    skyboxExposure -= 0.3f;
                    RenderSettings.skybox.SetFloat("_Exposure", skyboxExposure);
                }
            }

            // rotateTimer += Time.deltaTime;
            yield return new WaitForSeconds(rotateDealy);
            this.transform.Rotate(Vector3.left * degreePerTime);
            float skyboxRotate = RenderSettings.skybox.GetFloat("_Rotation");
            skyboxRotate += 1f;
            RenderSettings.skybox.SetFloat("_Rotation", skyboxRotate);
        }
        Debug.Log("rotate 0");
        

        isRotate = false;
    }

    private IEnumerator TurnNightLight()
    {
        nightBox.SetFloat("_Exposure", nightboxExposure);

        while (rotateTimer < noonToNightboxProcessTime)
        {
            rotateDealy = rotateDealySpeed * Time.deltaTime;
            float skyboxExposure = RenderSettings.skybox.GetFloat("_Exposure");

            if (skyboxExposure < 0.04f)
            {
                RenderSettings.skybox = nightBox;
                ++rotationSwitch;
                noonBox.SetFloat("_Exposure", 4f);
            }

            if (RenderSettings.skybox == noonBox)
            {
                if (skyboxExposure < 2f)
                {
                    yield return new WaitForSeconds(rotateDealy);
                    skyboxExposure -= 0.2f;
                    RenderSettings.skybox.SetFloat("_Exposure", skyboxExposure);
                }
            }

            if (RenderSettings.skybox == nightBox)
            {
                if (skyboxExposure < 1f)
                {
                    yield return new WaitForSeconds(rotateDealy);
                    skyboxExposure += noonToNightboxChaneTime;
                    RenderSettings.skybox.SetFloat("_Exposure", skyboxExposure);
                }
            }

            rotateTimer += Time.fixedDeltaTime;
            yield return new WaitForSeconds(rotateDealy);
            this.transform.Rotate(Vector3.left * degreePerTime);
            float skyboxRotate = RenderSettings.skybox.GetFloat("_Rotation");
            skyboxRotate += 1f;
            RenderSettings.skybox.SetFloat("_Rotation", skyboxRotate);


        }
        Debug.Log("rotate 1");
        rotateTimer = 0;


        isRotate = false;
    }

}
