using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderForStroke : MonoBehaviour
{
   
    private TrailRenderer traiRenderer;
    public MeshCollider meshColliderPregab, newMeshCollider;
    private MeshFilter meshFilter;
   // private Mesh mesh;
   // public GameObject stroke;
    public Camera cam;
    private Vector3 curPos, lastPos;

    public GameObject obj;


    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main;
        traiRenderer = gameObject.GetComponent<TrailRenderer>();
        //meshCollider = gameObject.GetComponent<MeshCollider>();
        newMeshCollider = Instantiate(meshColliderPregab, this.transform.position, this.transform.rotation);
      
       // meshFilter = stroke.AddComponent<MeshFilter>();


    }

    // Update is called once per frame
    void Update()
    {
       /* curPos = transform.position;
        if(curPos != lastPos)
        {
            //
            if(Vector3.Distance(curPos, lastPos) >= 0.8f)
            Instantiate(obj, transform.position, transform.rotation);
        }

        lastPos = curPos;*/
        
        // gameObject.GetComponent<MeshCollider>().sharedMesh = null;
        Mesh mesh = new Mesh();
        traiRenderer.BakeMesh(mesh, cam, true);


        //MeshCollider meshCollider = this.gameObject.AddComponent<MeshCollider>();
        // meshFilter.mesh = mesh;
        newMeshCollider.sharedMesh = mesh;

    }
}
