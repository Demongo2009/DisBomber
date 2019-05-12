using System;
using UnityEngine;


    public class NormalBombManager: MonoBehaviour, IBombManager
    {
        public delegate void ExplodeAction(GameObject explodingObject); 
        public static event ExplodeAction OnExplosion;

        private float timeToExplode;
        private ObjectPool objectPool;
        private IInputWrapper inputWrapper;

        private float initialTimeToExplode;
        private int allTicks = 9;

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
                if (OnExplosion != null)
                {
                    OnExplosion(gameObject);
                }
                
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
            objectPool.ReturnToPool(this.gameObject);

        }
        
    }
