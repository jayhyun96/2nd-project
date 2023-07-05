using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightMngr : MonoBehaviour
{
    [SerializeField] private Material dayBox = null;
    [SerializeField] private Material nightBox = null;

    [Header("Directional Light 월드 포지션 : default (0, 80, 0)")]

    [SerializeField] private Vector3 lightPosition = Vector3.zero;

    [Header("Directional Light 월드 로테이션 : default (50, -30, 0)")]
    [SerializeField] private Vector3 lightRotation = Vector3.zero;

    [Header("Directional Light 로테이션 각도 : default (1)")]
    [SerializeField, Range(0, 1f)] private float degreePerTime = 1f;

    [Header("Directional Light 타겟 로테이션 각도 : default (90)")]
    [SerializeField, Range(0, 90f)] private float targetDegree = 90f;

    [Header("Directional Light 로테이션 딜레이 :  default (0.05f)")]
    [SerializeField, Range(0, 0.1f)] private float rotateDealy = 0.05f;

    [Header("localrotation 시작각도")]
    [SerializeField] float startRotationY = 0f;

    [SerializeField] float rotateTimer = 0f;

    [SerializeField] private bool isRotate = false;

    [SerializeField] float curRotationX = 0f;
    [SerializeField] float rotationSwitch = 0f;
    //[SerializeField] float compStartRotationY = 0f;
    //[SerializeField] float rotationResult = 0f;

    private void Awake()
    {
        this.transform.position = lightPosition;
        this.transform.rotation = Quaternion.Euler(lightRotation);
    }
    private void Update()
    {
        curRotationX = transform.eulerAngles.x;
    }
    public void StartCo()
    {
        Coroutine co = StartCoroutine(RotateLight());
    }


    IEnumerator RotateLight()
    {
        if (!isRotate)
        {

            switch (rotationSwitch)
            {
                case 0:
                    while (transform.eulerAngles.x > 5f)
                    {
                        // rotateTimer += Time.deltaTime;
                        yield return new WaitForSeconds(rotateDealy);
                        this.transform.Rotate(Vector3.left * degreePerTime);
                        float skyboxRotate = RenderSettings.skybox.GetFloat("_Rotation");
                        skyboxRotate += 1f;
                        RenderSettings.skybox.SetFloat("_Rotation", skyboxRotate);

                        float skyboxExposure = RenderSettings.skybox.GetFloat("_Exposure");
                        if (skyboxExposure > 0.5f)
                        {
                            yield return new WaitForSeconds(rotateDealy);
                            skyboxExposure -= 0.05f;
                            RenderSettings.skybox.SetFloat("_Exposure", skyboxExposure);
                        }

                    }
                    Debug.Log("rotate 0");
                    ++rotationSwitch;
                    break;

                case 1:
                    while (rotateTimer < 0.74f)
                    {



                        rotateTimer += Time.deltaTime;
                        yield return new WaitForSeconds(rotateDealy);
                        this.transform.Rotate(Vector3.left * degreePerTime);
                        float skyboxRotate = RenderSettings.skybox.GetFloat("_Rotation");
                        skyboxRotate += 1f;
                        RenderSettings.skybox.SetFloat("_Rotation", skyboxRotate);

                        float skyboxExposure = RenderSettings.skybox.GetFloat("_Exposure");
                        if (skyboxExposure >= 0f)
                        {
                            yield return new WaitForSeconds(rotateDealy);
                            skyboxExposure -= 0.05f;
                            RenderSettings.skybox.SetFloat("_Exposure", skyboxExposure);
                        }

                        if (rotateTimer > 0.4f && skyboxExposure < 1f)
                        {
                            RenderSettings.skybox = nightBox;
                            RenderSettings.skybox.SetFloat("_Exposure", 0f);
                            skyboxExposure += 0.05f;
                        }

                    }
                    Debug.Log("rotate 1");
                    ++rotationSwitch;
                    break;
                case 2:
                    while (rotateTimer < 0.9f)
                    {


                        rotateTimer += Time.deltaTime;
                        yield return new WaitForSeconds(rotateDealy);
                        this.transform.Rotate(Vector3.left * degreePerTime);
                        float skyboxRotate = RenderSettings.skybox.GetFloat("_Rotation");
                        skyboxRotate += 1f;
                        RenderSettings.skybox.SetFloat("_Rotation", skyboxRotate);

                        if (rotateTimer > 0.7f)
                        {
                            RenderSettings.skybox = dayBox;
                            RenderSettings.skybox.SetFloat("_Exposure", 1f);

                        }

                    }
                    Debug.Log("rotate 2");
                    rotationSwitch = 0;
                    break;

            }



            isRotate = false;
            rotateTimer = 0f;
            yield return null;
        }
    }



    private IEnumerator TurnDayLight()
    {
        yield return null;
    }

    private IEnumerator TurnNoonLight()
    {
        yield return null;
    }

    private IEnumerator TurnNightLight()
    {
        yield return null;
    }

}
