using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Text highScore;


    void Awake()
    {
        if (SceneManager.GetSceneByName("GameScene").name == "GameScene")
            SceneManager.UnloadScene("GameScene");
    }

    private void Start()
    {
        highScore.text = SaveSystem.LoadScore().highScore.ToString() + " pt";
    }


    public void StartButton()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }


    public void ExitButton()
    {
        Application.Quit();
    }




    public void ResetRecord()
    {
        SaveSystem.SaveScore(0);
        highScore.text = SaveSystem.LoadScore().highScore.ToString() + " pt";
    }


}
