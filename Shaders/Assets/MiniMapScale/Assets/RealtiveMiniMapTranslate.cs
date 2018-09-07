using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This script is used for the full funtion of the minimap
///
///To acomplish this the following conditions must be met
/// -Any minimap assosiated objects must be a child of "minimapGroup"
/// -The minimap object's mesh renderer must be disabled along with any other child objects of minimapGroup
/// -A collider must exist on the minimap and must be turned off and set to trigger
///  (This collider determins the area that a player can exist before the minimap resets)
/// -Lastly this script is recomended to exist on the highest object node that will be parented to the dino
/// 
/// ------------
/// The function of this script is that when a player completes an action
/// the minimap will apear in front of the player with small representations 
/// of the dinosuars. 
/// 
/// When the player leaves the defined feild the minimap will return to the player in an off state
/// waiting for the action to repeat.
/// ------------
///Created by: Colin Hite
///Last edited: 10/22/2017
///</summary>

public class RealtiveMiniMapTranslate : MonoBehaviour {

    //These objects are the corresponding player or enemy
    //that will be mimicked on the minimap
    public GameObject playerOneMini;
    public GameObject playerTwoMini;
    public GameObject playerThreeMini;
    public GameObject playerFourMini;

    //These objects are the "fullscale" Dinos that will be represented on the minimap.
    //Also, the "PlayerOnePos" object is used to determine the parenting of the minimap object
    public GameObject playerOnePos;
    public GameObject playerTwoPos;
    public GameObject playerThreePos;
    public GameObject playerFourPos;

    //This object is the minimap assosiated with the player dino
    public GameObject miniMap;

    //These floats represent the place at which the minimap resets itself after leaving the trigger
    public float xPosReset;
    public float yPosReset;
    public float zPosReset;

	void FixedUpdate ()
    {
        //This coroutine translates the mini dinos on the minimap
        StartCoroutine("FindRealativePos");

        //This coroutine turns on the meshrenders of the minimap and its parts
        if (Input.GetKeyDown("e"))
        {
            StartCoroutine ("CreateMiniMap");
        }
    }//End Fixed Update

    //This moves the mini dinos on the minimap relative to the fullscale versions
    IEnumerator FindRealativePos()
    {
        //This line translates the dino representative on the minimap
        playerOneMini.transform.localPosition = new Vector3(playerOnePos.GetComponent<Transform>().position.x, playerOnePos.GetComponent<Transform>().position.y, playerOnePos.GetComponent<Transform>().position.z);
        playerTwoMini.transform.localPosition = new Vector3(playerTwoPos.GetComponent<Transform>().position.x, playerTwoPos.GetComponent<Transform>().position.y, playerTwoPos.GetComponent<Transform>().position.z);
        playerThreeMini.transform.localPosition = new Vector3(playerThreePos.GetComponent<Transform>().position.x, playerThreePos.GetComponent<Transform>().position.y, playerThreePos.GetComponent<Transform>().position.z);
        playerFourMini.transform.localPosition = new Vector3(playerFourPos.GetComponent<Transform>().position.x, playerFourPos.GetComponent<Transform>().position.y, playerFourPos.GetComponent<Transform>().position.z);

        return null;
    }//End FindRealativePos

    //This turns the collider of the minimap on,
    //turns on the minimap's meshrenderer on,
    //and deparents the minimapGroup from the player
    IEnumerator CreateMiniMap()
    {
        //These lines enable the meshrenderer of the minimap objects
        foreach (MeshRenderer rend in miniMap.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = true;
        }
        //These lines turns the trigger collider of the minimap on so that a distance from the minimap can be derived
        foreach (Collider col in miniMap.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }

        //This unparents the map from the player
        miniMap.transform.parent = null;

        yield return new WaitForSeconds(0.1f);
    }//End CreateMiniMap

    void OnTriggerExit()
    {
        //This reparents the minimap to the dino
        miniMap.transform.parent = playerOnePos.transform;

        //These lines turn the visuals of the map and its collider off
        foreach (MeshRenderer rend in miniMap.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = false;
        }
        foreach (Collider col in miniMap.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        //This line replaces the map relative to the player
        miniMap.transform.localPosition = new Vector3(xPosReset, yPosReset, zPosReset);
        miniMap.transform.localRotation = new Quaternion(0, 0, 0, 0);
        StopCoroutine("CreateMiniMap");
    }//End OnTriggerExit
}//End RealtiveMiniMapTranslate
