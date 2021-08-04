using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HoardingElementsSpawner : MonoBehaviour
{
    public GameObject[] officePrefabs;
    
    private List<GameObject> spawnedObjs = new List<GameObject>();
    private ARPlane plane;

    public bool isPlaced = false;
    public PolygonCollider2D planeCollider;



    private static HoardingElementsSpawner instance;

    public static HoardingElementsSpawner Instance { get { return instance; } }


    private void Awake()
    {
        plane = GetComponent<ARPlane>();

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }

    private void OnEnable()
    {
        plane.boundaryChanged += OnPlaneBoundaryChanged;
    }

    private void OnDisable()
    {
        plane.boundaryChanged -= OnPlaneBoundaryChanged;
    }

    public void OnPlaneBoundaryChanged(ARPlaneBoundaryChangedEventArgs args)
    {


        int prefabsToSpawn = officePrefabs.Length;
        prefabsToSpawn -= spawnedObjs.Count;

        if (prefabsToSpawn <= 0)
        {
            return;
        }

        GameObject go = new GameObject();
        PolygonCollider2D col = go.AddComponent<PolygonCollider2D>();

        col.points = plane.boundary.ToArray();
        planeCollider = col;

        for (int i = 0; i < prefabsToSpawn; i++)
        {
            GameObject prefab = officePrefabs[i];

            GameObject obj = Instantiate(prefab, transform.position, prefab.transform.rotation, transform);

            spawnedObjs.Add(obj);

            RandomPosInBounds(col, obj);
           // plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
                  



        }

    }

    void RandomPosInBounds(PolygonCollider2D col, GameObject obj)
    {
        
        Bounds bounds = col.bounds;
        Vector3 center = bounds.center;

        float x;
        float y;

        do
        {
            
            x = Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
            y = Random.Range(center.y - bounds.extents.y, center.y + bounds.extents.y);
            obj.transform.localPosition = new Vector3(x, 0, y);

            

        } while ( !col.OverlapPoint(new Vector2(x, y)));



    }
}
