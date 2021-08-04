using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneDisabler : MonoBehaviour
{
    private ARPlaneManager planeManager;


    public GameObject[] officePrefabs;
    private List<GameObject> spawnedObjs = new List<GameObject>();

    private ARPlane desk_palne;

    // Start is called before the first frame update
    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
       
    }

    

    // Update is called once per frame
    void Update()
    {
        
       /* foreach (var plane in planeManager.trackables)
        {
            
            float planeArea = plane.size.x * plane.size.y; //x = 1.3, y = 0.6
            
            if (planeArea > 2 && plane.size.x >= 1.8 && plane.size.y >= 0.8)
            {
               
                planeManager.enabled = false;
                desk_palne = planeManager.GetPlane(plane.trackableId) ;
               
                break;

            }
            */

    
        //}
        //else print message to player that he hasn't scanned the desk 
       /* if (planeManager.enabled == false)
        {

            DeactivatePlanes(ARPlaneDetector.Instance.desk_plane_ID);
            ObjectsSpawner.Instance.SpawnPrefabs();

        }*/

    }
    



    void DeactivatePlanes( TrackableId planeId)
    {
        
        foreach (var plane in planeManager.trackables)
        {
            if(plane.trackableId != planeId)
            plane.gameObject.SetActive(false);
        }
    }

}


