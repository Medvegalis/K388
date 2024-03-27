using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserPointer : MonoBehaviour
{
    public Transform laserOrigin;
    public float range = 50f;
    LineRenderer laserLine;

    public PdfReader slides;

    bool On = false;
    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        VrControls();
        //PcControls();

        if(On)
        {
            Vector3 laserDirection = laserOrigin.up;
            Vector3 laserEndPosition = laserOrigin.position + laserDirection * range;
            laserLine.SetPosition(0, laserOrigin.position);
            laserLine.SetPosition(1, laserEndPosition);
        }
    }

    public void VrControls()
    {
        if (InputManager.instance.currentRightHeldObjName == "InteractableLaserPointer")
        {
            if (InputManager.instance.playerControls.Vr.Trigger.WasPerformedThisFrame())
            {
                if(On == false)
                {
                    On = true;
                    Vector3 laserDirection = laserOrigin.up;
                    Vector3 laserEndPosition = laserOrigin.position + laserDirection * range;
                    laserLine.SetPosition(0, laserOrigin.position);
                    laserLine.SetPosition(1, laserEndPosition);
                    laserLine.enabled = true;
                }
                else
                {
                    laserLine.enabled = false;
                    On = false;
                }
            }

            if (InputManager.instance.playerControls.Vr.Pirmary.WasPerformedThisFrame())
            {
                slides.ShowNextPage();
            }

            if (InputManager.instance.playerControls.Vr.Secondary.WasPerformedThisFrame())
            {
                slides.ShowPrevPage();
            }
        }
    }

    public void PcControls()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {

            Vector3 laserDirection = laserOrigin.up;
            Vector3 laserEndPosition = laserOrigin.position + laserDirection * range;
            laserLine.SetPosition(0, laserOrigin.position);
            laserLine.SetPosition(1, laserEndPosition);
            laserLine.enabled = true;
        }
        else
            laserLine.enabled = false;
    }
}
