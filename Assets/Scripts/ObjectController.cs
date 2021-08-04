using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Defines the <see cref="ObjectController" />.
/// </summary>
public class ObjectController : MonoBehaviour
{
    /// <summary>
    /// Defines the office_supplies_tag.
    /// </summary>
    private string office_supplies_tag = "OfficeSupply";
    public bool turnOnTimer = false;
    private bool stopMovement = false;
    public bool canMove = true;

    public GameObject validRotText;
    public GameObject validPosText;
    public GameObject allObjsPlaced;
    private float remTimeEndGame = 2f;
    public GameObject timer;
    /// <summary>
    /// Defines the currSelectedObject.
    /// </summary>
    private GameObject currSelectedObject;

    /// <summary>
    /// Defines the cam.
    /// </summary>
    private Camera cam;

    /// <summary>
    /// Defines the rayManager.
    /// </summary>
    private ARRaycastManager rayManager;

    /// <summary>
    /// Defines the rotationFactor.
    /// </summary>
    public float rotationFactor = 1.0f;

    /// <summary>
    /// Defines the corectlyRotatedObjectsNum.
    /// </summary>
    private int corectlyRotatedObjectsNum = 0;

    /// <summary>
    /// Defines the corectlyRotatedObjects.
    /// </summary>
    private Dictionary<string, bool> corectlyRotatedObjects = new Dictionary<string, bool>();

    /// <summary>
    /// Defines the corectlyPlacedObjectsNum.
    /// </summary>
    private int corectlyPlacedObjectsNum = 0;

    /// <summary>
    /// Defines the corectlyPlacedObjects.
    /// </summary>
    private Dictionary<string, bool> corectlyPlacedObjects = new Dictionary<string, bool>();

    /// <summary>
    /// Defines the UpLeft, UpRight, DownLeft, DownRight..
    /// </summary>
    private GameObject UpLeft, UpRight, DownLeft, DownRight, Down, Up, Down_1, Up_1;

    private static ObjectController instance;

    public static ObjectController Instance { get { return instance; } }


    private void Awake()
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

    /// <summary>
    /// The Start.
    /// </summary>
    void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();
        //counter = GameObject.Find("Loading");
        //counter.SetActive(false);
        cam = Camera.main;
       // validRotText = GameObject.Find("ValidRotText").GetComponent<TextMeshPro>();
        //validPosText = GameObject.Find("ValidPosText").GetComponent<TextMeshPro>();

    }

    /// <summary>
    /// The Update.
    /// </summary>
    void Update()
    {
        //if (office_supplies_tag.Length == 0) ARPlaneDetector.Instance.areaText.text = "NOTHING IN LIST BITCH";

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) //if we are touching an ui element, used for the info button
        {
            //ARPlaneDetector.Instance.areaText.text = "Touch registered ";
            Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // ARPlaneDetector.Instance.areaText.text = "Touch registered and Raycast hit " + hit.collider.tag;
                if (hit.collider.gameObject != null && office_supplies_tag.Contains(hit.collider.tag))
                {
                    //ARPlaneDetector.Instance.areaText.text = "Touch registered and Raycast hit sth and entered selection loop" ;
                    if (currSelectedObject != null && hit.collider.gameObject != currSelectedObject)
                    {
                        Select(hit.collider.gameObject);
                    }
                    else if (currSelectedObject == null)
                    {
                        Select(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                // ARPlaneDetector.Instance.areaText.text = "Touch registered and Raycast hit sth and entered deselection loop";
                Deselect();
            }
        }
        CheckPlacementOfObjects();

        if (currSelectedObject != null && Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Moved && canMove)
        {
            MoveSelected();
        }
        else if (currSelectedObject != null && Input.touchCount == 2 && canMove)
        {
            RotateSelected(currSelectedObject);
        }
    }

    /// <summary>
    /// The RotateSelected.
    /// </summary>
    /// <param name="obj">The obj<see cref="GameObject"/>.</param>
    void RotateSelected(GameObject obj)
    {
        if (stopMovement == false)
        {
            validRotText.GetComponent<TextMeshProUGUI>().text = "";
            validRotText.SetActive(false);


            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            Vector2 touchDeltaDir = (touchZero.position - touchOne.position).normalized;
            Vector2 prevTouchDeltaDir = (touchZeroPrevPos - touchOnePrevPos).normalized;

            float v = Mathf.Atan2(touchDeltaDir.y, touchDeltaDir.x) * Mathf.Rad2Deg;
            float v1 = Mathf.Atan2(prevTouchDeltaDir.y, prevTouchDeltaDir.x) * Mathf.Rad2Deg;

            float delta = v - v1;

            if (obj.name == "Headphones")
            {
                obj.transform.Rotate(0, 0, delta * rotationFactor);

                if (obj.transform.localEulerAngles.x == 90 &&
                        (obj.transform.localEulerAngles.y >= 0 && obj.transform.localEulerAngles.y <= 5 || obj.transform.localEulerAngles.y <= 360 && obj.transform.localEulerAngles.y >= 355) &&
                        obj.transform.localEulerAngles.z == 0)
                {
                    corectlyRotatedObjects[obj.name] = true;
                    validRotText.SetActive(true);
                    validRotText.GetComponent<TextMeshProUGUI>().text = "Valid Rotation";
                }
                else
                {
                    corectlyRotatedObjects[obj.name] = false;
                    validRotText.SetActive(true);
                    validRotText.GetComponent<TextMeshProUGUI>().text = "Invalid Rotation";
                }

            }
            else if (obj.name == "Router_Pro")
            {
                GameObject selected = new GameObject();
                GameObject newSelected = new GameObject();
                foreach (Transform child in obj.transform)
                {
                    if (child.gameObject.name == "Selected")
                    {
                        selected = child.gameObject;

                    }
                    if (child.gameObject.name == "New_Selected")
                    {
                        newSelected = child.gameObject;
                    }
                }



                if (obj.transform.localEulerAngles.x > 3 && obj.transform.localEulerAngles.x <= 90)
                {
                    obj.transform.Rotate(delta * rotationFactor, 0, 0);
                    selected.transform.Rotate(-delta * rotationFactor, 0, 0);

                }
                else
                {

                    selected.transform.localPosition = newSelected.transform.localPosition;
                    selected.transform.localRotation = newSelected.transform.localRotation;
                    selected.transform.localScale = newSelected.transform.localScale;



                    obj.transform.Rotate(0, delta * rotationFactor, 0);


                    if (
                            (obj.transform.localEulerAngles.y >= 0 && obj.transform.localEulerAngles.y <= 5 || obj.transform.localEulerAngles.y <= 360 && obj.transform.localEulerAngles.y >= 355))
                    {
                        corectlyRotatedObjects[obj.name] = true;
                        validRotText.SetActive(true);
                        validRotText.GetComponent<TextMeshProUGUI>().text = "Valid Rotation";
                    }
                    else
                    {
                        corectlyRotatedObjects[obj.name] = false;
                        validRotText.SetActive(true);
                        validRotText.GetComponent<TextMeshProUGUI>().text = "Invalid Rotation";
                    }

                }


            }
            else
            {
                obj.transform.Rotate(0, delta * rotationFactor, 0);

                if (obj.name == "Notepad" || obj.name == "Pen red" || obj.name == "Pen blue (1)" || obj.name == "Pen blue"
                     || obj.name == "Pencil" || obj.name == "Eraser" || obj.name == "Book1_Pro" || obj.name == "Book2_Pro" || obj.name == "Book3_Pro"
                     || obj.name == "Laptop")
                {
                    if (obj.transform.localEulerAngles.x == 0 &&
                        (obj.transform.localEulerAngles.y >= 0 && obj.transform.localEulerAngles.y <= 5 || obj.transform.localEulerAngles.y <= 360 && obj.transform.localEulerAngles.y >= 355) &&
                        obj.transform.localEulerAngles.z == 0)
                    {
                        corectlyRotatedObjects[obj.name] = true;
                        validRotText.SetActive(true);
                        validRotText.GetComponent<TextMeshProUGUI>().text = "Valid Rotation";
                    }
                    else
                    {
                        corectlyRotatedObjects[obj.name] = false;
                        validRotText.SetActive(true);
                        validRotText.GetComponent<TextMeshProUGUI>().text = "Invalid Rotation";
                    }
                }
                else if (obj.name == "Printer")
                {
                    if (obj.transform.localEulerAngles.x == 0 &&
                       (obj.transform.localEulerAngles.y <= 180 && obj.transform.localEulerAngles.y >= 175 || obj.transform.localEulerAngles.y >= -180 && obj.transform.localEulerAngles.y <= -175) &&
                       obj.transform.localEulerAngles.z == 0)
                    {
                        corectlyRotatedObjects[obj.name] = true;
                        validRotText.SetActive(true);
                        validRotText.GetComponent<TextMeshProUGUI>().text = "Valid Rotation";
                    }
                    else
                    {
                        corectlyRotatedObjects[obj.name] = false;

                        validRotText.SetActive(true);
                        validRotText.GetComponent<TextMeshProUGUI>().text = "Invalid Rotation";
                    }
                }


            }
        }
    }

    /// <summary>
    /// The MoveSelected.
    /// </summary>
    void MoveSelected()
    { if(stopMovement == false)

        { 
        validPosText.GetComponent<TextMeshProUGUI>().text = "";
            validPosText.SetActive(false);

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
                if (currSelectedObject.name == "Book2_Pro")
                {
                    RaycastHit hits1;
                    RaycastHit hits2;
                    RaycastHit hits3;
                    RaycastHit hits4;

                    UpLeft = currSelectedObject.transform.Find("UpLeft").gameObject;
                    DownLeft = currSelectedObject.transform.Find("DownLeft").gameObject;
                    UpRight = currSelectedObject.transform.Find("UpRight").gameObject;
                    DownRight = currSelectedObject.transform.Find("DownRight").gameObject;

                    if (Physics.Raycast(UpLeft.transform.position, Vector3.down, out hits1, Mathf.Infinity)
                         && Physics.Raycast(UpRight.transform.position, Vector3.down, out hits2, Mathf.Infinity)
                         && Physics.Raycast(DownLeft.transform.position, Vector3.down, out hits3, Mathf.Infinity)
                         && Physics.Raycast(DownRight.transform.position, Vector3.down, out hits4, Mathf.Infinity))
                    {
                        if (hits1.collider.gameObject.name == "Book1_Pro" && hits2.collider.gameObject.name == "Book1_Pro"
                            && hits3.collider.gameObject.name == "Book1_Pro" && hits4.collider.gameObject.name == "Book1_Pro")
                        {
                            //corectlyPlacedObjects["Book1_Pro"] = true;
                            corectlyPlacedObjects["Book2_Pro"] = true;
                            //ARPlaneDetector.Instance.areaText.text = "Book1 and 2 correctly placed";
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";

                        }
                        else
                        {
                            // corectlyPlacedObjects["Book1_Pro"] = false;
                            corectlyPlacedObjects["Book2_Pro"] = false;
                            // ARPlaneDetector.Instance.areaText.text = "Book1 and 2 INCORRECTLYs placed";
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                        }

                    }
                    else
                    {
                        corectlyPlacedObjects["Book2_Pro"] = false;
                        // ARPlaneDetector.Instance.areaText.text = "Book1 and 2 INCORRECTLYs placed";
                        validPosText.SetActive(true);
                        validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";

                    }
                }
                else if (currSelectedObject.name == "Book3_Pro")
                {

                    RaycastHit hits1;
                    RaycastHit hits2;
                    RaycastHit hits3;
                    RaycastHit hits4;

                    // int hits_t = 0;

                    UpLeft = currSelectedObject.transform.Find("UpLeft").gameObject;
                    DownLeft = currSelectedObject.transform.Find("DownLeft").gameObject;
                    UpRight = currSelectedObject.transform.Find("UpRight").gameObject;
                    DownRight = currSelectedObject.transform.Find("DownRight").gameObject;

                    if (Physics.Raycast(UpLeft.transform.position, Vector3.down, out hits1, Mathf.Infinity)
                         && Physics.Raycast(UpRight.transform.position, Vector3.down, out hits2, Mathf.Infinity)
                         && Physics.Raycast(DownLeft.transform.position, Vector3.down, out hits3, Mathf.Infinity)
                         && Physics.Raycast(DownRight.transform.position, Vector3.down, out hits4, Mathf.Infinity))
                    {
                        if (hits1.collider.gameObject.name == "Book2_Pro" && hits2.collider.gameObject.name == "Book2_Pro"
                            && hits3.collider.gameObject.name == "Book2_Pro" && hits4.collider.gameObject.name == "Book2_Pro")
                        {
                            //corectlyPlacedObjects["Book2_Pro"] = true;
                            corectlyPlacedObjects["Book3_Pro"] = true;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";

                        }
                        else
                        {
                            //corectlyPlacedObjects["Book2_Pro"] = false;
                            corectlyPlacedObjects["Book3_Pro"] = false;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                        }


                    }
                    else
                    {
                        corectlyPlacedObjects["Book2_Pro"] = false;
                        corectlyPlacedObjects["Book3_Pro"] = false;
                        validPosText.SetActive(true);
                        validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                    }
                }
                else if (currSelectedObject.name == "Notepad")
                {
                    RaycastHit hits1;
                    RaycastHit hits2;
                    Up = currSelectedObject.transform.Find("Up").gameObject;
                    Down = currSelectedObject.transform.Find("Down").gameObject;
                    if (Physics.Raycast(Down.transform.position, Vector3.right, out hits1, 0.08f) &&
                        Physics.Raycast(Up.transform.position, Vector3.right, out hits2, 0.08f))
                    {
                        if (hits1.collider.gameObject.name == "Pen blue (1)" && hits2.collider.gameObject.name == "Pen blue (1)" || hits1.collider.gameObject.name == "Pen blue" && hits2.collider.gameObject.name == "Pen blue")
                        {
                            corectlyPlacedObjects["Notepad"] = true;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";

                        }
                        else
                        {
                            corectlyPlacedObjects["Notepad"] = false;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                        }
                    }
                    else
                    {
                        corectlyPlacedObjects["Notepad"] = false;
                        validPosText.SetActive(true);
                        validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";

                    }
                }
                else if (currSelectedObject.name == "Pen blue")
                {
                    RaycastHit hits1;
                    RaycastHit hits2;

                    Down = currSelectedObject.transform.Find("Down").gameObject;
                    Up = currSelectedObject.transform.Find("Up").gameObject;

                    if (Physics.Raycast(Down.transform.position, Vector3.right, out hits1, 0.08f) 
                        && Physics.Raycast(Up.transform.position, Vector3.right, out hits2, 0.08f))
                    {
                        if (hits1.collider.gameObject.name == "Pen blue (1)" && hits2.collider.gameObject.name == "Pen blue (1)")
                        {
                            corectlyPlacedObjects["Pen blue"] = true;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";
                        }
                        else if (hits1.collider.gameObject.name == "Pen red" && hits2.collider.gameObject.name == "Pen red")
                        {
                            corectlyPlacedObjects["Pen blue"] = true;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";
                        }
                        else
                        {
                            corectlyPlacedObjects["Pen blue"] = false;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                        }
                    }
                    else
                    {
                        corectlyPlacedObjects["Pen blue"] = false;
                        validPosText.SetActive(true);
                        validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                    }
                }
                else if (currSelectedObject.name == "Pen blue (1)")
                {
                    RaycastHit hits1;
                    RaycastHit hits2;

                    Down = currSelectedObject.transform.Find("Down").gameObject;
                    Up = currSelectedObject.transform.Find("Up").gameObject;

                    if (Physics.Raycast(Down.transform.position, Vector3.right, out hits1, 0.08f) 
                        && Physics.Raycast(Up.transform.position, Vector3.right, out hits2, 0.08f))
                    {
                        if (hits1.collider.gameObject.name == "Pen blue" && hits2.collider.gameObject.name == "Pen blue")
                        {
                            corectlyPlacedObjects["Pen blue (1)"] = true;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";
                        }
                        else if (hits1.collider.gameObject.name == "Pen red" && hits2.collider.gameObject.name == "Pen red")
                        {
                            corectlyPlacedObjects["Pen blue (1)"] = true;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";
                        }
                        else
                        {
                            corectlyPlacedObjects["Pen blue (1)"] = false;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                        }
                    }
                    else
                    {
                        corectlyPlacedObjects["Pen blue (1)"] = false;
                        validPosText.SetActive(true);
                        validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                    }
                }
                else if (currSelectedObject.name == "Pen red")
                {
                    RaycastHit hits1;
                    RaycastHit hits2;

                    Down = currSelectedObject.transform.Find("Down").gameObject;
                    Up = currSelectedObject.transform.Find("Up").gameObject;

                    if (Physics.Raycast(Down.transform.position, Vector3.right, out hits1, 0.08f)
                        && Physics.Raycast(Up.transform.position, Vector3.right, out hits2, 0.08f))
                    {
                        if (hits1.collider.gameObject.name == "Pencil" && hits2.collider.gameObject.name == "Pencil")
                        {
                            corectlyPlacedObjects["Pen red"] = true;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";
                        }
                        else
                        {
                            corectlyPlacedObjects["Pen red"] = false;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                        }
                    }
                    else
                    {
                        corectlyPlacedObjects["Pen red"] = false;
                        // corectlyPlacedObjects["Eraser"] = false;
                        validPosText.SetActive(true);
                        validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                    }
                }
                else if (currSelectedObject.name == "Pencil")
                {
                    RaycastHit hits1;
                    RaycastHit hits2;
                    Up = currSelectedObject.transform.Find("Up").gameObject;
                    Down = currSelectedObject.transform.Find("Down").gameObject;
                    if (Physics.Raycast(Down.transform.position, Vector3.right, out hits1, 0.08f) &&
                        Physics.Raycast(Up.transform.position, Vector3.right, out hits2, 0.08f))
                    {
                        if (hits1.collider.gameObject.name == "Eraser" && hits2.collider.gameObject.name == "Eraser")
                        {
                            corectlyPlacedObjects["Pencil"] = true;
                            //corectlyPlacedObjects["Eraser"] = true;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";
                        }
                        else
                        {
                            corectlyPlacedObjects["Pencil"] = false;
                            // corectlyPlacedObjects["Eraser"] = false;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                        }
                    }
                    else
                    {
                        corectlyPlacedObjects["Pencil"] = false;
                        // corectlyPlacedObjects["Eraser"] = false;
                        validPosText.SetActive(true);
                        validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                    }
                }
                else if (currSelectedObject.name == "Laptop")
                {
                    RaycastHit hits1;
                    RaycastHit hits2;
                    RaycastHit hits3;
                    RaycastHit hits4;
                    Up = currSelectedObject.transform.Find("Up").gameObject;
                    Down = currSelectedObject.transform.Find("Down").gameObject;
                    Up_1 = currSelectedObject.transform.Find("Up (1)").gameObject;
                    Down_1 = currSelectedObject.transform.Find("Down (1)").gameObject;

                    if (Physics.Raycast(Down.transform.position, Vector3.right, out hits1, 0.2f) &&
                        Physics.Raycast(Up.transform.position, Vector3.right, out hits2, 0.2f) &&
                        Physics.Raycast(Up_1.transform.position, Vector3.left, out hits3, 0.2f) &&
                        Physics.Raycast(Down_1.transform.position, Vector3.left, out hits4, 0.2f))
                    {
                        if (hits1.collider.gameObject.name == "Notepad" && hits2.collider.gameObject.name == "Notepad" &&
                            hits3.collider.gameObject.name == "Headphones" && hits4.collider.gameObject.name == "Headphones")
                        {
                            corectlyPlacedObjects["Laptop"] = true;
                            //corectlyPlacedObjects["Headphones"] = true;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";
                        }
                        else
                        {
                            corectlyPlacedObjects["Laptop"] = false;
                            //corectlyPlacedObjects["Headphones"] = false;
                            validPosText.SetActive(true);
                            validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                        }
                    }
                    else
                    {
                        corectlyPlacedObjects["Laptop"] = false;
                        //corectlyPlacedObjects["Headphones"] = false;
                        validPosText.SetActive(true);
                        validPosText.GetComponent<TextMeshProUGUI>().text = "Invalid Position";
                    }
                }
                else
                {
                    corectlyPlacedObjects[currSelectedObject.name] = true;
                    validPosText.SetActive(true);
                    validPosText.GetComponent<TextMeshProUGUI>().text = "Valid Position";
                }

            }
        }
    }

    /// <summary>
    /// The Select.
    /// </summary>
    /// <param name="selected">The selected<see cref="GameObject"/>.</param>
    void Select(GameObject selected)
    {
        validPosText.GetComponent<TextMeshProUGUI>().text = "";
        validPosText.SetActive(false);
        validRotText.GetComponent<TextMeshProUGUI>().text = "";
        validRotText.SetActive(false);
        //if (currSelectedObject != null) { ToggleSelectionVisual(currSelectedObject, false); }
        currSelectedObject = selected;
       // ToggleSelectionVisual(currSelectedObject, true);
        corectlyRotatedObjects.Add(selected.name, false);
    }

    /// <summary>
    /// The Deselect.
    /// </summary>
    void Deselect()
    {
        validPosText.GetComponent<TextMeshProUGUI>().text = "";
        validPosText.SetActive(false);
        validRotText.GetComponent<TextMeshProUGUI>().text = "";
        validRotText.SetActive(false);
        //if (currSelectedObject != null) { ToggleSelectionVisual(currSelectedObject, false); }
        currSelectedObject = null;
    }

    /// <summary>
    /// The ToggleSelectionVisual.
    /// </summary>
    /// <param name="obj">The obj<see cref="GameObject"/>.</param>
    /// <param name="toogle">The toogle<see cref="bool"/>.</param>
    void ToggleSelectionVisual(GameObject obj, bool toogle)
    {
        obj.transform.Find("Selected").gameObject.SetActive(toogle);
    }

    /// <summary>
    /// The CheckPlacementOfObjects.
    /// </summary>
    void CheckPlacementOfObjects()
    {
        string unrotated = "";
        string unplaced = "";

        corectlyRotatedObjectsNum = 0;
        corectlyPlacedObjectsNum = 0;

        foreach (var obj in corectlyRotatedObjects)
        {
            if (obj.Value == true) corectlyRotatedObjectsNum++;
            else unrotated = obj.Key;
            //ARPlaneDetector.Instance.areaText.text = "+ One object left to rotate"
        }

        foreach(var obj in corectlyPlacedObjects){
            if (obj.Value == true) corectlyPlacedObjectsNum++;
            else unplaced = obj.Key;
        }


        if (corectlyRotatedObjectsNum == 12 && corectlyPlacedObjectsNum==12  || corectlyRotatedObjectsNum == 13 && corectlyPlacedObjectsNum==12
            ||corectlyPlacedObjectsNum==13 && corectlyRotatedObjectsNum == 12)
        {
            // ARPlaneDetector.Instance.areaText.text = " One object left to PLACE AND ROTATE "+"unplaced: " +unplaced+" unrotaed: "+unrotated ;

            //call coroutine for waiting
            //  if (coroutineCalled == false) 
            //Timer(6f);
            //StartCoroutine(WaitBeforePlacingLastObjecy(6f));
            validPosText.GetComponent<TextMeshProUGUI>().text = "";
            validPosText.SetActive(false);
            validRotText.GetComponent<TextMeshProUGUI>().text = "";
            validRotText.SetActive(false);
            turnOnTimer = true;

        } else if(corectlyPlacedObjectsNum == 13 && corectlyRotatedObjectsNum == 13)
        {
            //canMove = false;
            //allObjsPlaced.SetActive(true);
            //allObjsPlaced.GetComponent<TextMeshProUGUI>().text = "ALL OBJECTS PLACED CORRECTLY :)";
            //validPosText.GetComponent<TextMeshProUGUI>().text = "All Objects Placed Correctly :)";
            //validRotText.GetComponent<TextMeshProUGUI>().text = canMove.ToString();

            //stopMovement = true;



            canMove = false;
            remTimeEndGame -= Time.deltaTime;

                if (remTimeEndGame < 0)
                {
                SceneManager.LoadScene("AllObjectsPlacedCorrectly");

            }
            
            

        }
       
        /*else
        {
            corectlyRotatedObjectsNum = 0;
            corectlyPlacedObjectsNum = 0;
        }*/
    }

}
