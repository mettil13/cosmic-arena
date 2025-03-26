using System;
using UnityEngine;

namespace CommonLogic
{
    public class Timer
    {
        public event Action OnEnd;
        public event Action OnStart;

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
            if (elapsedTime == 0) OnStart?.Invoke();
            elapsedTime += dt;
            if (elapsedTime > duration) 
            { 
                OnEnd?.Invoke();
                isRunning = false;
            }
        }

        public void EndTimer() {
            OnEnd?.Invoke();
            isRunning = false;
        }

        public void Reset()
        {
            elapsedTime = 0;
            isRunning = true;
        }

        public void Reset(float newDuration)
        {
            Duration = newDuration;
            Reset();
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
