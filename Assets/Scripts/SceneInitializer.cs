using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [Header("Bomb variables")]
    [SerializeField] private GameObject normalBomb;
    [SerializeField] private float timeToExplodeDiffBetweenMaxMin;
    [SerializeField] private float timeToExplodeMax;
    [SerializeField] private int optimumNormalBombAmount;
    [SerializeField] private GameObject killerBomb;
    [SerializeField] private float timeToDisappear;
    [SerializeField] private int optimumKillerBombAmount;

    [Header("Spawn variables")] 
    [SerializeField] private GameObject normalBombPool;
    [SerializeField] private GameObject killerBombPool;
    [SerializeField] private float windowXMin;
    [SerializeField] private float windowXMax;
    [SerializeField] private float windowYMin;
    [SerializeField] private float windowYMax;
    [SerializeField] private float spawningZ;
    [SerializeField] private int secondsToStartSpawning;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private float adjustmentFactor;
    [SerializeField] private float differenceFactor;
    [SerializeField] private float timeToHarderDifficulty;

    [Header("Tools")] 
    [SerializeField] private GameObject coroutineRunnerPrefab;
    [SerializeField] private GameObject gameManagerObject;
    [SerializeField] private GameObject particleManagerObject;
    [SerializeField] private GameObject hudManagerObject;
    [SerializeField] private GameObject soundEffectManagerObject;
//    [SerializeField] private GameObject tapManagerObject;


    private ICoroutineRunner coroutineRunner;
    private GameManager gameManager;

    private ParticleManager particleManager;

    private HUDManager hudManager;

    private SoundEffectManager soundEffectManager;
//    private TapManager tapManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        coroutineRunner = Instantiate(coroutineRunnerPrefab).GetComponent<ICoroutineRunner>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
        particleManager = particleManagerObject.GetComponent<ParticleManager>();
        hudManager = hudManagerObject.GetComponent<HUDManager>();
        soundEffectManager = soundEffectManagerObject.GetComponent<SoundEffectManager>();
//        tapManager = tapManagerObject.GetComponent<TapManager>();

        soundEffectManager.Initialize(coroutineRunner);
        hudManager.Initialize(coroutineRunner);
        particleManager.Initialize(coroutineRunner);
        gameManager.Initialize(coroutineRunner,hudManager);
        ObjectPool normalBombObjectPool = new ObjectPool(normalBomb, normalBombPool.transform, optimumNormalBombAmount );
        ObjectPool killerBombObjectPool = new ObjectPool(killerBomb, killerBombPool.transform, optimumKillerBombAmount );

        BombSpawner bombSpawner = new BombSpawner(windowXMin,windowXMax,windowYMin,windowYMax,spawningZ,normalBomb,
            spawnCooldown,killerBomb,coroutineRunner,timeToExplodeMax,timeToExplodeDiffBetweenMaxMin,timeToDisappear,
            normalBombObjectPool,killerBombObjectPool, adjustmentFactor,timeToHarderDifficulty, differenceFactor);
        
        coroutineRunner.StartCoroutine(WaitForStart(bombSpawner,secondsToStartSpawning));


    }
    
    IEnumerator WaitForStart(BombSpawner bombSpawner, float secondsToStart)
    {
        yield return new WaitForSeconds(secondsToStart);
        bombSpawner.StartSpawning();
        
    }
}
