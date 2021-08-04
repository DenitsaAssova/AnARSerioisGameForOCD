using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetPreviousActiveScene : MonoBehaviour
{
    // For sake of example, assume -1 indicates first scene
    public int prevScene = -1;


    private static GetPreviousActiveScene instance;

    public static GetPreviousActiveScene Instance { get { return instance; } }


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
        DontDestroyOnLoad(this.gameObject);
    }

   
}
