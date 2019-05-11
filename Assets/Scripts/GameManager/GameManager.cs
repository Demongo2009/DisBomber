
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    public static bool isGameOver = false;

    private float timeStart=0;
    private float timeSinceStart;

    [Header("TimeCounter")] 
    [SerializeField] private GameObject timeCounterWindow;
    [SerializeField] private TextMeshProUGUI timeCounterText;
    [Header("CountDown")]
    [SerializeField] private GameObject countDownWindow;
    [SerializeField] private TextMeshProUGUI startCountDownText;
    [Header("GameOver")]
    [SerializeField] private GameObject gameOverWindow;
    [SerializeField] private TextMeshProUGUI gameOverTimeCount;
    [SerializeField] private TextMeshProUGUI gameOverTimeIsHighScore;

    [Header("Light")] 
    [SerializeField] private Light light;
    
    
    private ICoroutineRunner coroutineRunner;

    
    private bool notGameOverFlag = true;
    private bool gameStarted = false;

    public void Initialize(ICoroutineRunner coroutineRunner)
    {
        this.coroutineRunner = coroutineRunner;
    }
    

    private void Start()
    {
        coroutineRunner.StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        int i = 3;
        while (true)
        {
            i--;
            
            yield return new WaitForSeconds(1);
            startCountDownText.text = i.ToString();
            if (i == 0)
            {
                gameStarted = true;
                countDownWindow.SetActive(false);
                timeStart = Time.time;
                yield break;
            }
        }
    }

    private void Update()
    {
        UpdateTimeCounter();
        if (isGameOver && notGameOverFlag)
        {
            notGameOverFlag = false;
            GameOver();
            
        }
    }

    private void UpdateTimeCounter()
    {
        if (gameStarted)
        {
            timeSinceStart = Time.time - timeStart;
            string stringTime = TimeToString(timeSinceStart);
            timeCounterText.text = "Time survived: " + stringTime;
            
        }
    }

    private string TimeToString(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        int miliseconds = Mathf.FloorToInt(((time - seconds) * 100)%100);
        string stringTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);
        return stringTime;
    }

    private void GameOver()
    {
        float timeSurvived = timeSinceStart;
        string stringTime = TimeToString(timeSurvived);

        light.intensity = 0.25f;

        timeCounterWindow.SetActive(false);
        gameOverWindow.SetActive(true);
        gameOverTimeCount.text = "Your time: " + stringTime;

        if (GotNewHighScore(timeSurvived))
        {
            gameOverTimeIsHighScore.text = "You got new high score!";
        }
        else
        {
            gameOverTimeIsHighScore.text = "No new high score ;( ";
        }
        
        

    }

    private bool GotNewHighScore(float timeSurvived)
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
        isGameOver = false;
        notGameOverFlag = true;
        SceneManager.LoadScene("MainMenu");
    }



}
