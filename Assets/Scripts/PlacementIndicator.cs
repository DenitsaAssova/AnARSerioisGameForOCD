using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class PlacementIndicator : MonoBehaviour
{

    private ARRaycastManager rayManager;
    public GameObject visual;
    public Button placeBin;
    public Button scanFloor;
    private bool activatedVisualOnce = false;
    private bool activatedDrawRoadOnce = false;
    public  Button startDraw;
    public GameObject objsToSpawn;

    public static Vector3 firstGroundTransform;


    private static PlacementIndicator instance;
    
    public static PlacementIndicator Instance { get { return instance; } }


    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }


    private void Start()
    {
        
        rayManager = FindObjectOfType<ARRaycastManager>();
        visual = transform.GetChild(0).gameObject;
        placeBin.gameObject.SetActive(false);
        visual.SetActive(false);
        
    }

    private void Update()
    {
        //shoot raycast from center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);
       // Vector3 projectedPos = new Vector3();
      
        if (hits.Count > 0)
        {
           
           
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;

           

            if (activatedVisualOnce == false)
            {
               // projectedPos =  hits[0].pose.up;
                firstGroundTransform = transform.position;
                activatedVisualOnce = true;

            }

            if(transform.position.y< firstGroundTransform.y)
            {
                firstGroundTransform = transform.position;
               // projectedPos = hits[0].pose.up;
            }

           if(DrawRoad.canMoveObj == false)
            {
                transform.position = new Vector3(transform.position.x, firstGroundTransform.y, transform.position.z);
               
               // transform.position = Vector3.ProjectOnPlane(transform.position, projectedPos); 
            }


                if (!visual.activeInHierarchy && BinPlacer.Instance.itemsCollected < 7)
                {
               
                visual.SetActive(true);
               
            } else
            {
                objsToSpawn.gameObject.SetActive(true);
                scanFloor.GetComponentInChildren<TextMeshProUGUI>().text = "Place the items";
                if (BinPlacer.Instance.numObjsFromUIPlaced == 7)
                {
                    scanFloor.gameObject.SetActive(false);
                    //objsToSpawn.gameObject.SetActive(false);
                    if (BinPlacer.Instance.binPlaced == false)
                    {
                    placeBin.gameObject.SetActive(true);
                    }
                    else if (BinPlacer.Instance.binPlaced == true)
                    {
                    
                    placeBin.gameObject.SetActive(false);
                    if (activatedDrawRoadOnce == false)
                    {
                        startDraw.gameObject.SetActive(true);
                            //scanFloor.GetComponentInChildren<TextMeshProUGUI>().text = "Draw path to the objects";
                            activatedDrawRoadOnce = true;
                    }
                    
                    
                    }

                    
                }

                if(BinPlacer.Instance.itemsCollected == 7)
                {
                    //DrawRoad.Instance.instructDraw.GetComponentInChildren<TextMeshProUGUI>().text = "Throw away the items";
                    visual.SetActive(false);
                }

                
            }
        } else
        {
            placeBin.gameObject.SetActive(false);
            visual.SetActive(false);
           
        }

    }
    

   
}
