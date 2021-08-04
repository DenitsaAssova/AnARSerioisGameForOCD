using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;

public class CleaningUiController : MonoBehaviour
{
    public GameObject infoButton, rescanButton, continueButton, placeFinger, waitButton, sniper, spray, killBacs;
    private ARPlaneManager planeManager;
    public static bool continuePressed = false;
    private float remainingTime = 10f;
    private float remTimeEndGame =2f;
    public static bool timerUp = false;
    public Camera cam;
    public ParticleSystem sprayParticle;
    private bool getSpawnedObj = false;

    private GameObject sprayedBac;

    private int bacsToKill = 5;
    // Start is called before the first frame update
    public GameObject tester;


    private void Awake()
    {
        planeManager = FindObjectOfType<ARPlaneManager>();
    }
    private void Start()
    {
        continueButton.GetComponentInChildren<Button>().interactable = false;
       
    }

    private void Update()
    {
        if(PlanePainter.bacSpawned == true)
        {
            continueButton.GetComponentInChildren<Button>().interactable = true;
        }

        if(PlanePainter.touched == true)
        {
            placeFinger.SetActive(false);
            waitButton.SetActive(true);
            remainingTime -= Time.deltaTime;
            waitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Wait " + Mathf.FloorToInt(remainingTime).ToString() + "s";
            if (remainingTime < 0)
            {
                waitButton.SetActive(false);
                timerUp = true;
            }
        }
        else if(PlanePainter.firstTouched == true && timerUp == false)
        {
            placeFinger.SetActive(true);
            waitButton.SetActive(false);
           
        } else if (timerUp)
        {
            if (getSpawnedObj == false)
            {
                bacsToKill = PlanePainter.prefabsToSpawnPublic;
                //tester.GetComponent<TextMeshProUGUI>().text = "UI: " + bacsToKill.ToString();
                getSpawnedObj = true;

            }
            placeFinger.SetActive(false);
            killBacs.SetActive(true);
            sniper.SetActive(true);
            spray.SetActive(true);
           
        }

        if (bacsToKill <= 0 )
        {

            remTimeEndGame -= Time.deltaTime;
   
            if (remTimeEndGame < 0)
            {
                killBacs.SetActive(false);
                SceneManager.LoadScene("Cleaning_GameOver");

            }
        }


    }

    public void Rescan()
    {
        var xrManagerSettings = XRGeneralSettings.Instance.Manager;
        xrManagerSettings.DeinitializeLoader();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload current scene
        xrManagerSettings.InitializeLoaderSync();
    }

    public void Continue()
    {
        continuePressed = true;
        planeManager.enabled = false;
        infoButton.SetActive(false);
        placeFinger.SetActive(true);
        rescanButton.SetActive(false);
        continueButton.SetActive(false);
    }

    public void Spray()
    {
        spray.GetComponent<Animator>().Play("Spray");
        
        sprayParticle.Play();
       // sprayParticle.GetComponent<Animator>().Play("MoveSpray");
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            if(hit.collider.tag == "Bacteria")
            {
                //sprayedBac = hit.collider.gameObject;
                Destroy(hit.collider.gameObject, 1f);
               // hit.collider.gameObject.SetActive(false);
                bacsToKill--;
              
            }
        }
    }

}
