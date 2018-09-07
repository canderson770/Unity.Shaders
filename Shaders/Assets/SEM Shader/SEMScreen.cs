using UnityEngine;
using System.Collections;


public class SEMScreen : MonoBehaviour
{
    private Material SEMCameraMaterial;
    private new Renderer renderer;
    private float scanSpeed;
    private const float fraction = 0.05f;


    [ReadOnly] public bool isRunning = false;


    [Space(10)]
    [Tooltip("Max y position for scan line to start at")]
    public Transform max;

    [Tooltip("Min y position for scan line to stop at")]
    public Transform min;


    [Space(10)]
    [Tooltip("Max speed scan line moves")]
    [Range(0, .01f)] public float speed = 0.01f;


    [Space(10)]
    [Tooltip("Clarity of screen on Start")]
    public float startingClarity = 1;

    [Tooltip("Lowest clarity screen will go")]
    public float minClarity = 1;

    [Tooltip("Highest clarity screen will go")]
    public float maxClarity = 10;


    private void Start()
    {
        //if null throw errors
        if (max == null)
            Debug.LogError("Max Transform not set on " + gameObject.name + "!");

        if (min == null)
            Debug.LogError("Min Transform not set on " + gameObject.name + "!");

        //get material
        renderer = GetComponent<Renderer>();
        SEMCameraMaterial = renderer.material;

        //set starting clarity and scan pos to max
        SEMCameraMaterial.SetFloat("_ScanPos", max.position.y);
        SEMCameraMaterial.SetFloat("_BottomClarity", startingClarity);
        SEMCameraMaterial.SetFloat("_TopClarity", startingClarity);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadPlus) || Input.GetKeyUp(KeyCode.Plus))
        {
            ImproveImage();
        }
        else if (Input.GetKeyUp(KeyCode.KeypadMinus) || Input.GetKeyUp(KeyCode.Minus))
        {
            ReduceImage();
        }
    }


    private void ImproveImage()
    {
        //disable while running
        if (isRunning)
            return;

        //get current clarity
        float currentClarity = SEMCameraMaterial.GetFloat("_BottomClarity");

        //check if below min clarity
        if (currentClarity < maxClarity)
        {
            //increase clarity
            currentClarity += (fraction * currentClarity * currentClarity) + 1;

            //make sure it doesn't pass max clarity
            if (currentClarity > maxClarity)
                currentClarity = maxClarity;

            //the higher the clarity, the slower the speed
            scanSpeed = speed - ((speed / 15) * currentClarity);

            //set top clarity
            SEMCameraMaterial.SetFloat("_TopClarity", currentClarity);

            //start scan
            StartCoroutine(Scan());
        }

    }

    private void ReduceImage()
    {
        //disable while running
        if (isRunning)
            return;

        //get current clarity
        float currentClarity = SEMCameraMaterial.GetFloat("_BottomClarity");

        //check if above min clarity
        if (currentClarity > minClarity)
        {
            //reduce clarity
            currentClarity -= (fraction * currentClarity * currentClarity) + 1;

            //make sure it doesn't pass min clarity
            if (currentClarity < minClarity)
                currentClarity = minClarity;

            //the higher the clarity, the slower the speed
            scanSpeed = speed - ((speed / 15) * currentClarity);

            //set top clarity
            SEMCameraMaterial.SetFloat("_TopClarity", currentClarity);

            //start scan
            StartCoroutine(Scan());
        }
    }

    private IEnumerator Scan()
    {
        isRunning = true;

        //stop movement
        float oldMoveBool = SEMCameraMaterial.GetFloat("_Speed");
        SEMCameraMaterial.SetFloat("_Speed", 0);

        //get scan pos
        float scanPos = SEMCameraMaterial.GetFloat("_ScanPos");

        //while still above min
        while (scanPos > min.position.y)
        {
            //lower scan pos
            scanPos -= scanSpeed;

            //set scan pos
            SEMCameraMaterial.SetFloat("_ScanPos", scanPos);
            scanPos = SEMCameraMaterial.GetFloat("_ScanPos");

            //wait
            yield return new WaitForEndOfFrame();
        }

        //set bottom clarity to match top clarity
        float newQuality = SEMCameraMaterial.GetFloat("_TopClarity");
        SEMCameraMaterial.SetFloat("_BottomClarity", newQuality);

        //reset scan post back up to top
        SEMCameraMaterial.SetFloat("_ScanPos", max.position.y);

        //start movement
        SEMCameraMaterial.SetFloat("_Speed", oldMoveBool);

        isRunning = false;
    }
}
