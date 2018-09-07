using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour 
{
	public UnityEvent OnClick;

	void Awake()
	{
		gameObject.SetActive (false);
	}

	void OnMouseDown()
	{
		OnClick.Invoke ();
	}
}
