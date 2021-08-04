using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject timer;
    public GameObject timerWaitFor;
    public float remainingTime = 6f;

    // Update is called once per frame
    private void Start()
    {
       // timer.SetActive(false);
        timerWaitFor.SetActive(false);
    }

    void Update()
    {
        if (ObjectController.Instance.turnOnTimer == true)
        {
           // timer.SetActive(true);
            timerWaitFor.SetActive(true);
            ObjectController.Instance.canMove = false;
            timerWaitFor.GetComponentInChildren<TextMeshProUGUI>().text = "Please wait for: " + Mathf.FloorToInt(remainingTime).ToString() + "s";
           // timerWaitFor.GetComponent<TextMeshProUGUI>().text = "Please wait for: ";
            //timer.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(remainingTime).ToString()+ "s";
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0)
            {
                //timer.GetComponent<TextMeshProUGUI>().text = "";
                //timerWaitFor.GetComponent<TextMeshProUGUI>().text = "";
               // timer.SetActive(false);
                timerWaitFor.SetActive(false);
                ObjectController.Instance.canMove = true;
            }
        }
    }
}
