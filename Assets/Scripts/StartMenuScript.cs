using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour
{
    public GameObject goodCharacter, badCharacter, character;

    private Animator goodChAnim, badChAnim, characterAnim;

    public GameObject textInMenu;
    public GameObject tester;

    private static StartMenuScript instance;

    public static StartMenuScript Instance { get { return instance; } }


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

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 21) //start menu with two characters
        {
            goodChAnim = goodCharacter.GetComponent<Animator>();
            badChAnim = badCharacter.GetComponent<Animator>();
        } else if (SceneManager.GetActiveScene().buildIndex == 1 ||
            SceneManager.GetActiveScene().buildIndex == 3||
            SceneManager.GetActiveScene().buildIndex == 4 ||
            SceneManager.GetActiveScene().buildIndex == 6 ||
             SceneManager.GetActiveScene().buildIndex == 18
            ) //game rules menus and game over menus
        {
            characterAnim = character.GetComponent<Animator>();
        }

        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
           
            textInMenu.GetComponentInChildren<TextMeshProUGUI>().text += Mathf.FloorToInt(OnRoadChecker.timeOnROad).ToString() + " seconds away from the road!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 21)
        {
            goodChAnim.Play("Idle_Stance_01");
            badChAnim.Play("Idle_Stance_01");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 ||
            SceneManager.GetActiveScene().buildIndex == 3 ||
            SceneManager.GetActiveScene().buildIndex == 4 ||
            SceneManager.GetActiveScene().buildIndex == 6 ||
             SceneManager.GetActiveScene().buildIndex == 18
            )
        {
            characterAnim.Play("Idle_Stance_01");

        }
       
    }

   public void PressStartGameButton()
    {
       
        SceneManager.LoadScene("Symmetry_Game_Rules");
    }

    public void PressContunieButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene("Symmetry_Game");

        else if( SceneManager.GetActiveScene().buildIndex == 3) {
            SceneManager.LoadScene("HoardingGameRules");
        } else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene("Hoarding_Game");
        }
     else if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            SceneManager.LoadScene("Cleaning_Game_Rules");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 18)
        {
            SceneManager.LoadScene("Cleaning_Game");
        } else if (SceneManager.GetActiveScene().buildIndex == 21)
        {
            SceneManager.LoadScene(GetPreviousActiveScene.Instance.prevScene);
        }
            //go to next game rule menu
    }

    public void PressBackButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 4 || SceneManager.GetActiveScene().buildIndex == 18)
        {
            GetPreviousActiveScene.Instance.prevScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("Start_Menu_Continue");
        } else
        SceneManager.LoadScene("Start_Menu");
    }
    public void PressLexiconButton()
    {

        SceneManager.LoadScene("Lexicon");

    }

    public void PressOCDButton()
    {

        SceneManager.LoadScene("OCD_Lexicon");

    }

    public void PressSubtypeButton()
    {

        SceneManager.LoadScene("Subtype_Lexicon");

    }
    public void PressTreatmentButton()
    {

        SceneManager.LoadScene("Treatment_Lexicon");

    }

    public void PressBackButtonLexicon()
    {
        SceneManager.LoadScene("Lexicon");
    }

    public void PressOCDForwardButton()
    {

        SceneManager.LoadScene("OCD_Lexicon_Forward");

    }
    

    public void PressOCDForwardBackButton()
    {

        SceneManager.LoadScene("OCD_Lexicon");

    }

    public void PressSubtypeBackButton()
    {

        SceneManager.LoadScene("Subtype_Lexicon");

    }

    public void PressSubtypeContamination()
    {

        SceneManager.LoadScene("Contamination");

    }

    public void PressSubtypeChecking()
    {

        SceneManager.LoadScene("Checking");

    }

    public void PressSubtypeOrdering()
    {

        SceneManager.LoadScene("Ordering");

    }

    public void PressSubtypeNoCompulsions()
    {

        SceneManager.LoadScene("NoCompulsions");

    }

    public void PressSubtypeHoarding()
    {

        SceneManager.LoadScene("Hoarding");

    }

    public void PressCBTBackButton()
    {

        SceneManager.LoadScene("Treatment_Lexicon");

    }

    public void PressCBTButton()
    {

        SceneManager.LoadScene("CBT");

    }
}
