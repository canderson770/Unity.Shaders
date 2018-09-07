using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableMinimap : MonoBehaviour 
{
	public GameObject target;
	float targetScale;
	float startDist;
	Quaternion rotation;
	public Transform other;

	public float minScale = 0.1f;
	public float maxScale = 7;

	void OnEnable()
	{
		targetScale = target.transform.localScale.x;
		startDist = Vector3.Distance (other.position, transform.position);
	}

	void Update() 
	{
		if (other && target) 
		{
			float dist = Vector3.Distance(other.position, transform.position) - startDist + 1;

			if(target && dist > minScale && dist < maxScale)
				target.transform.localScale = Vector3.one * dist * targetScale;
			else if(dist < minScale)
				target.transform.localScale = Vector3.one * minScale * targetScale;
			else if(dist > maxScale)
				target.transform.localScale = Vector3.one * maxScale * targetScale;
		}
	}
}
