using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Music")] 
    [SerializeField] private GameObject soundTrackPrefab;


    // Start is called before the first frame update
    private void Awake()
    {
        GameObject[] objectsArray = FindObjectsOfType<GameObject>();
        GameObject soundTrackObject;

        bool soundTrackFound = false;
        foreach (GameObject obj in objectsArray)
        {
            if (obj.CompareTag("SoundTrack"))
            {
                soundTrackFound = true;
            }
        }

        if (!soundTrackFound)
        {
            soundTrackObject = Instantiate(soundTrackPrefab);
            soundTrackObject.GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(soundTrackObject);
        }

    }

    private void Start()
    {
    }

    public void GotoGameRoom()
    {
        SceneManager.LoadScene("GameRoom");
    }
}
