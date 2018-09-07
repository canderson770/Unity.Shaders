using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerTriggerCheck : MonoBehaviour 
{
	ScalableMinimap minimap;
	public UnityEvent onHoverBegin;
	public UnityEvent onHoverEnd;

	void UnSubFromControls()
	{
	}

	void SubToControls()
	{
	}

	void SubToNewControls()
	{
	}

	void UnSubFromNewControls()
	{
	}

	void Start()
	{
		minimap = GetComponent<ScalableMinimap> ();
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.name.Contains("ScalableMiniMap"))
		{
			UnSubFromControls ();
			SubToNewControls ();
			onHoverBegin.Invoke ();

			if (coll.transform.parent != minimap.other.transform) 
			{
				coll.transform.parent = transform;
			}
			else 
			{
				minimap.enabled = true;
			}
		}
	}

	void OnTriggerExit(Collider coll)
	{
		if (coll.gameObject.name.Contains("ScalableMiniMap"))
		{
//			UnSubFromNewControls ();
//			SubToControls ();
//			onHoverEnd.Invoke ();

//			if (coll.transform.parent == transform) 
//			{
//				coll.transform.parent = null;
//			}
//			else 
//			{
//				minimap.enabled = false;
//				minimap.other.GetComponent<ScalableMinimap> ().enabled = false;
//			}
		}
	}
}
