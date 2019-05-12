using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool 
{
    private GameObject objectPrefab;

    private Transform poolTransform;

    private int optimumObjectAmount;
    
    private Queue<GameObject> objectInPoolQueue = new Queue<GameObject>();


    public ObjectPool(GameObject objectPrefab, Transform poolTransform, int optimumObjectAmount)
    {
        this.objectPrefab = objectPrefab;
        this.poolTransform = poolTransform;
        this.optimumObjectAmount = optimumObjectAmount;
    }

    private void CreatePoolObject()
    {
        GameObject createdObject = GameObject.Instantiate(objectPrefab);
        objectInPoolQueue.Enqueue(createdObject);

        createdObject.SetActive(false);

        createdObject.transform.parent = poolTransform;
    }

    public GameObject GetObjectFormPool()
    {
        if (objectInPoolQueue.Count == 0)
        {
            CreatePoolObject();
        }

        return objectInPoolQueue.Dequeue();
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        if (objectInPoolQueue.Count > optimumObjectAmount)
        {

            GameObject.Destroy(objectToReturn);
            return;
        }
        objectToReturn.SetActive(false);
        objectInPoolQueue.Enqueue(objectToReturn);
    }
    
}
