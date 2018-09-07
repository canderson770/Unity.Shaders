using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameData //: MonoBehaviour 
{
	GameData()
	{
		
	}

	public float speed = 10;
	public float health = 1;
	public Vector3 checkPoint = Vector3.one;
	
	const string dataName = "GameData";

	private static GameData instance;

	public static GameData Instance
	{
		get
		{
			if (instance == null) 
			{
				GetData ();
			}

			return instance; 
		}
	}

	public static void GetData()
	{
		if (string.IsNullOrEmpty (PlayerPrefs.GetString (dataName))) 
		{
			instance = new GameData ();
		} 
		else 
		{
			instance = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(dataName));	
		}
	}
		
	public static void SetData()
	{
		PlayerPrefs.SetString (dataName, JsonUtility.ToJson(instance));
	}
}
