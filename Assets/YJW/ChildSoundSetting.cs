using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChildSoundSetting : MonoBehaviour
{
    [SerializeField] private AudioClip btnSound;
    [SerializeField] private Toggle toggle;
    [SerializeField] private GameObject effectPrefab;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate { PlayPushSound(toggle); });
    }


    public void PlayPushSound(Toggle btnPush)
    {
        Instantiate(effectPrefab,transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(btnSound, transform.position);
    }


    //[SerializeField] private Transform[] childToggleObjPosition;
    //[SerializeField] private Toggle[] curChildToggleObjState;
    //[SerializeField] private Toggle[] preChildToggleObjState;

    //private void Awake()
    //{

    //    childToggleObjPosition = GetComponentsInChildren<Transform>();
    //    curChildToggleObjState = GetComponentsInChildren<Toggle>();
    //    preChildToggleObjState = curChildToggleObjState;
    //}

    //private void Update()
    //{
    //    if (!pushButtonAction.isOn)
    //        return;

    //    for (int i = 0; i < curChildToggleObjState.Length; i++)
    //    {
    //        if (curChildToggleObjState[i].isOn == true && preChildToggleObjState[i].isOn == true)
    //            continue;

    //        //if (curChildToggleObjState[i].isOn != preChildToggleObjState[i].isOn)
    //        //{
    //        
    //        preChildToggleObjState[i] = curChildToggleObjState[i];
    //        Debug.Log("curChildToggleObjState : " + curChildToggleObjState);
    //        Debug.Log("preChildToggleObjState : " + preChildToggleObjState);
    //        Debug.Log("childToggleObjPosition : " + childToggleObjPosition[i + 3].position);
    //        Debug.Log("사운드 재생");
    //        //}
    //    }
    //}
}
