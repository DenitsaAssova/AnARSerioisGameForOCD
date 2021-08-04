using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDrag : MonoBehaviour, IDragHandler
{
   

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = Input.touches[0].position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
