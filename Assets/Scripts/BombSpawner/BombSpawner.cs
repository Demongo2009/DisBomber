using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : IBombSpawner
{
    
    private float windowXMin;
    private float windowXMax;
    private float windowYMin;
    private float windowYMax;
    private float bombZ;
    
    private float adjustmentFactor;
    private float differenceFactor;

    private const int BombsOnSceneMaxAmount = 10;
    
    private float timeToHarderDifficulty;

    private GameObject normalBombPrefab;
    private GameObject killerBombPrefab;
    
    private float spawnBombCooldown;
    private const float LowerBoundarySpawnBombCooldown = 0.15f;
    
    private float timeToExplodeMax;
    private const float  LowerBoundaryTimeToExplodeMax = 1.5f;
    
    private float timeToExplodeDiffBetweenMaxMin;
    private const float LowerBoundaryTimeToExplodeDiffBetweenMaxMin = 1.0f;
    
    private float timeToDisappear;

    private ICoroutineRunner coroutineRunner;
    
    private GameManager gameManager;

    private ObjectPool normalBombObjectPool;
    private ObjectPool killerBombObjectPool;

    public BombSpawner(float windowXMin, float windowXMax, float windowYMin, float windowYMax, float bombZ,
        GameObject normalBombPrefab, float spawnBombCooldown, GameObject killerBombPrefab,
        ICoroutineRunner coroutineRunner, float timeToExplodeMax, float timeToExplodeDiffBetweenMaxMin, float timeToDisappear,
        GameManager gameManager, ObjectPool normalBombObjectPool, ObjectPool killerBombObjectPool, float adjustmentFactor, float timeToHarderDifficulty,
        float differenceFactor)
    {
        this.windowXMin = windowXMin;
        this.windowXMax = windowXMax;
        this.windowYMin = windowYMin;
        this.windowYMax = windowYMax;
        this.bombZ = bombZ;
        
        
        this.normalBombPrefab = normalBombPrefab;
        this.timeToExplodeMax = timeToExplodeMax;
        this.timeToExplodeDiffBetweenMaxMin = timeToExplodeDiffBetweenMaxMin;
        this.spawnBombCooldown = spawnBombCooldown;
        
        this.killerBombPrefab = killerBombPrefab;
        this.timeToDisappear = timeToDisappear;

        this.coroutineRunner = coroutineRunner;
        
        
        this.gameManager = gameManager;
        
        this.normalBombObjectPool = normalBombObjectPool;
        this.killerBombObjectPool = killerBombObjectPool;

        this.adjustmentFactor = adjustmentFactor;
        this.timeToHarderDifficulty = timeToHarderDifficulty;
        this.differenceFactor = differenceFactor;
    }

    public void StartSpawning()
    {
        coroutineRunner.StartCoroutine(StartSpawningBombs());
        coroutineRunner.StartCoroutine(AdjustTimeToExplode());
        coroutineRunner.StartCoroutine(AdjustTimeToSpawn());
    }



    IEnumerator AdjustTimeToExplode()
    {
        while (true)
        {
            timeToExplodeMax = Mathf.Max(timeToExplodeMax * adjustmentFactor, LowerBoundaryTimeToExplodeMax);
            timeToExplodeDiffBetweenMaxMin = Mathf.Max(timeToExplodeDiffBetweenMaxMin * differenceFactor, LowerBoundaryTimeToExplodeDiffBetweenMaxMin);
            yield return new WaitForSeconds(timeToHarderDifficulty);
        }
    }
    
    IEnumerator AdjustTimeToSpawn()
    {
        while (true)
        {
            spawnBombCooldown = Mathf.Max(spawnBombCooldown * adjustmentFactor, LowerBoundarySpawnBombCooldown);
//            spawnTimeKillerBomb *= adjustmentFactor;
Debug.Log(spawnBombCooldown);
            yield return new WaitForSeconds(timeToHarderDifficulty);
        }
    }

    IEnumerator StartSpawningBombs()
    {
        
        while (true)
        {
            bool isSpawningBombNormal = Random.Range(0.0f, 1.0f) > 0.1f;
            if ( isSpawningBombNormal )
            {
                SpawnBombs(normalBombPrefab,timeToExplodeDiffBetweenMaxMin,timeToExplodeMax,normalBombObjectPool);
                
            }
            else
            {
                SpawnBombs(killerBombPrefab,0,timeToDisappear,killerBombObjectPool);
                
            }


            float secondsToNextSpawn = Random.Range(LowerBoundarySpawnBombCooldown, spawnBombCooldown);
            yield return new WaitForSeconds(secondsToNextSpawn);
            
        }
        
    }

    private void SpawnBombs(GameObject bombPrefab, float timeToActionDiffMaxMin,float timeToActionMax, ObjectPool objectPool)
    {
        if ( objectPool.GetObjectCount() > BombsOnSceneMaxAmount )
        {
            return;
        }
        
        float bombRadius = bombPrefab.transform.localScale.x;

        Vector3 targetPosition = new Vector3(GetRandomX(),GetRandomY(),bombZ);
//        float timeBeforeSearch = Time.time;
//        Debug.Log(timeBeforeSearch);
        while ( Physics.OverlapSphere(targetPosition, bombRadius).Length != 0 )
        {
//            Debug.Log(Time.time);
//            if (Time.time - timeBeforeSearch > 0.01f)
//            {
//
//                return;
//            }
            
            targetPosition.x = GetRandomX();
            targetPosition.y = GetRandomY();
        }

        GameObject bomb = objectPool.GetObjectFormPool();
        bomb.transform.position = targetPosition;
        bomb.SetActive(true);
        IInputWrapper bombInput = new InputWrapperTappedOnBomb();


        bomb.GetComponent<IBombManager>().Initialize(bombInput, GetRandomTimeToAction(timeToActionDiffMaxMin,timeToActionMax),objectPool);
    
    }

    private float GetRandomTimeToAction(float timeToActionDiffMaxMin, float timeToActionMax)
    {
        float timeToActionMin = timeToActionMax - timeToActionDiffMaxMin;
        if (timeToActionMax - timeToActionDiffMaxMin < 0)
        {
            timeToActionMin = 0;
        }


        return Random.Range(timeToActionMin, timeToActionMax);
    }
    

    private float GetRandomX()
    {
        return Random.Range(windowXMin, windowXMax);
    }

    private float GetRandomY()
    {
        return Random.Range(windowYMin, windowYMax);

    }
}
