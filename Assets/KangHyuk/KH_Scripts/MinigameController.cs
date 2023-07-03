using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameController : MonoBehaviour
{
    public Canvas canvas;
    public Button startButton;
    public Button completeButton;
    public TextMeshProUGUI timerText;

    private bool timing;
    private float elapsedTime;
    private bool allTag;

    private void Start()
    {
        timing = false;
        elapsedTime = 0f;
        timerText.text = "00:00"; // 초기 시간을 00:00으로 설정

        startButton.onClick.AddListener(StartTimer);
        completeButton.onClick.AddListener(CompleteGame);
    }

    private void Update()
    {
        if (timing)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    private void StartTimer()
    {
        allTag = false;

        Toggle[] toggles = canvas.GetComponentsInChildren<Toggle>();

        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
        }

        timing = true;
    }

    private void CompleteGame()
    {
        CheckToggleGroup();
        if (allTag)
        {
            timing = false;
            RecordTime();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void RecordTime()
    {
        Debug.Log("Time taken: " + timerText.text);
    }

    private void CheckToggleGroup()
    {
        Toggle[] toggles = canvas.GetComponentsInChildren<Toggle>();

        bool allTogglesChecked = true;

        foreach (Toggle toggle in toggles)
        {
            if (!toggle.isOn)
            {
                allTogglesChecked = false;
                break;
            }
        }

        if (allTogglesChecked)
        {
            allTag = true;
            Debug.Log("All toggles are checked.");
        }
        else
        {
            //Debug.Log("Not all toggles are checked.");
        }
    }
}