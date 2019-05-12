using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [Header("Sound")] 
    [SerializeField] private GameObject explosionSoundPrefab;
    
    private ICoroutineRunner coroutineRunner;
    
    public void Initialize(ICoroutineRunner coroutineRunner)
    {
        this.coroutineRunner = coroutineRunner;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        NormalBombManager.OnExplosion += PlayExplosionSound;
        KillerBombManager.OnDisarm += PlayExplosionSound;
    }

    // Update is called once per frame
    private void PlayExplosionSound(GameObject explodingObject)
    {

        Vector3 explodingObjectPosition = explodingObject.transform.position;
        GameObject explosionSoundEffect = Instantiate(explosionSoundPrefab, explodingObjectPosition, Quaternion.identity);
        explosionSoundEffect.GetComponent<AudioSource>().Play();
        explosionSoundEffect.transform.parent = transform;

        coroutineRunner.StartCoroutine(DestroySoundObjectAfterDuration(explosionSoundEffect));
    }

    IEnumerator DestroySoundObjectAfterDuration(GameObject soundEffectObject)
    {
        float soundDuration = soundEffectObject.GetComponent<AudioSource>().clip.length;
        
        yield return new WaitForSeconds(soundDuration);
        Destroy(soundEffectObject);
    }

    private void OnDestroy()
    {
        NormalBombManager.OnExplosion -= PlayExplosionSound;
        KillerBombManager.OnDisarm -= PlayExplosionSound;
    }
}
