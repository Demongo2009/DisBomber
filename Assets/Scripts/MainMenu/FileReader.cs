using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class FileReader : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI highScoreText;
        
    // Start is called before the first frame update
    void Start()
    {

        float highScore = PlayerPrefs.GetFloat("HighScore", 0.0f);
        highScoreText.text = "Best time: " + TimeToString(highScore);

    }
    
    private string TimeToString(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        int miliseconds = Mathf.FloorToInt(((time - seconds) * 100)%100);
        string stringTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);
        return stringTime;
    }

    // Update is called once per frame

}
