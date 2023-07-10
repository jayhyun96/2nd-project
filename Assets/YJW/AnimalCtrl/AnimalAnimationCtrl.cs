using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimationCtrl : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [Space(10)]
    [SerializeField] private string _preAnimState = null;
    public string preAnimState { get => _preAnimState; set => preAnimState = value; }
    [SerializeField] private string _curAnimState = null;
    public string curAnimState { get => _curAnimState; set => _curAnimState = value; }
    [Space(10)]
    [SerializeField] private string _preShapeState = null;
    public string preShapeState { get => _preShapeState; set => _preShapeState = value; }

    [SerializeField] private string _curShapeState = null;
    public string curShapeState { get => _curShapeState; set => _curShapeState = value; }
    [Space(10)]
    [SerializeField] private bool isHit = false; 

    [Space(20)]

    [SerializeField]
    private List<string> animationList = new List<string>
                                            {   "Attack",
                                                "Bounce",
                                                "Clicked",
                                                "Death",
                                                "Eat",
                                                "Fear",
                                                "Fly",
                                                "Hit",
                                                "Idle_A", "Idle_B", "Idle_C",
                                                "Jump",
                                                "Roll",
                                                "Run",
                                                "Sit",
                                                "Spin/Splash",
                                                "Swim",
                                                "Walk"
                                            };

    [SerializeField]
    private List<string> shapekeyList = new List<string>
                                            {   "Eyes_Annoyed",
                                                "Eyes_Blink",
                                                "Eyes_Cry",
                                                "Eyes_Dead",
                                                "Eyes_Excited",
                                                "Eyes_Happy",
                                                "Eyes_LookDown",
                                                "Eyes_LookIn",
                                                "Eyes_LookOut",
                                                "Eyes_LookUp",
                                                "Eyes_Rabid",
                                                "Eyes_Sad",
                                                "Eyes_Shrink",
                                                "Eyes_Sleep",
                                                "Eyes_Spin",
                                                "Eyes_Squint",
                                                "Eyes_Trauma",
                                                "Sweat_L",
                                                "Sweat_R",
                                                "Teardrop_L",
                                                "Teardrop_R"
                                            };

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }
    private void Update()
    {
        if (_preAnimState == _curAnimState && _preShapeState == _curShapeState)
            return;

        ChangeAnimation();
    }

    public void ChangeAnimation()
    {


        if (_preAnimState != _curAnimState)
        {
            if ((isHit = CheckAnimationListInclude(_curAnimState)) == false)
            {
                _curAnimState = _preAnimState;
                return;
            }

            _preAnimState = _curAnimState;
            anim.Play(_curAnimState);
            isHit = false;
        }

        if (_preShapeState != _curShapeState)
        {
            if ((isHit = CheckShapekeyListInclude(_curShapeState)) == false)
            {
                _curShapeState = _preShapeState;
                return;
            }
            _preShapeState = _curShapeState;
            anim.Play(_curShapeState);
            isHit = false;
        }
    }
    public bool CheckAnimationListInclude(string animationName)
    {
        bool isInclude = false;
        for (int i = 0; i < animationList.Count; ++i)
        {
            if (animationList[i] == animationName)
            {
                isInclude = true;
                break;
            }
        }

        return isInclude;
    }
    public bool CheckShapekeyListInclude(string shapekeyName)
    {
        bool isInclude = false;
        for (int i = 0; i < shapekeyList.Count; ++i)
        {
            if (shapekeyList[i] == shapekeyName)
            {
                isInclude = true;
                break;
            }
        }

        return isInclude;
    }
}
