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
        // ��ư �ε����� �ش��ϴ� UI Image�� UI Text�� ������
        Image targetImage = imageArray[buttonIndex];
        TMP_Text targetText = textArray[buttonIndex];

        // �ش� UI Image�� Ȱ��ȭ�ϰ� �������� ��Ȱ��ȭ
        foreach (Image image in imageArray)
        {
            image.gameObject.SetActive(image == targetImage);
        }

        // �ش� UI Text�� Ȱ��ȭ�ϰ� �������� ��Ȱ��ȭ
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
