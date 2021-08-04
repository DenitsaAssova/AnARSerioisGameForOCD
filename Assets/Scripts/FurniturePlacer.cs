using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FurniturePlacer : MonoBehaviour
{
    public Transform placementIndicator;
    public GameObject selectionUI;

    private List<GameObject> furniture = new List<GameObject>();
    private GameObject currSelected;
    private Camera cam;


    private void Start()
    {
        
        cam = Camera.main; // Camera.main is an expensive function so we don't want to constantly call it
        selectionUI.SetActive(false);
    }

    private void Update()
    {

        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) //if we are touching an ui element
        {
            Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject != null && furniture.Contains(hit.collider.gameObject))
                {
                    if (currSelected != null && hit.collider.gameObject != currSelected)
                    {
                        Select(hit.collider.gameObject);
                    } else if(currSelected == null)
                    {
                        Select(hit.collider.gameObject);
                    }
                }
            } else
            {
                Deselect();
            }
        }

        if (currSelected != null && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
            MoveSelected();
    }

    void MoveSelected()
    {
        Vector3 curPos = cam.ScreenToViewportPoint(Input.touches[0].position);
        Vector3 lastPos = cam.ScreenToViewportPoint(Input.touches[0].position - Input.touches[0].deltaPosition);

        Vector3 touchDir = curPos - lastPos;

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        currSelected.transform.position += (camRight * touchDir.x + camForward * touchDir.y);
    }

    void Select(GameObject selected)
    {
        if(currSelected != null) { ToggleSelectionVisual(currSelected, false); }
        currSelected = selected;
        ToggleSelectionVisual(currSelected, true);
        selectionUI.SetActive(true);


    }

    void Deselect()
    {
        if (currSelected != null) { ToggleSelectionVisual(currSelected, false); }
        currSelected = null;
        selectionUI.SetActive(false);
    }

    void ToggleSelectionVisual(GameObject obj, bool toogle)
    {
        obj.transform.Find("Selected").gameObject.SetActive(toogle);
    }

    public void PlaceFurniture(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, placementIndicator.position, Quaternion.identity);
        furniture.Add(obj);

        //select the object
        Select(obj);

    }

    public void ScaleSelected(float rate)
    {
        currSelected.transform.localScale += Vector3.one * rate;
    }

    public void RotateSelected(float rate)
    {
        currSelected.transform.eulerAngles += Vector3.up * rate;
    }

    public void SetColor(Image buttonImage)
    {
        MeshRenderer[] meshRenderes = currSelected.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mr in meshRenderes)
        {
            if (mr.gameObject.name == "Selected") continue;
            mr.material.color = buttonImage.color;
        }
    }

    public void DeleteSelected()
    {
        furniture.Remove(currSelected);
        Destroy(currSelected);
        Deselect();
    }
}
