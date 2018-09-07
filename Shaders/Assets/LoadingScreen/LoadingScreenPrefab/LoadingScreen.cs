using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadingScreen : MonoBehaviour 
{
	AsyncOperation operation;

	public bool loadOnStart = false;
    public bool autoTransition = false;
	public Material dinoMat;
	public float dinoHeight = 2.1f;
	public float minLoadTime = 3;

	public UnityEvent OnLoadReady;

	void Start()
	{
		if(loadOnStart)
			LoadLevel (SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void LoadLevel(int sceneIndex)
	{
		StartCoroutine(Load(sceneIndex));
	}

	IEnumerator Load(int sceneIndex)
	{
		operation = SceneManager.LoadSceneAsync (sceneIndex, LoadSceneMode.Single);
		operation.allowSceneActivation = false;

		while (operation.progress < .9f || Time.timeSinceLevelLoad < minLoadTime)
		{
			float progress = Mathf.Clamp01 (Time.timeSinceLevelLoad / minLoadTime) * dinoHeight;

			if (dinoMat != null)
				dinoMat.SetFloat ("_ConstructY", progress);
			
			yield return null;
		}
        		
        if (autoTransition)
            operation.allowSceneActivation = true;
        else
            OnLoadReady.Invoke ();
	}

	public void FinishLoadScene()
	{
		operation.allowSceneActivation = true;
	}
}
