using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stroke : MonoBehaviour
{
    private Transform penPoint;

    public TrailRenderer trailRenderer;
    public EdgeCollider2D edgeCollider;


    // Start is called before the first frame update
    void Start()
    {
        penPoint = GameObject.FindObjectOfType<DrawRoad>().penPoint;
       // edgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //penPoint = GameObject.FindObjectOfType<DrawRoad>().penPoint;
        if (DrawRoad.drawing)
        {
            this.transform.position = penPoint.transform.position;
            this.transform.rotation = penPoint.transform.rotation;

           /* int maxSize =this.GetComponent<TrailRenderer>().positionCount;
            Vector2[] trailRendererPoints = new Vector2[maxSize];

            for (int i = 0; i < maxSize; i++)
            {
                trailRendererPoints[i] = GetComponent<TrailRenderer>().GetPosition(i);
            }

            edgeCollider.points = trailRendererPoints;

    */

            //this.transform.position = new Vector3(penPoint.transform.position.x ,PlacementIndicator.firstGroundTransform.y, penPoint.transform.position.z);
            //this.transform.rotation = PlacementIndicator.firstGroundTransform.rotation;


        }
        else
        {
            this.enabled = false;
        }

    }
}
