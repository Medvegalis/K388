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
	public float yeet_points = 0.1f;

	private float currentLookTime = 0.0f;
	private float currentLookAwayTime = 0.0f;
	private Vector3 lastHitPoint;
	private bool lastHitPointExists = false;

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
				TimeLookedAtTextUI.text = currentLookTime.ToString("F2");

				if (currentLookTime >= lookTimeThreshold && currentLookTime < lookTimeMax)
				{
					LonglookAway = false;
					point_mod = 1 + (currentLookTime / lookTimeMax);
				}
				else if (currentLookTime >= lookTimeMax)
				{
					LonglookAway = false;
					point_mod = Mathf.Max(0, point_mod - yeet_points * Time.deltaTime);
				}
			}
			else
			{
				currentLookTime = 0.0f;
				currentLookAwayTime += Time.deltaTime;

				if (currentLookAwayTime >= lookAwayThreshold)
				{
					LonglookAway = true;
				}
			}

			if (lastHitPointExists)
			{
				float distance = Vector3.Distance(lastHitPoint, hit.point);
				distanceSum += point_mod * distance;
			}

			lastHitPoint = hit.point;
			lastHitPointExists = true;
		}
		else
		{
			currentLookTime = 0.0f;
			currentLookAwayTime += Time.deltaTime;

			if (currentLookAwayTime >= lookAwayThreshold)
			{
				LonglookAway = true;
			}

			lastHitPointExists = false;
		}

		//Debug.Log($"Current Look Time: {currentLookTime}, Current Look Away Time: {currentLookAwayTime}, Distance Sum: {distanceSum}, Point Mod: {point_mod}");
	}

	public float DistanceSum
	{
		get { return distanceSum; }
		set { distanceSum=value; }
	}
}
