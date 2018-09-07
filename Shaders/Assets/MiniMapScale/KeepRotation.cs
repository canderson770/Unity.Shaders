using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotation : MonoBehaviour 
{
	Quaternion rotation;

	void OnEnable()
	{
		rotation = transform.rotation;
	}

	void Update() 
	{
		transform.rotation = rotation;
	}
}
