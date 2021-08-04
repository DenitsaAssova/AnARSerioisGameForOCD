using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

public class ARPlaneDetector : MonoBehaviour
{

    public TextMeshPro areaText;
    public ARPlane arPlane;

    public TrackableId desk_plane_ID;

    private ARPlaneManager planeManager;
    //private ARSession arSession;

    private GameObject leftUp, leftDown, rightDown, rightUp;

    private static ARPlaneDetector instance;

    public GameObject objToSpawn;
        private GameObject badCharacter;

    private Animator chAnim;

    public float scanTIme = 15f;
    private bool toRescan = true;

    public static ARPlaneDetector Instance { get { return instance; } }


    
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

        planeManager = FindObjectOfType<ARPlaneManager>();
       // arSession = FindObjectOfType<ARSession>().GetComponent<ARSession>();
        areaText.text = "";
        


    }

    private void Start()
    {
       // badCharacter = GameObject.Find("Bad_character_StartMenu");
       
      //  chAnim = badCharacter.GetComponent<Animator>();
       // rescan.gameObject.SetActive(false);
        
    }

    void Update()
    {
       // chAnim.Play("Idle_Stance_01");
        areaText.transform.rotation =
        Quaternion.LookRotation(areaText.transform.position -
           Camera.main.transform.position);

       
        scanTIme -= Time.deltaTime;
        if (scanTIme < 0 && toRescan)
        {
            UICOntroller.Instance.scanDesk.GetComponentInChildren<TextMeshProUGUI>().text = "Your desk is too small, please scan again.";
            UICOntroller.Instance.rescan.gameObject.SetActive( true);
        }

    }

    private void OnEnable()
    {
        arPlane.boundaryChanged += OnPlaneBoundaryChanged;
    }

    private void OnDisable()
    {
        arPlane.boundaryChanged -= OnPlaneBoundaryChanged;
    }

    void OnPlaneBoundaryChanged(ARPlaneBoundaryChangedEventArgs args)
    {
        //BoxCollider collider = objToSpawn.GetComponent<BoxCollider>();

        //Vector3 collider_size = objToSpawn.GetComponent<BoxCollider>().size;


        leftUp = objToSpawn.transform.Find("LeftUp").gameObject;
        leftDown = objToSpawn.transform.Find("LeftDown").gameObject;
        rightUp = objToSpawn.transform.Find("RightUp").gameObject;
        rightDown = objToSpawn.transform.Find("RightDown").gameObject;

        float distance_left_right_down = Vector3.Distance(leftDown.transform.position, rightDown.transform.position);
        float distance_left_right_up = Vector3.Distance(leftUp.transform.position, rightUp.transform.position);
        float distance_left_up_down = Vector3.Distance(leftDown.transform.position, leftUp.transform.position);
        float distance_right_up_down = Vector3.Distance(rightUp.transform.position, rightDown.transform.position);

        float max_x = Mathf.Max(distance_left_right_down, distance_left_right_up);
        float max_y = Mathf.Max(distance_left_up_down, distance_right_up_down);

        float planeArea = arPlane.size.x * arPlane.size.y;
        //areaText.text = "Area:"+ planeArea.ToString()+", X: "+arPlane.size.x.ToString()+", Y: " + arPlane.size.y.ToString();
        //areaText.text = "max_x: "+max_x.ToString()+" max_y: "+max_y.ToString();
        if (planeArea >= 1.1 && arPlane.size.x >= 1.2 && arPlane.size.y >= 1 && arPlane.size.x >= max_x && arPlane.size.y >= max_y )//&& arPlane.size.x >= collider_size.x && arPlane.size.y >= collider_size.z
        {
            UICOntroller.Instance.scanDesk.SetActive(false);
            // desk_plane_ID = arPlane.trackableId;
            planeManager.enabled = false;
            // areaText.text = "DESK PLANE FOUND";
            /* foreach (var plane in planeManager.trackables)
             {
                 if (plane.trackableId != arPlane.trackableId)
                     plane.gameObject.SetActive(false);
             }*/
            toRescan = false;
            ObjectsSpawner.Instance.SpawnPrefabs();
        }
            

    }

   public void restartGame()
    {
        //SceneManager.LoadScene("Symmetry_Game");
        var xrManagerSettings = XRGeneralSettings.Instance.Manager;
        xrManagerSettings.DeinitializeLoader();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload current scene
        xrManagerSettings.InitializeLoaderSync();
    }

    

}
