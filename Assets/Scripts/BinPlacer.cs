using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BinPlacer : MonoBehaviour
{
    public Transform placementIndicator;
    
    private List<GameObject> furniture = new List<GameObject>();
    private List<GameObject> disposedItems = new List<GameObject>();
    private GameObject currSelectedObject;
    private Camera cam;
    private ARRaycastManager rayManager;
    public bool binPlaced = false;
    private bool isBinOpened = false;
    public int numObjsFromUIPlaced = 0;
    public int itemsCollected = 0;
    public int disposedItemsNum = 0;
    private static BinPlacer instance;
    public GameObject tester;
    public static bool canDraw = false;


    public GameObject magazineButton, shoeButton, milkJugButton, spinnerButton, saltButton, bearStatueButton, boxButton, objsToSpawn2, aimer, aimerImage; 
       public GameObject dumpster;


    public GameObject mascoutPrefab;
    private GameObject mascout;
    public float speed = 0.3f;
    private bool mascoutSpawned = false;

    private GameObject truckPosDumspter;
    public GameObject truckPrefab;
    private GameObject truck;
    public float speedTruck = 0.3f;
    private bool truckSpawed = false;
    private bool waitedEnough = false;
    private Transform afterDumpsterOut;
    private float remainingTime = 5f;
         private float remainingTimeToMenu = 5f;

    public static BinPlacer Instance { get { return instance; } }


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


        // Start is called before the first frame update
        void Start()
    {
        cam = Camera.main;
        rayManager = FindObjectOfType<ARRaycastManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) //if we are touching an ui element
        {
            Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject != null && (furniture.Contains(hit.collider.gameObject) || furniture.Contains(hit.collider.transform.root.gameObject)))
                {
                    if (currSelectedObject != null && hit.collider.gameObject != currSelectedObject && hit.collider.gameObject.tag != "Dumpster")
                    {
                       
                        Select(hit.collider.gameObject);
                    }
                    else if (currSelectedObject == null && hit.collider.gameObject.tag != "Dumpster")
                    {
                        
                        Select(hit.collider.gameObject);
                    } 
                    else
                    {
                        Select(hit.collider.transform.root.gameObject);
                    }

                } else if (hit.collider.name == "DumpsterTop"  && isBinOpened == false)
                {
                    //if(currSelectedObject == null)
                    //{
                        currSelectedObject = hit.collider.transform.root.gameObject;
                    //}
                   
                    isBinOpened = true;
                    currSelectedObject.GetComponent<Animator>().Play("OpenDumpster");
                    
                } else if (hit.collider.name == "DumpsterTop"  && isBinOpened == true)
                {
                   // if (currSelectedObject == null)
                    //{
                        currSelectedObject = hit.collider.transform.root.gameObject;
                    //}
                   
                    isBinOpened = false;
                    
                    currSelectedObject.GetComponent<Animator>().Play("CloseDumpster");

                }


            }
            else
            {

                Deselect();
            }
            
        }

        if (currSelectedObject != null && Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Moved && DrawRoad.canMoveObj == true && disposedItemsNum == 0)
        {
            MoveSelected();
        }
        else if (currSelectedObject != null && Input.touchCount == 2  &&  DrawRoad.canMoveObj == true && disposedItemsNum == 0)
        {
            RotateSelected(currSelectedObject);
        }

        if (itemsCollected == 7)
        {
            DrawRoad.Instance.instructDraw.GetComponentInChildren<TextMeshProUGUI>().text = "Throw away the items";
            magazineButton.GetComponent<Button>().interactable = true;
            shoeButton.GetComponent<Button>().interactable = true;
            milkJugButton.GetComponent<Button>().interactable = true;
            spinnerButton.GetComponent<Button>().interactable = true;
            saltButton.GetComponent<Button>().interactable = true;
            bearStatueButton.GetComponent<Button>().interactable = true;
            boxButton.GetComponent<Button>().interactable = true;
            aimer.SetActive(true);
            aimerImage.SetActive(true);
        }

        if(disposedItemsNum == 7)
        {
            DrawRoad.Instance.instructDraw.gameObject.SetActive(false);
            aimerImage.SetActive(false);
            /*
            if (mascoutSpawned == false)
            {
                Vector3 mascoutPos = new Vector3(dumpster.transform.position.x+5f, dumpster.transform.position.y, dumpster.transform.position.z);
                mascout = Instantiate(mascoutPrefab, mascoutPos, Quaternion.identity);
                mascoutSpawned = true;
            }
            else
            {
                float step = speed * Time.deltaTime;
                mascout.transform.position = Vector3.MoveTowards(mascout.transform.position, BinPlacer.Instance.dumpster.transform.position, step);
                mascout.transform.LookAt(dumpster.transform);
                mascout.GetComponent<Animator>().Play("Walk");

                if (Vector3.Distance(mascout.transform.position, BinPlacer.Instance.dumpster.transform.position) < 0.001f)
                {
                    
                }
            }*/

          if(truckSpawed == false)
            {

                truckPosDumspter = dumpster.transform.Find("TruckPos").gameObject;
                Vector3 truckPos = new Vector3(truckPosDumspter.transform.position.x + 8f, truckPosDumspter.transform.position.y, truckPosDumspter.transform.position.z);
                truck = Instantiate(truckPrefab, truckPos, Quaternion.identity);
                truckSpawed = true;
            }
            else
            {
                
                float step = speed * Time.deltaTime;
               

                if (Vector3.Distance(truck.transform.position, truckPosDumspter.transform.position) != 0f && waitedEnough == false)
                {
                    truck.transform.position = Vector3.MoveTowards(truck.transform.position, truckPosDumspter.transform.position, step);
                    truck.transform.LookAt(truckPosDumspter.transform);
                    truck.GetComponent<Animator>().Play("DriveAnim");
                    

                }
                else
                {
                    waitedEnough = true;
                    truck.GetComponent<Animator>().enabled = false;
                    remainingTime -= Time.deltaTime;
                    if (remainingTime < 0)
                    {
                        DisableFurniture();
                        truck.GetComponent<Animator>().enabled = true;
                      
                    truck.transform.position += truck.transform.forward * Time.deltaTime*speed;
                        truck.GetComponent<Animator>().Play("DriveAnim");
                        dumpster.SetActive(false);
                        remainingTimeToMenu -= Time.deltaTime;
                        if(remainingTimeToMenu < 0)
                        {
                            SceneManager.LoadScene("HoardingGameOver");
                        }
                    }
                }
                
            }

           

        }
    }

    public void PlaceFurniture(GameObject prefab)
    {
        if (PlacementIndicator.Instance.visual.activeInHierarchy)
        {
            Vector3 pos = new Vector3(placementIndicator.position.x, placementIndicator.position.y + 0.015f, placementIndicator.position.z);
            //numObjsFromUIPlaced++;
            binPlaced = true;
            //tester.GetComponentInChildren<TextMeshProUGUI>().text = binPlaced.ToString();
           dumpster = Instantiate(prefab, pos, prefab.transform.rotation);
           // truckPosDumspter = dumpster.transform.Find("TruckPos").gameObject;
            afterDumpsterOut = truckPosDumspter.transform;
            furniture.Add(dumpster);
            canDraw = true;
            Select(dumpster);
        }

    }

    public void PlaceObjsWithButtons(GameObject prefab)
    {
        if (PlacementIndicator.Instance.visual.activeInHierarchy)
        {
            Vector3 pos = new Vector3(placementIndicator.position.x, placementIndicator.position.y + 0.015f, placementIndicator.position.z);
            numObjsFromUIPlaced++;
            GameObject obj = Instantiate(prefab, pos, prefab.transform.rotation);
            furniture.Add(obj);
            Select(obj);

        }
    }

    public void ThrowAwayItems(GameObject prefab)
    {
            GameObject proj = Instantiate(prefab, aimer.transform.position, dumpster.transform.rotation);
        disposedItems.Add(proj);
            proj.GetComponent<Rigidbody>().velocity = cam.transform.forward*3f ;
        disposedItemsNum++;




    }

    public void DisableButtons(Button button)
    {
        if(PlacementIndicator.Instance.visual.activeInHierarchy || aimer.activeInHierarchy)
            button.gameObject.SetActive(false);
        
    }



    public void Select(GameObject selected)
    {
        //
if (selected.tag == "Stroke") tester.GetComponentInChildren<TextMeshProUGUI>().text = "MESH COLLIDER ROAD";

        if (DrawRoad.collectItems == true && selected.tag != "Dumpster")
        {

            objsToSpawn2.gameObject.SetActive(true);

            if (selected.tag == "Shoe") shoeButton.SetActive(true);
            if (selected.tag == "Magazines") magazineButton.SetActive(true);
            if (selected.tag == "MilkJug") milkJugButton.SetActive(true);
            if (selected.tag == "Salt") saltButton.SetActive(true);
            if (selected.tag == "Statue") bearStatueButton.SetActive(true);
            if (selected.tag == "Box") boxButton.SetActive(true);
            if (selected.tag == "Spinner") spinnerButton.SetActive(true);
            itemsCollected++;
            selected.SetActive(false);



        }
        else
        {
            currSelectedObject = selected;
        }

        
        

    }

    void Deselect()
    {
       
        currSelectedObject = null;
       
    }

    void MoveSelected()
    {
        Vector3 curPos = cam.ScreenToViewportPoint(Input.touches[0].position);
        Vector3 lastPos = cam.ScreenToViewportPoint(Input.touches[0].position - Input.touches[0].deltaPosition);

        Vector3 touchDir = curPos - lastPos;

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        List<ARRaycastHit> hit = new List<ARRaycastHit>();


        Vector3 checkIfValidPos = currSelectedObject.transform.position + (camRight * touchDir.x + camForward * touchDir.y);
        Ray ray = new Ray(checkIfValidPos, Vector3.down);
        rayManager.Raycast(ray, hit, TrackableType.PlaneWithinPolygon);

        Vector3 lastAllowedPostion = currSelectedObject.transform.position;


        if (hit.Count <= 0)
        {
            currSelectedObject.transform.position = lastAllowedPostion;
        }
        else
        {
            currSelectedObject.transform.position += (camRight * touchDir.x + camForward * touchDir.y);
            lastAllowedPostion = currSelectedObject.transform.position;
        }
    }

    void RotateSelected(GameObject obj)
    {
       
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        Vector2 touchDeltaDir = (touchZero.position - touchOne.position).normalized;
        Vector2 prevTouchDeltaDir = (touchZeroPrevPos - touchOnePrevPos).normalized;

        float v = Mathf.Atan2(touchDeltaDir.y, touchDeltaDir.x) * Mathf.Rad2Deg;
        float v1 = Mathf.Atan2(prevTouchDeltaDir.y, prevTouchDeltaDir.x) * Mathf.Rad2Deg;

        float delta = v - v1;
        if (obj.tag == "Magazines")
        {
            //tester.GetComponent<TextMeshProUGUI>().text = obj.transform.rotation.eulerAngles.ToString();
            obj.transform.Rotate( 0, 0, delta);
        }
        else
        {
            

            obj.transform.Rotate(0, delta, 0);
        }
    }

   void DisableFurniture()
    {
        for (int i = 0; i < disposedItems.Count; i++)
            Destroy(disposedItems[i]);
    }
}
