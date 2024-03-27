using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    public ControlScheme playerControls;

    public XRDirectInteractor leftHand;
    public XRDirectInteractor rightHand;

    public string currentLeftHeldObjName {get; private set;}
    public string currentRightHeldObjName { get; private set; }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        playerControls = new ControlScheme();
        currentLeftHeldObjName = "";
        currentRightHeldObjName = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (rightHand.GetOldestInteractableSelected() != null)
        {
            currentRightHeldObjName = rightHand.GetOldestInteractableSelected().transform.name;

        }

        if (!rightHand.hasSelection)
        {
            currentRightHeldObjName = "";
        }

        if (leftHand.GetOldestInteractableSelected() != null)
        {
            currentLeftHeldObjName = leftHand.GetOldestInteractableSelected().transform.name;

        }

        if (!leftHand.hasSelection)
        {
            currentLeftHeldObjName = "";
        }
    }
}
