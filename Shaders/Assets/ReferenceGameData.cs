using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceGameData : MonoBehaviour 
{

	void Start () 
	{
		print (GameData.Instance.speed);
		GameData.Instance.speed = 20;
		print (GameData.Instance.speed);
	}

	void OnApplicationQuit()
	{
		GameData.SetData ();
		print ((PlayerPrefs.GetString ("GameData")));
	}
}
