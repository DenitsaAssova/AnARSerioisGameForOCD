using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public GameObject goodCharacter;
    private Animator goodCharAnim;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 20) 
        {
            goodCharAnim = goodCharacter.GetComponent<Animator>();
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 20)
        {
            goodCharAnim.Play("Idle_Stance_01");
           
        }
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("Start_Menu");
    }
}
