using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool allowMove = false;
    public bool allowShiftPerspective = false;
    public bool allowInteraction = false;

    public GameObject menuPanel;
    public CameraState cameraState;
    public TargetAnimation targetAnimation;
    public PlatformConsoleMananger platformConsoleMananger;
    // Start is called before the first frame update
    void Start()
    {
        allowMove = true;
        allowShiftPerspective = true;
        allowInteraction = true;
    }

    void LateUpdate()
    {
        if (menuPanel.activeSelf || cameraState.GetIsRebinding() || (targetAnimation && !targetAnimation.finished) || (platformConsoleMananger && platformConsoleMananger.GetPlatformIsRotating()))
        {
            allowMove = false;
            allowShiftPerspective = false;
            allowInteraction = false;
        }
        else
        {
            if (cameraState.GetIsRotating())
            {
                allowMove = false;
                allowShiftPerspective = true;
                allowInteraction = false;
            }
            else
            {
                allowMove = true;
                allowShiftPerspective = true;
                allowInteraction = true;
            }
        }
    }

    public bool GetAllowMove()
    {
        return allowMove;
    }

    public bool GetAllowShiftPerspective()
    {
        return allowShiftPerspective;
    }

    public bool GetAllowInteraction()
    {
        return allowInteraction;
    }
}
