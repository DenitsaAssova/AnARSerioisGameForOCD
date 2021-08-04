using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawRoad : MonoBehaviour
{
    public GameObject surfacePenPoint;
   
    public GameObject stroke, currentStroke;

    public TrailRenderer trailRenderer;
    public GameObject meshColliderPrefab, newMeshCollider;
    

    public GameObject panel;

    public Button stopDrawing;

    public Button startDraw;

    public Button reDraw;

    public  Button instructDraw;

    [HideInInspector]
    public Transform penPoint;

    [HideInInspector]
    public static bool drawing = false;

    [HideInInspector]
    public static bool canMoveObj = true;

    [HideInInspector]
    public static bool roadDrawn = false;


    [HideInInspector]
    public static bool collectItems = false;

    private List<GameObject> strokes = new List<GameObject>();

    private static DrawRoad instance;
    public static DrawRoad Instance { get { return instance; } }


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


    // Update is called once per frame
    void Update()
    {
        
        
            penPoint = surfacePenPoint.transform;
        if (DrawRoad.drawing)
        {
            currentStroke.transform.position = penPoint.transform.position;
            currentStroke.transform.rotation = penPoint.transform.rotation;
           // newMeshCollider.transform.rotation = currentStroke.transform.rotation;

            Mesh mesh = new Mesh();
            currentStroke.GetComponent<TrailRenderer>().BakeMesh(mesh, true);


            //MeshCollider meshCollider = this.gameObject.AddComponent<MeshCollider>();
            // meshFilter.mesh = mesh;
            newMeshCollider.GetComponent<MeshCollider>().sharedMesh = mesh;

            /* int maxSize = currentStroke.GetComponent<TrailRenderer>().positionCount;
            // Debug.Log(maxSize.ToString());
             Vector2[] trailRendererPoints = new Vector2[maxSize];


             for (int i = 0; i < maxSize; i++)
             {
                 trailRendererPoints[i] = currentStroke.GetComponent<TrailRenderer>().GetPosition(i);
             }

             newEdgeCollider.GetComponent<EdgeCollider2D>().points = trailRendererPoints;


             newEdgeCollider.transform.rotation = currentStroke.transform.rotation;
             BinPlacer.Instance.tester.GetComponentInChildren<TextMeshProUGUI>().text = newEdgeCollider.tag.ToString();
             BinPlacer.Instance.tester.GetComponentInChildren<TextMeshProUGUI>().text += newEdgeCollider.transform.rotation.ToString();
             BinPlacer.Instance.tester.GetComponentInChildren<TextMeshProUGUI>().text += " " + currentStroke.transform.rotation.ToString();*/

        }

    }

        

    public void StartStroke()
    {
        

           // GameObject currentStroke;
            drawing = true;
            currentStroke = Instantiate(stroke, penPoint.transform.position, penPoint.transform.rotation) as GameObject;
        newMeshCollider = Instantiate(meshColliderPrefab, currentStroke.transform.position, currentStroke.transform.rotation) as GameObject ;
        
        //trailRenderer = currentStroke.GetComponent<TrailRenderer>();
        //edgeCollider = currentStroke.GetComponent<EdgeCollider2D>();


        /* newEdgeCollider= Instantiate(edgeCollider, penPoint.transform.position, penPoint.transform.rotation) as GameObject;
         newEdgeCollider.transform.position = Vector3.zero;
         int maxSize = currentStroke.GetComponent<TrailRenderer>().positionCount;
         Vector2[] trailRendererPoints = new Vector2[maxSize];

         for (int i = 0; i < maxSize; i++)
         {
             trailRendererPoints[i] = currentStroke.GetComponent<TrailRenderer>().GetPosition(i);
         }

         newEdgeCollider.GetComponent<EdgeCollider2D>().points = trailRendererPoints;

         // currentStroke = Instantiate(stroke, PlacementIndicator.firstGroundTransform.position, PlacementIndicator.firstGroundTransform.rotation) as GameObject;*/
        strokes.Add(currentStroke);
        
    }

    public void EndStroke()
    {
        
            drawing = false;
       

    }

    public void ActivatePanel()
    {
        canMoveObj = false;
        panel.SetActive(true);
        startDraw.gameObject.SetActive(false);
        stopDrawing.gameObject.SetActive(true);
        instructDraw.gameObject.SetActive(true);
       // scanFloor.gameObject.SetActive(true);
       instructDraw.GetComponentInChildren<TextMeshProUGUI>().text = "Draw path to the items";
        reDraw.gameObject.SetActive(true);
    }

    public void DeactivatePanel()
    {
        canMoveObj = true;
        panel.SetActive(false);
        stopDrawing.gameObject.SetActive(false);
        reDraw.gameObject.SetActive(false);
        //scanFloor.gameObject.SetActive(true);
        instructDraw.GetComponentInChildren<TextMeshProUGUI>().text = "Collect the items";
        collectItems = true;
        roadDrawn = true;

    }

    public void Redraw()
    {
        foreach(var obj in strokes)
        {
            obj.SetActive(false);
        }

        strokes = new List<GameObject>();
    }
}
