using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTimers : MonoBehaviour
{
    public Camera playerCamera;
    public List<GameObject> targetObjects; // List of target objects to track
    public float lookTimeThreshold = 1.0f; // Adjust as needed
    public float lookAwayThreshold = 3.0f; // Adjust as needed

    private float currentLookTime = 0.0f;
    private float currentLookAwayTime = 0.0f;

    void Update()
    {
        // Cast a ray from the center of the screen forward
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // If the ray hits any of the target objects
            if (targetObjects.Contains(hitObject))
            {
                currentLookTime += Time.deltaTime;

                if (currentLookTime >= lookTimeThreshold)
                {
                    // If the player has been looking for the required time at any of the target objects
                    Debug.Log("Player has been looking at one of the target objects for " + currentLookTime + " seconds.");
                    // Do something here, like increase a counter or trigger an event
                }
            }
            else
            {
                // If the ray hits something other than the target objects, reset the look time
                currentLookTime = 0.0f;
            }
        }
        else
        {
            // If the ray doesn't hit anything, reset the look time
            currentLookTime = 0.0f;
            currentLookAwayTime += Time.deltaTime;

            if (currentLookAwayTime >= lookAwayThreshold)
            {
                // If the player has looked away from all target objects for too long
                Debug.Log("Player has looked away from all target objects for " + currentLookAwayTime + " seconds.");
                // Do something here, like call a function or trigger an event
            }
        }
    }
}