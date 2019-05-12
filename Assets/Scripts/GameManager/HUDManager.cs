
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HUDManager: MonoBehaviour
{
    [Header("TimeCounter")] 
    [SerializeField] private GameObject timeCounterWindow;
    [SerializeField] private TextMeshProUGUI timeCounterText;
    [Header("CountDown")]
    [SerializeField] private GameObject countDownWindow;
    [SerializeField] private TextMeshProUGUI countDownText;
    [Header("GameOver")]
    [SerializeField] private GameObject gameOverWindow;
    [SerializeField] private TextMeshProUGUI gameOverTimeCount;
    [SerializeField] private TextMeshProUGUI gameOverTimeIsHighScore;

    private ICoroutineRunner coroutineRunner;
    
    
    private bool isGameStarted;
    
    private float timeStartedCountingTimeSurvived=0;

    private float timeSurvived=0.0f;

    

    public void Initialize(ICoroutineRunner coroutineRunner)
    {
        this.coroutineRunner = coroutineRunner;
    }

    private void Start()
    {
        coroutineRunner.StartCoroutine(StartCountDown());
    }
    private void Update()
    {
        UpdateTimeCounter();
    }

    IEnumerator StartCountDown()
    {
        int secondsToStart = 3;
        while (true)
        {
            countDownText.text = secondsToStart.ToString();
            if (secondsToStart == 0)
            {
                isGameStarted = true;
                countDownWindow.SetActive(false);
                timeStartedCountingTimeSurvived = Time.time;
                yield break;
            }
            secondsToStart--;
            
            yield return new WaitForSeconds(1);
        }
    }

    public float GetTimeSurvived()
    {
        return timeSurvived;
    }

    public void SetTimeSurvived(float gameTime)
    {
        timeSurvived = gameTime - timeStartedCountingTimeSurvived;
    }

    public void ShowGameOverWindow(bool isNewHighscore)
    {
        timeCounterWindow.SetActive(false); 
        gameOverWindow.SetActive(true);
        
        

        coroutineRunner.StartCoroutine(CountGameOverTime());

        if (isNewHighscore)
        {
            gameOverTimeIsHighScore.text = "You got new high score!";
        }
        else
        {
            gameOverTimeIsHighScore.text = "No new high score ;( ";
        }
        
    }

    IEnumerator CountGameOverTime()
    {
        float currentTime = 0;
        while (currentTime != timeSurvived)
        {
            float differenceBetweenAdjustmentFactor = Mathf.Max(1.0f, (timeSurvived - currentTime/timeSurvived));
            currentTime += differenceBetweenAdjustmentFactor * Time.deltaTime;
            if (currentTime >= timeSurvived)
            {
                currentTime = timeSurvived;
            }
            string stringCurrentTime = TimeToString(currentTime);
            gameOverTimeCount.text = "Your time: " + stringCurrentTime;
            
            yield return new WaitForEndOfFrame();
            
            if (Input.touchCount > 0)
            {
                Touch currentTouch = Input.touches[0];
                if (currentTouch.phase == TouchPhase.Began)
                {
                    string stringTimeSurvived = TimeToString(timeSurvived);
                    gameOverTimeCount.text = "Your time: " + stringTimeSurvived;
                    break;
                }
            }
        

        }
    }


    private void UpdateTimeCounter()
    {
        timeCounterText.text = "Time survived: " + TimeToString(0);
        if (isGameStarted)
        {
            float timeSinceStart = Time.time - timeStartedCountingTimeSurvived;
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
    
}
