using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICOntroller : MonoBehaviour
{

    private static UICOntroller instance;

    public static UICOntroller Instance { get { return instance; } }

    public GameObject scanDesk;
    public Button rescan;

    public GameObject validRotText, validPosText, infoPanel;

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
        //rescan = GameObject.Find("RescanButton").GetComponent<Button>();
        rescan.gameObject.SetActive(false);
        validRotText.gameObject.SetActive(false);
        validPosText.gameObject.SetActive(false);
       
        //scanDesk = GameObject.Find("ScanDesk");
    }

    // Update is called once per frame
    public void InfoButtonPressed()
    {
        infoPanel.SetActive(true);
    }

    public void InfoCloseButtonPressed()
    {
        infoPanel.SetActive(false);
    }
}
