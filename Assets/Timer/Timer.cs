using System;
using UnityEngine;

namespace CommonLogic
{
    public class Timer
    {
        public event Action OnEnd;

        float duration;
        float elapsedTime;

        public float Duration 
        {
            get => duration;  
            set 
            { 
                duration = value;
            } 
        }
        public float ElapsedTime => elapsedTime;
        public bool isRunning { get; private set; }

        public Timer(float duration) 
        {
            this.duration = duration;
            isRunning = true;
        }

        public Timer AddCallBack(Action fn)
        {
            this.OnEnd += fn;
            return this;
        }

        public void Update(ref float dt)
        {
            if (!isRunning) return;
            elapsedTime += dt;
            if (elapsedTime > duration) 
            { 
                OnEnd.Invoke();
                isRunning = false;
            }
        }

        public void Reset()
        {
            elapsedTime = 0;
            isRunning = true;
        }

        public void Reset(float newDuration)
        {
            Duration = newDuration;
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void Resume()
        {
            isRunning = true;
        }
    }

}
