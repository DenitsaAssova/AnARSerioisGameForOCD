using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class OnRoadChecker : MonoBehaviour
{
   
    public static float timeOnROad = 0f;
    public GameObject roadRayPos;
    private ARRaycastManager rayManager;
   
    // Update is called once per frame
    private void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);
        // Vector3 projectedPos = new Vector3();

        if (hits.Count > 0 && DrawRoad.roadDrawn)
        {

            timeOnROad += Time.deltaTime;
            RaycastHit hit;
            //BinPlacer.Instance.tester.GetComponentInChildren<TextMeshProUGUI>().text = "EnTEST";

            if (Physics.Raycast(roadRayPos.transform.position, Vector3.down, out hit, Mathf.Infinity))

            {

               // BinPlacer.Instance.tester.GetComponentInChildren<TextMeshProUGUI>().text = "Entered if for check on road";
                if (hit.collider.tag == "Stroke")
                {

                    //BinPlacer.Instance.tester.GetComponentInChildren<TextMeshProUGUI>().text = "MESH WORKS";
                    timeOnROad -= Time.deltaTime;
                }

            }

            if (BinPlacer.Instance.disposedItemsNum == 7)
            {
                //BinPlacer.Instance.tester.GetComponentInChildren<TextMeshProUGUI>().text = "Time not on road:" + Mathf.FloorToInt(timeOnROad).ToString();
                //display menu
               
                enabled = false;
            }

        }

            
    }
}
