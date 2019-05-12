using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject debrisEffect;
    
    private ICoroutineRunner coroutineRunner;
    
    public void Initialize(ICoroutineRunner coroutineRunner)
    {
        this.coroutineRunner = coroutineRunner;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        NormalBombManager.OnExplosion += MakeExplosion;
        KillerBombManager.OnDisarm += MakeExplosion;
    }

    // Update is called once per frame
    private void MakeExplosion(GameObject explodingObject)
    {

        Vector3 explodingObjectPosition = explodingObject.transform.position;
        GameObject explosionParticles = Instantiate(explosionEffect, explodingObjectPosition, Quaternion.identity);
        GameObject debrisParticles = Instantiate(debrisEffect, explodingObjectPosition, Quaternion.identity);

        explosionParticles.transform.parent = transform;
        debrisParticles.transform.parent = transform;

        coroutineRunner.StartCoroutine(DestroyParticlesAfterDuration(explosionParticles));
        coroutineRunner.StartCoroutine(DestroyParticlesAfterDuration(debrisParticles));
    }

    IEnumerator DestroyParticlesAfterDuration(GameObject particles)
    {
        float particlesDuration = particles.GetComponent<ParticleSystem>().main.duration;
        
        yield return new WaitForSeconds(particlesDuration);
        Destroy(particles);
    }

    private void OnDestroy()
    {
        NormalBombManager.OnExplosion -= MakeExplosion;
        KillerBombManager.OnDisarm -= MakeExplosion;
    }
}
