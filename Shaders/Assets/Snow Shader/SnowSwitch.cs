using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowSwitch : MonoBehaviour
{
    private Material material;
    public bool isOn = true;

    private void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        material = mr.material;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isOn = !isOn;
            if (isOn)
                material.SetFloat("_Tess", 64);
            else
                material.SetFloat("_Tess", 1);
        }
    }
}
