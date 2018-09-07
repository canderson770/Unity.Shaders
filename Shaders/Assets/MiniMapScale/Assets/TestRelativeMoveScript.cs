using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRelativeMoveScript : MonoBehaviour {

//    private float factorTwo;

    public GameObject mini;
    public GameObject posPass;

	// Use this for initialization
	void Start ()
    {
        //This should be 0.1 in our case
//		factorTwo = mini.GetComponent<Transform>().lossyScale.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        mini.transform.localPosition = new Vector3(0, 0, posPass.GetComponent<Transform>().position.z);
    }
}
