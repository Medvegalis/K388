using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    Animator anim;
    [SerializeField] private InputActionProperty pinch;
    [SerializeField] private InputActionProperty grip;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim == null)
            return;

        float gripValue = grip.action.ReadValue<float>();
        float pinchValue = pinch.action.ReadValue<float>();

        anim.SetFloat("Trigger", pinchValue);
        anim.SetFloat("Grip", gripValue);
    }
}
