using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraState : MonoBehaviour
{
    public FacingDirection facingDirection = FacingDirection.Front;
    public bool isRotating = false;
    public bool isUsing3DView = false;

    public void SetFacingDirection(FacingDirection facingDirection)
    {
        this.facingDirection = facingDirection;
    }

    public FacingDirection GetFacingDirection()
    {
        return facingDirection;
    }

    public void SetIsRotating(bool isRotating)
    {
        this.isRotating = isRotating;
    }

    public bool GetIsRotating()
    {
        return isRotating;
    }

    public void SetIsUsing3DView(bool isUsing3DView)
    {
        this.isUsing3DView = isUsing3DView;
    }

    public bool GetIsUsing3DView()
    {
        return isUsing3DView;
    }
}
