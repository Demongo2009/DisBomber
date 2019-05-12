
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    public static bool isGameOver = false;
    
    [Header("Light")] 
    [SerializeField] private Light light;

    private ICoroutineRunner coroutineRunner;
    private HUDManager hudManager;

    
//    private bool notGameOverFlag = true;

    public void Initialize(ICoroutineRunner coroutineRunner, HUDManager hudManager)
    {
        this.coroutineRunner = coroutineRunner;
        this.hudManager = hudManager;
    }
    

    private void Start()
    {
        NormalBombManager.OnExplosion += GameOver;
        KillerBombManager.OnDisarm += GameOver;
    }
    

    private void GameOver(GameObject caller)
    {
        
        if (isGameOver)
        {
            return;
        }
        
        isGameOver = true;
        hudManager.SetTimeSurvived(Time.time);

        coroutineRunner.StartCoroutine(WaitForGameOverScreen());
        
    }

    IEnumerator WaitForGameOverScreen()
    {
        yield return new WaitForSeconds(1.0f);
        
        light.intensity = 0.25f;
        float timeSurvived = hudManager.GetTimeSurvived();
        hudManager.ShowGameOverWindow(IsNewHighScore(timeSurvived));
    }

    
    private bool IsNewHighScore(float timeSurvived)
    {

        float highScore = PlayerPrefs.GetFloat("HighScore", 0.0f);

        if (highScore < timeSurvived)
        {
            PlayerPrefs.SetFloat("HighScore", timeSurvived);
            
            return true;
        }
        return false;
    }

    public void GotoMainMenu()
    {
        NormalBombManager.OnExplosion -= GameOver;
        KillerBombManager.OnDisarm -= GameOver;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }



}
