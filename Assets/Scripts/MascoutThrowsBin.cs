using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MascoutThrowsBin : MonoBehaviour
{
    public GameObject mascoutPrefab;
    private GameObject mascout;
    public float speed = 1f;
    private Animator anim;
  //  private Camera cam;
    // Start is called before the first frame update
    void Start()
    {

        //  cam = Camera.main;
        //Vector3 mascoutPos = new Vector3(BinPlacer.Instance.dumpster.transform.position.x, BinPlacer.Instance.dumpster.transform.position.y, BinPlacer.Instance.dumpster.transform.position.z);
        if (BinPlacer.Instance.disposedItemsNum == 7)
        {
            mascout = Instantiate(mascoutPrefab, Vector3.zero, Quaternion.identity);

            anim = mascout.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BinPlacer.Instance.disposedItemsNum == 7)
        {
            float step = speed * Time.deltaTime;
            mascout.transform.position = Vector3.MoveTowards(mascout.transform.position, BinPlacer.Instance.dumpster.transform.position, step);
            anim.Play("Walk");
            if (Vector3.Distance(mascout.transform.position, BinPlacer.Instance.dumpster.transform.position) < 0.001f)
            {

            }
        }
    }
}
