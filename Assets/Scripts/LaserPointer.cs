using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserPointer : MonoBehaviour
{
    public Transform laserOrigin;
    public float range = 50f;
    LineRenderer laserLine;
    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }
    private void Update()
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
