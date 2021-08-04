using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlanePainter : MonoBehaviour
{
    public GameObject[] backteriaPrefabs;
    private List<GameObject> spawnedObjs = new List<GameObject>();
    private List<Vector2> usedPos = new List<Vector2>();
    private List<float> disBacToTouch = new List<float>();

    private Dictionary<float, GameObject> dictObjDistance = new Dictionary<float, GameObject>();

    public static int minNumOfSpawnedObjs = 5;
    public static int bacsToKill = 0;
    public static int prefabsToSpawnPublic = 0;

    private ARPlane plane;
    public float areaPerPrefab;
    public PolygonCollider2D colPublic;

    private ARRaycastManager rayManager;

    public GameObject tester;

    public static bool touched = false;
    public static bool bacSpawned = false;
    public static bool firstTouched = false;
    private bool rayCasted = false;

    private float speed = 0.6f;
    private Camera cam;

    private static PlanePainter instance;

    public static PlanePainter Instance { get { return instance; } }



    // Start is called before the first frame update
    void Awake ()
    {
       
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        plane = GetComponent<ARPlane>();
       // tester = GameObject.Find("Tester");
    }

    private void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();
        cam = Camera.main;
        tester = GameObject.Find("Tester");

    }


    private void Update()
    {
       // && Input.touches[0].phase == TouchPhase.Stationary


        if (Input.touchCount > 0  && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId) && CleaningUiController.continuePressed )
        {

            //Vector3 pos = cam.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, cam.nearClipPlane));
            Ray ray = cam.ScreenPointToRay(Input.touches[0].position);

            RaycastHit hit;

            //if (Physics.Raycast(pos, cam.transform.forward, out hit, Mathf.Infinity))
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "ARPLane" && CleaningUiController.timerUp == false)
                {
                    touched = true;
                    firstTouched = true;
                    disBacToTouch = new List<float>();
                    dictObjDistance = new Dictionary<float, GameObject>();

                    float planeArea = plane.size.x * plane.size.y;

                    // calculate how many prefabs we need to spawn
                    prefabsToSpawnPublic = Mathf.FloorToInt(planeArea / areaPerPrefab);
                    //tester.GetComponent<TextMeshProUGUI>().text = "Painter: " + prefabsToSpawnPublic.ToString();
                    //if(spawnedObjs.Count < 5)
                    if (spawnedObjs.Count < prefabsToSpawnPublic)
                    {
                        //for (int x = 0; x < minNumOfSpawnedObjs - spawnedObjs.Count; ++x)
                        for (int x = 0; x < prefabsToSpawnPublic - spawnedObjs.Count; ++x)
                        {
                            // get a random one
                            GameObject randomPrefab = backteriaPrefabs[Random.Range(0, backteriaPrefabs.Length)];

                            // instantiate it
                            GameObject obj = Instantiate(randomPrefab, transform.position, randomPrefab.transform.rotation, transform);
                            spawnedObjs.Add(obj);

                            // set its position to a random point on the mesh
                            RandomPosInBounds(colPublic, obj);

                        }
                    }

                    for (int i = 0; i<spawnedObjs.Count; i++)
                    {
                        //float step = speed * Time.deltaTime;
                        // spawnedObjs[i].transform.position = Vector3.MoveTowards(spawnedObjs[i].transform.position, hit.point, step);
                        float distToTouch = Vector3.Distance(spawnedObjs[i].transform.position, hit.point);
                        disBacToTouch.Add(distToTouch);
                        dictObjDistance[distToTouch] = spawnedObjs[i];



                    }
                    float step = speed * Time.deltaTime;
                    disBacToTouch.Sort((p1, p2) => p1.CompareTo(p2));
                    dictObjDistance[disBacToTouch[0]].transform.position = Vector3.MoveTowards(dictObjDistance[disBacToTouch[0]].transform.position, hit.point, step);
                    if (Vector3.Distance(dictObjDistance[disBacToTouch[0]].transform.position, hit.point) <= 0.1f)
                    {
                        spawnedObjs.Remove(dictObjDistance[disBacToTouch[0]]);
                        Destroy(dictObjDistance[disBacToTouch[0]]);
                    }

                    

                  


                }
            }


        }
        else
        {
            touched = false;
        }
        
    }

    void OnEnable ()
    {
        plane.boundaryChanged += OnPlaneBoundaryChanged;
    }

    void OnDisable ()
    {
        plane.boundaryChanged -= OnPlaneBoundaryChanged;
    }

    // called when the plane's boundary has been updated
    void OnPlaneBoundaryChanged (ARPlaneBoundaryChangedEventArgs args)
    {
        // get the area of the plane
        float planeArea = plane.size.x * plane.size.y;

        // calculate how many prefabs we need to spawn
        int prefabsToSpawn = Mathf.FloorToInt(planeArea / areaPerPrefab);
      
        prefabsToSpawn -= spawnedObjs.Count;
        
        // if there are none to spawn then return
        if(prefabsToSpawn <= 0)
            return;

        GameObject go = new GameObject();
        PolygonCollider2D col = go.AddComponent<PolygonCollider2D>();
       

        col.points = plane.boundary.ToArray();
        colPublic = col;

        // create a number of new nature prefabs
        for (int x = 0; x < prefabsToSpawn; ++x)
        {
            // get a random one
            GameObject randomPrefab = backteriaPrefabs[Random.Range(0, backteriaPrefabs.Length)];

            // instantiate it
            GameObject obj = Instantiate(randomPrefab, transform.position, randomPrefab.transform.rotation, transform);
            spawnedObjs.Add(obj);

            bacSpawned = true;

            // set its position to a random point on the mesh
            RandomPosInBounds(col, obj);
            
        }
    }

    // returns a random position within the sent polygon collider
   public void RandomPosInBounds (PolygonCollider2D col, GameObject obj)
    {
        Bounds bounds = col.bounds;
        Vector3 center = bounds.center;
        bool valid = true;

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        Ray ray;

       GameObject boundary = obj.transform.Find("Boundary").gameObject;

        if(boundary == null)
        tester.GetComponent<TextMeshProUGUI>().text = "NULLLLL";

        float x = 0;
        float y = 0;

        do
        {
            int i = 0;
            valid = true;
            x = Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
            y = Random.Range(center.y - bounds.extents.y, center.y + bounds.extents.y);
            obj.transform.localPosition = new Vector3(x, 0, y);

            ray = new Ray(boundary.transform.position, Vector3.down);
            rayCasted = rayManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon);

            for (i = 0; i < usedPos.Count; i++)
            {
                Vector2 pos = usedPos[i];
                if(Vector2.Distance(pos, new Vector2(x, y))<0.1f)
                {
                    valid = false;
                }
            }
        }
        while(!col.OverlapPoint(new Vector2(x, y)) || usedPos.Contains(new Vector2(x,y)) || valid ==false || hits.Count<1 || rayCasted==false);

        usedPos.Add(new Vector2(x, y));
        
       
    }
}