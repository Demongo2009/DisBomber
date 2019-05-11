using UnityEngine;

public class KillerBombManager: MonoBehaviour, IBombManager
{

    private float timeToDisappear;
    private ObjectPool objectPool;
    private IInputWrapper inputWrapper;
    
    private bool isTapped =false;

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
            isTapped = false;
            DisarmBomb();
        }
    }

    private void DisarmBomb()
    {
        GameManager.isGameOver = true;
        objectPool.ReturnToPool(gameObject);

    }

    public void Tapped()
    {
        isTapped = true;
    }
    
}
