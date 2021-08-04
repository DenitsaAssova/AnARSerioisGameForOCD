using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeColliderTester : MonoBehaviour
{

    public GameObject trailPrefab;
    public GameObject edgeColliderPrefab;

    private GameObject newTrailPaint;
    private GameObject newEdgeCollider;
    // Start is called before the first frame update
    void Start()
    {
        //edgeColliderPrefab = this.GetComponent<TrailRenderer>();
        newEdgeCollider = Instantiate(edgeColliderPrefab, this.transform.position, this.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
        int maxSize = this.GetComponent<TrailRenderer>().positionCount;
        Debug.Log(maxSize.ToString());
        Vector2[] trailRendererPoints = new Vector2[maxSize];

        for (int i = 0; i < maxSize; i++)
        {
            trailRendererPoints[i] = this.GetComponent<TrailRenderer>().GetPosition(i);
        }

        newEdgeCollider.GetComponent<EdgeCollider2D>().points = trailRendererPoints;
        //newEdgeCollider.transform.position = this.transform.position;
        //newEdgeCollider.transform.rotation = this.transform.rotation;

    }
}
