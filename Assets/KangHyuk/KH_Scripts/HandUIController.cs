using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandUIController : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject LocalUI;
    public GameObject MonorailUI;
    public GameObject StampUI;

    [SerializeField] GameObject[] Stamps;

    public Image[] imageArray;
    public TMP_Text[] textArray;

    public void CollectStamp()
    {
        for (int i = 0; i < (int)STAMP.SIZE; i++)
        {
            if (GameManager.Instance.StampContentsFinish[i])
            {
                Stamps[i].SetActive(true);
            }
        }
    }

    public void LocalInformation()
    {
        MainUI.SetActive(false);
        LocalUI.SetActive(true);
    }

    public void MonorailControl()
    {
        MainUI.SetActive(false);
        MonorailUI.SetActive(true);
    }

    public void StampControl()
    {
        MainUI.SetActive(false);
        StampUI.SetActive(true);
    }

    public void ChangeInformation(int buttonIndex)
    {
        // 버튼 인덱스에 해당하는 UI Image와 UI Text를 가져옴
        Image targetImage = imageArray[buttonIndex];
        TMP_Text targetText = textArray[buttonIndex];

        // 해당 UI Image를 활성화하고 나머지는 비활성화
        foreach (Image image in imageArray)
        {
            image.gameObject.SetActive(image == targetImage);
        }

        // 해당 UI Text를 활성화하고 나머지는 비활성화
        foreach (TMP_Text text in textArray)
        {
            text.gameObject.SetActive(text == targetText);
        }
    }

    public void ReturnButton()
    {
        MainUI.SetActive(true);
        LocalUI.SetActive(false);
        MonorailUI.SetActive(false);
        StampUI.SetActive(false);
    }
}
