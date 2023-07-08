using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour
{

    public GameObject player;
    public GameObject joystick;
    public Canvas menu;
    public Image menuPanel;
    public Canvas hud;
    public Image confirmPanel;


    //OPTIONS
    public Text fpsCounterText;
    public Button fpsEnabler;
    public Text currentRes; //RESOLUTION
    public Slider grenadeThrowDistance;
    public Text grenadeValue;
    public Slider sensibilitySlider;
    public Text sensibilityText;
    public GameObject controlModifierPrefab;
    public Button exitControlModifier;

    GameObject controlModifier;

    bool fpsEnabled = false;
    float timeCounter = 0f;
    float frameCounter = 0f;






    /*    public GameObject player;
        public Transform enemyParent;
        public Transform bulletParent;
        public Transform bossProjectileParent;
        public GameObject boss;
    */


    private void Awake()
    {
        Debug.Log(sensibilitySlider.value);
        //SI RISOLVE SPOSTANDO DA START A AWAKE??

        OptionsSave optionsData = SaveSystem.LoadOptions();
        grenadeThrowDistance.value = optionsData.GrenadeThrowDistance;
        grenadeValue.text = optionsData.GrenadeThrowDistance.ToString();

        sensibilitySlider.value = optionsData.sensibility;
        sensibilityText.text = optionsData.sensibility.ToString();


        //SAVE SYSTEM DEFAULT

        if (!File.Exists(Application.persistentDataPath + "/options.zs"))
            SaveSystem.SaveOptions(8, 1280, 720, 50, 0.5f);


    }



    void Start()
    {
        Debug.Log(sensibilitySlider.value);
        menu.gameObject.SetActive(false);


        fpsEnabled = false;
        fpsCounterText.gameObject.SetActive(false);

        currentRes.text = "RESOLUTION " + Screen.currentResolution.width.ToString() + "x" + Screen.currentResolution.height.ToString();
    }



    private void Update()
    {
        Debug.Log(sensibilitySlider.value);
        //FRAMERATE COUNTER

        if (fpsEnabled)
        {
            frameCounter += 1f;
            timeCounter += Time.deltaTime;

            if(timeCounter >= 1f)
            {
                fpsCounterText.text = ((int)(frameCounter / timeCounter)).ToString() + " FPS";
                frameCounter = 0f;
                timeCounter = 0f;
            }
        }


        //GRENADE THROW DISTANCE

        int tmpGrenadeDistance = (int)grenadeThrowDistance.value;
        grenadeValue.text = tmpGrenadeDistance.ToString();
        player.GetComponent<PlayerMovement>().ChangeGrenadeThrowDistance(tmpGrenadeDistance);
        SaveSystem.SaveOptions(tmpGrenadeDistance, 0, 0, -1, -1);


        //JOYSTICK SENSIBILITY

        int tmpSensibility = (int)sensibilitySlider.value * 720 / Camera.main.scaledPixelHeight;
        sensibilityText.text = tmpSensibility.ToString();
        joystick.GetComponent<MyJoystick>().SetSensibility(tmpSensibility);
        SaveSystem.SaveOptions(0, 0, 0, tmpSensibility, -1);

    }







    public void OpenMenu()
    {
        menu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        hud.gameObject.SetActive(false);
        confirmPanel.gameObject.SetActive(false);
    }



    #region RETURN TO MAIN MENU


    public void ResumeButton()
    {
        Time.timeScale = 1f;
        menu.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);
    }



    public void ReturnToMenuBUtton()
    {
        confirmPanel.gameObject.SetActive(true);
    }


    public void YesButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }


    public void NoButton()
    {
        confirmPanel.gameObject.SetActive(false);
    }


    #endregion





    #region OPTIONS



    public void FPS_Enabler()
    {
        fpsCounterText.gameObject.SetActive(true);
        fpsEnabled = true;
    }
    public void FPS_Disabler()
    {
        fpsCounterText.gameObject.SetActive(false);
        fpsEnabled = false;
    }

    public void Resolution720p()
    {
        joystick.GetComponent<MyJoystick>().SetWithResolution(720, Screen.currentResolution.height);
        Screen.SetResolution(1280, 720, true, 30);
        currentRes.text = "RESOLUTION 1280x720";
        SaveSystem.SaveOptions(0, 1280, 720, -1, -1);
    }
    public void Resolution450p()
    {
        joystick.GetComponent<MyJoystick>().SetWithResolution(450, Screen.currentResolution.height);
        Screen.SetResolution(800, 450, true, 30);
        currentRes.text = "RESOLUTION 800x450";
        SaveSystem.SaveOptions(0, 800, 450, -1, -1);
    }
    public void Resolution360p()
    {
        joystick.GetComponent<MyJoystick>().SetWithResolution(360, Screen.currentResolution.height);
        Screen.SetResolution(640, 360, true, 30);
        currentRes.text = "RESOLUTION 640x360";
        SaveSystem.SaveOptions(0, 640, 360, -1, -1);
    }


    public void ModifyControls()
    {
        controlModifier = Instantiate(controlModifierPrefab, new Vector2(player.transform.position.x + 8.65f, 5f), Quaternion.identity);
        DragHandler tmpDragger = controlModifier.GetComponent<DragHandler>();
        tmpDragger.player = player;
        tmpDragger.menuPanel = menuPanel;
        tmpDragger.exitControlModifier = exitControlModifier;
        controlModifier.SetActive(true);
        exitControlModifier.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(false);
    }

    public void ExitControlModifier()
    {
        menuPanel.gameObject.SetActive(true);
        exitControlModifier.gameObject.SetActive(false);
        Destroy(controlModifier.gameObject);
    }


    #endregion


    /*
    void PauseFunction()
    {
        bool newState = !player.GetComponent<PlayerMovement>().enabled;
        player.GetComponent<PlayerMovement>().enabled = newState;
        player.GetComponent<Weapon>().enabled = newState;
        //if(boss != null)
            
    }
    */


}
