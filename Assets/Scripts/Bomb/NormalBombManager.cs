using System;
using UnityEngine;

namespace Bomb
{
    public class NormalBombManager: MonoBehaviour, IBombManager
    {

        private float timeToExplode;
        private ObjectPool objectPool;
        private IInputWrapper inputWrapper;

        private float initialTimeToExplode;
        private int allTicks = 9;
        private bool isTapped;

        public void Initialize(IInputWrapper inputWrapper,float timeToExplode, ObjectPool objectPool)
        {

            this.timeToExplode = timeToExplode;
            this.initialTimeToExplode = timeToExplode;
            this.objectPool = objectPool;
            this.inputWrapper = inputWrapper;

        }

        private void Update()
        {
            TryDisarmBomb();
            MakeClockTick();
            TryExplode();
        }

        private void MakeClockTick()
        {
            timeToExplode = timeToExplode - Time.deltaTime;
            int fractionOfClock;
            for (fractionOfClock = 0; fractionOfClock < allTicks; fractionOfClock++)
            {
                if (timeToExplode >= initialTimeToExplode - ((fractionOfClock * initialTimeToExplode) / allTicks))
                {
                    break;
                }
            }
            
            String firstPartPath = "Tick" + (fractionOfClock-1);
            String secondPartPath = "_8";
            String path = firstPartPath + secondPartPath;
            Material currentMaterial = Resources.Load<Material>(path);
            this.GetComponent<Renderer>().material = currentMaterial;
        }

        private void TryExplode()
        {
            if (timeToExplode <= 0)
            {
                GameManager.isGameOver= true;
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
            objectPool.ReturnToPool(this.gameObject);

        }

        public void Tapped()
        {
            isTapped = true;
        }
    }
}