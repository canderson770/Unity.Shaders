using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTimer : MonoBehaviour
{
	public Material material;

	public float minY = 0;
	public float maxY = 2;
	public float duration = 5;

	// Update is called once per frame
	void Update () {
		float y = Mathf.Lerp(minY, maxY, Time.time / duration);
		material.SetFloat("_ConstructY", y);
	}
}