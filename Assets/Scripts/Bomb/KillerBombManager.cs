using UnityEngine;

public class KillerBombManager: MonoBehaviour, IBombManager
{
    
    public delegate void DisarmAction(GameObject disarmedObject); 
    public static event DisarmAction OnDisarm;

    private float timeToDisappear;
    private ObjectPool objectPool;
    private IInputWrapper inputWrapper;

    public void Initialize( IInputWrapper inputWrapper, float timeToDisappear, ObjectPool objectPool)
    {

        this.timeToDisappear = timeToDisappear;
        this.objectPool = objectPool;
        this.inputWrapper = inputWrapper;
    }

    private void Update()
    {
        TryDisarmBomb();
        MakeClockTick();
        TryDisappear();
    }

    private void MakeClockTick()
    {
        timeToDisappear = timeToDisappear - Time.deltaTime;
    }

    private void TryDisappear()
    {
        if (timeToDisappear <= 0)
        {
            objectPool.ReturnToPool(gameObject);
        }
    }

    private void TryDisarmBomb()
    {
        if (inputWrapper.IsTapped(gameObject))
        {
            DisarmBomb();
        }
    }

    private void DisarmBomb()
    {
        if (OnDisarm != null && !GameManager.isGameOver)
        {
            OnDisarm(gameObject);
        }
        objectPool.ReturnToPool(gameObject);

    }


    
}
