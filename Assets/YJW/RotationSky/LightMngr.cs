using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class LightMngr : MonoBehaviour
{
    [Header("==모든 프로퍼티는 마우스를 가져다 대면 툴팁 나옵니다.==")]
    [Space(30)]

    [Header("Skybox 마테리얼")]
    [Tooltip("낮으로 지정할 마테리얼을 넣으시오.")]
    [SerializeField] private Material dayBox = null;
    [Tooltip("오후로 지정할 마테리얼을 넣으시오.")]
    [SerializeField] private Material noonBox = null;
    [Tooltip("밤으로 지정할 마테리얼을 넣으시오.")]
    [SerializeField] private Material nightBox = null;

    [Space(20)]
    [Header("AddListener")]
    [SerializeField] private TelescopeInteraction telescopeInteraction = null;

    [Space(20)]

    [Header("Directional Light 월드 포지션 | default (0, 80, 0)")]
    [Tooltip("시작 시 DirectionalLight의 위치값를 지정.")]
    [SerializeField] private Vector3 lightPosition = Vector3.zero;

    [Header("Directional Light 월드 로테이션 | default (50, -30, 0)")]
    [Tooltip("시작 시 DirectionalLight의 회전값을 지정.")]
    [SerializeField] private Vector3 lightRotation = Vector3.zero; 

    [Header("Directional Light 프레임당 로테이션 각도 | default : 1")]
    [Tooltip("DirectionalLight와 Skybox의 회전속도. 높을수록 큰각도를 돌아 프레임이낮고 낮을수록 부드러움")]
    [SerializeField, Range(0, 5f)] private float degreePerTime = 1f;

    //[Header("Directional Light 로테이션 갱신 딜레이 |  default : 0.03f")]
    //[Tooltip("초당 갱신주기를 설정. 높을수록 느리고, 낮을수록 빠름")]
    //[SerializeField, Range(0, 10f)] private float rotateDealy = 0.03f;
    private float rotateDealy = 0f;
    [Header("Directional Light 로테이션 갱신 딜레이 |  default : 2.4f")]
    [Tooltip("초당 갱신주기를 설정. 높을수록 느리고, 낮을수록 빠름")]
    [SerializeField, Range(0, 10f)] private float rotateDealySpeed = 0.03f;


    [Space(20)]
    [Header("=============== 낮 -> 오후 전환 property ===============")]
    [Header("오후 skybox 노출 값 | default : 4f")]
    [Tooltip("NoonSkybox의 노출값을 설정. 높을수록 밝고 낮을 수록 어두움")]
    [SerializeField, Range(0, 8)] private float noonboxExposure = 0f;
    [Header("오후 skybox 목표 각도 | default : 5f")]
    [Tooltip("NoonSkybox를 지정된 각도에서 멈출 수 있게 설정")]
    [SerializeField, Range(1f, 20f)] private float dayToNoonboxTargetLightDegree = 0f;
    [Header("오후 skybox 전환 각도 | default : 40f")]
    [Tooltip("DaySkybox와 NoonSkybox의 전환할 타이밍을 설정. x 로테이션 값을 기준으로 설정")]
    [SerializeField, Range(20f, 50f)] private float dayToNoonboxChaneDegree = 0f;

    [Space(20)]
    [Header("=============== 밤 -> 낮 전환 property ===============")]
    [Header("낮 skybox 노출 값 | default : 4f")]
    [Tooltip("DaySkybox의 노출값을 설정. 높을수록 밝고 낮을 수록 어두움")]
    [SerializeField, Range(0, 8)] private float dayboxExposure = 0f;
    [Header("낮 skybox 변화 과정 시간 | default : 1.4f")]
    [Tooltip("Skybox가 전환되는 총 시간 설정")]
    [SerializeField, Range(0, 3f)] private float nightToDayboxProcessTime = 0f;
    [Header("낮 skybox 전환 타이밍 | default : 0.5f")]
    [Tooltip("nightToDayboxProcessTime 중 정해진 시간에 스카이박스를 교체함. nightToDayboxProcessTime 보다는 낮아야함")]
    [SerializeField, Range(1, 2f)] private float nightToDayboxChaneTime = 0f;

    [Space(20)]
    [Header("=============== 오후 -> 밤 전환 property ===============")]
    [Header("밤 skybox 노출 값 | default : 0f")]
    [Tooltip("NightSkybox의 노출값을 설정. 높을수록 밝고 낮을 수록 어두움")]
    [SerializeField, Range(0, 8)] private float nightboxExposure = 0f;
    [Header("밤 skybox 변화 과정 시간 | default : 1.45f")]
    [Tooltip("Skybox가 전환되는 총 시간 설정")]
    [SerializeField, Range(0, 3f)] private float noonToNightboxProcessTime = 0f;
    [Header("밤 skybox 전환 타이밍 | default : 0.04f")]
    [Tooltip("noonToNightboxProcessTime 중 정해진 시간에 스카이박스를 교체함. noonToNightboxProcessTime 보다는 낮아야함")]
    [SerializeField, Range(0, 3f)] private float noonToNightboxChaneTime = 0f;

    [Space(20)]
    [Header("디버깅용================")]
    [Header("localrotation | 시작각도")]
    [SerializeField] float startRotationY = 0f;
    [Header("rotateTimer | 로테이션 타이머")]
    [SerializeField] float rotateTimer = 0f;
    [Header("현재 작동중 확인")]
    [SerializeField] private bool isRotate = false;
    //[Header("현재 작동중 확인")]
    //[SerializeField] float curRotationX = 0f;

    [Header("현재 진행도 | 오전 : 0 / 오후 : 1 / 밤 : 2")]
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
