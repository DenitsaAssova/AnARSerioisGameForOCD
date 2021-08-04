using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectsSpawner : MonoBehaviour
{
    public GameObject[] officePrefabs;
    public GameObject deskPlane;
    private List<GameObject> spawnedObjs = new List<GameObject>();
    private ARPlane plane;

    private GameObject leftUp, leftDown, rightDown, rightUp;

    private ARRaycastManager rayManager;

    public bool isPlaced = false;
    public PolygonCollider2D planeCollider;

    

    private static ObjectsSpawner instance;

    public static ObjectsSpawner Instance { get { return instance; } }


    private void Awake()
    {
        plane = GetComponent<ARPlane>();

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
        
    }

    public void SpawnPrefabs()
    {

       
        int prefabsToSpawn =officePrefabs.Length;
        prefabsToSpawn -= spawnedObjs.Count;

        if (prefabsToSpawn <= 0)
        {
            return;
        }

        GameObject go = new GameObject();
        PolygonCollider2D col = go.AddComponent<PolygonCollider2D>();

        col.points = plane.boundary.ToArray();
        planeCollider = col;

        for (int i = 0; i < prefabsToSpawn; i++)
        {
            GameObject prefab = officePrefabs[i];
           
            GameObject obj = Instantiate(prefab, transform.position,prefab.transform.rotation , transform);

            spawnedObjs.Add(obj);

             RandomPosInBounds(col, obj);
            plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
            isPlaced = true;

            //GameObject desk = Instantiate(deskPlane, transform.position, transform.rotation);
           // desk.transform.localScale = plane.size;
           // plane.gameObject.SetActive(false);


            
        }

    }

    void RandomPosInBounds(PolygonCollider2D col, GameObject obj )
    {
        //ARPlaneDetector.Instance.areaText.text += " ENTERED RANDOMPOS ";
        Bounds bounds = col.bounds;
        Vector3 center = bounds.center;

        float x;
        float y;

        List<ARRaycastHit> hits1 = new List<ARRaycastHit>();
        List<ARRaycastHit> hits2 = new List<ARRaycastHit>();
        List<ARRaycastHit> hits3 = new List<ARRaycastHit>();
        List<ARRaycastHit> hits4 = new List<ARRaycastHit>();

        int hits_t = 0  ;

        leftUp = obj.transform.Find("LeftUp").gameObject;
        leftDown = obj.transform.Find("LeftDown").gameObject;
        rightUp = obj.transform.Find("RightUp").gameObject;
        rightDown = obj.transform.Find("RightDown").gameObject;

        Ray rayLeftDown, rayLefUp, rayRightDown, rayRightUp;

       // if (leftDown == null || leftUp == null || rightDown == null || rightUp == null) { ARPlaneDetector.Instance.areaText.text = "CORNERS NOT FOUND"; return; } 
       // if (leftDown != null && leftUp != null && rightDown != null && rightDown != null) { ARPlaneDetector.Instance.areaText.text += " CORNERS FOUND "; }


        int checker = 0;
        do
        {
           if (checker > 1500)
            {
               // ARPlaneDetector.Instance.areaText.text = "Cannot place objects in the plane";
                //scanDesk.SetActive(true);
                UICOntroller.Instance.scanDesk.SetActive(true);
                UICOntroller.Instance.scanDesk.GetComponentInChildren<TextMeshProUGUI>().text = "Your desk is too small, please scan again.";
                UICOntroller.Instance.rescan.gameObject.SetActive(true);
               
                obj.SetActive(false);
                break;

            };
            checker++;
            x = Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
            y = Random.Range(center.y - bounds.extents.y, center.y + bounds.extents.y);
            obj.transform.localPosition = new Vector3(x, 0, y);

            rayLeftDown = new Ray(leftDown.transform.position, Vector3.down);
            rayLefUp = new Ray(leftUp.transform.position, Vector3.down);
            rayRightDown = new Ray(rightDown.transform.position, Vector3.down);
            rayRightUp = new Ray(rightUp.transform.position, Vector3.down);


            rayManager.Raycast(rayLeftDown, hits1, TrackableType.PlaneWithinPolygon);
            rayManager.Raycast(rayLefUp, hits2, TrackableType.PlaneWithinPolygon);
            rayManager.Raycast(rayRightDown, hits3, TrackableType.PlaneWithinPolygon);
            rayManager.Raycast(rayRightUp, hits4, TrackableType.PlaneWithinPolygon);


            hits_t = (hits1.Count + hits2.Count + hits3.Count + hits4.Count);

          //  ARPlaneDetector.Instance.areaText.text = "Hits" + hits_t.ToString() + " x: " + x.ToString() + " y: " + y.ToString();

        } while (hits_t < 4 || !col.OverlapPoint(new Vector2(x, y)));


        leftUp.SetActive(false);
        leftDown.SetActive(false);
        rightUp.SetActive(false);
        rightDown.SetActive(false);

    }
}
