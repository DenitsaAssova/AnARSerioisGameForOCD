using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackteriaController : MonoBehaviour
{

    public float speed = 1f;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
        {
           
            Vector3 pos = cam.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, cam.nearClipPlane));

            RaycastHit hit;
           
            if (Physics.Raycast(pos, cam.transform.forward, out hit, Mathf.Infinity))
            {
                if(hit.collider.tag == "ARPLane")
                {
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, hit.point, step);
                }
            }

           
        }
    }
}
