
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager: MonoBehaviour
{
    public static bool isGameOver = false;
    
    [Header("Light")] 
    [SerializeField] private Light mainLight;

    [Header("Camera")] 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float decreaseFactor;
    
    
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

        coroutineRunner.StartCoroutine(ShakeScreen());
        isGameOver = true;
        hudManager.SetTimeSurvived(Time.time);

        coroutineRunner.StartCoroutine(WaitForGameOverScreen());
        
    }

    IEnumerator ShakeScreen()
    {

        Vector3 originalCameraPosition = mainCamera.transform.position;
        
        while (shakeDuration > 0)
        {
            mainCamera.transform.position = originalCameraPosition + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
            yield return new WaitForEndOfFrame();
        }

        mainCamera.transform.position = originalCameraPosition;



    }

    IEnumerator WaitForGameOverScreen()
    {
        yield return new WaitForSeconds(1.0f);
        
        mainLight.intensity = 0.25f;
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
