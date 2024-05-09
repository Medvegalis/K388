using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookTimers : MonoBehaviour
{
	public Camera playerCamera;
	public List<GameObject> targetObjects;
	public float lookTimeThreshold = 1.0f;
	public float lookTimeMax = 10.0f;
	public float lookAwayThreshold = 3.0f;
	public bool LonglookAway = false;
	public float distanceSum = 0.0f;
	public float point_mod = 1.0f;

	private float currentLookTime = 0.0f;
	private float currentLookAwayTime = 0.0f;
	private Vector3 vect;
	private bool vect_exists = false;

	[SerializeField] private Text TimeLookedAtTextUI;

	void Update()
	{
		Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			GameObject hitObject = hit.collider.gameObject;

			if (targetObjects.Contains(hitObject))
			{
				currentLookAwayTime = 0f;
				currentLookTime += Time.deltaTime;
				TimeLookedAtTextUI.text = currentLookTime.ToString();

				if (currentLookTime >= lookTimeThreshold && currentLookTime < lookTimeMax)
				{
					LonglookAway = false;
					Debug.Log("Player has been looking at one of the target objects");
					point_mod = 1+((float) currentLookTime /lookTimeMax);
				}
				else if (currentLookTime >= lookTimeMax)
				{
					LonglookAway = false;
					Debug.Log("Player has been looking too long at one of the target objects");
					point_mod = 1;
				}
			}
			else
			{
				currentLookTime = 0.0f;
				currentLookAwayTime += Time.deltaTime;

				if (currentLookAwayTime >= lookAwayThreshold)
				{
					LonglookAway = true;
					Debug.Log("Player has looked away from all target objects");
				}
			}

			if (vect_exists)
			{
				float distance = Vector3.Distance(vect, ray.origin);
				distanceSum += point_mod * distance;
			}

			vect = ray.origin;
			vect_exists = true;
		}
	}
	public float DistanceSum()
	{
		return distanceSum;
	}
}