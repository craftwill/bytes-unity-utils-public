using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes
{
    public class Animate
    {
        public Animate(float duration_, System.Action<float> stepCallback_, System.Action endCallback_, bool timeScaled_)
        {
            duration = duration_;
            stepCallback = stepCallback_;
            endCallback = endCallback_;
            timeScaled = timeScaled_;
        }
        public void Play(bool resetTimeLeft = true)
        {
            timeLeft = duration;
            playing = true;
            done = false;
        }
        public void Continue()
        {
            playing = true;
        }
        public void Pause()
        {
            playing = false;
        }
        public void Restart()
        {
            timeLeft = duration;
        }
        public void Stop(bool callEndFunction = true)
        {
            playing = false;
            done = true;
            if (callEndFunction) { endCallback?.Invoke(); }
        }
        public void TriggerStepCallback(float timeDelta)
        {
            timeLeft -= timeDelta;
            float step = Mathf.Abs(timeLeft - duration) / duration;
            step = Mathf.Lerp(0, 1, step);
            stepCallback?.Invoke(Mathf.Clamp01(step));
            if (step >= 1) { Stop(true); }
        }
        // Getters
        public float GetDuration()
        {
            return duration;
        }
        public float GetTimeLeft()
        {
            return timeLeft;
        }
        public bool GetIsPlaying()
        {
            return playing;
        }
        public bool GetIsDone()
        {
            return done;
        }
        public bool GetIsTimeScaled()
        {
            return timeScaled;
        }

        private float duration;
        private System.Action<float> stepCallback;
        private System.Action endCallback;

        private bool playing;
        private bool done;
        private float timeLeft;
        private bool timeScaled;

        /// <summary>
        /// Wait a fixed duration before calling function.
        /// </summary>
        static public Animate Delay(float duration_, System.Action endCallback_, bool timeScaled_ = false)
        {
            Animate newAnimation = new Animate(duration_, null, endCallback_, timeScaled_);
            newAnimation.Play(); AnimateManager.GetInstance().AddAnimation(newAnimation); return newAnimation;
        }

        /// <summary>
        /// Repeat execution while callback return is true.
        /// </summary>
        static public Animate Repeat(float duration_, System.Func<bool> callback_, int maxRepeat = -1, bool timeScaled_ = false)
        {
            Animate newAnimation = new Animate(duration_, null, () => {
                if (callback_?.Invoke() == true)
                {
                    if (maxRepeat == 0) { return; } else { if (maxRepeat != -1) { maxRepeat--; } }
                    Animate.Repeat(duration_, callback_, maxRepeat, timeScaled_);
                }
            }, timeScaled_);
            newAnimation.Play(); AnimateManager.GetInstance().AddAnimation(newAnimation); return newAnimation;
        }

        /// <summary>
        /// Used to receive a T value from 0.0f to 1.0f over a certain duration each frame.
        /// </summary>
        static public Animate LerpSomething(float duration_, System.Action<float> stepCallback_, System.Action endCallback_ = null, bool timeScaled_ = false)
        {
            Animate newAnimation = new Animate(duration_, stepCallback_, endCallback_, timeScaled_);
            newAnimation.Play(); AnimateManager.GetInstance().AddAnimation(newAnimation); return newAnimation;
        }

        /// <summary>
        /// Used to receive a T value from 0.0f to 1.0f over a certain duration each frame.
        /// </summary>
        static public Animate FadeCanvasGroup(CanvasGroup targetComponent, float duration_, float alphaStart = 0.0f, float alphaEnd = 1.0f, System.Action endCallback_ = null, bool timeScaled_ = false)
        {
            return Animate.LerpSomething(duration_, (step) => {
                Utils.SetOpacity(targetComponent, Mathf.Lerp(alphaStart, alphaEnd, step));
            }, endCallback_, timeScaled_);
        }

    }

    public class AnimateManager : MonoBehaviour
    {
        #region instance
        static protected AnimateManager instance;
        private void Awake() { instance = this; }
        static public AnimateManager GetInstance() { return instance; }
        #endregion
        private List<Animate> animations = new List<Animate>();
        private List<Animate> animationsToAdd = new List<Animate>();

        private void Update()
        {
            var AnimationsToRemove = new List<Animate>();

            // Add animations to Add
            while (animationsToAdd.Count > 0)
            {
                animations.Add(animationsToAdd[0]);
                animationsToAdd.RemoveAt(0);
            }

            foreach (Animate anim in animations)
            {
                if (anim.GetIsDone()) { AnimationsToRemove.Add(anim); }
                else if (anim.GetIsPlaying())
                {
                    if (anim.GetIsTimeScaled()) { if(!Mathf.Approximately(Time.timeScale, 0f)) anim.TriggerStepCallback(Time.deltaTime); }
                    else                        { anim.TriggerStepCallback(Time.unscaledDeltaTime); }
                }
            }
            // Remove unused animations
            foreach (Animate anim in AnimationsToRemove) { RemoveAnimation(anim); }
        }

        public void ClearAllAnimations() 
        {
            animations.Clear();
            animationsToAdd.Clear();
        }

        public void AddAnimation(Animate newAnimation)
        {
            animationsToAdd.Add(newAnimation);
        }

        public void RemoveAnimation(Animate removedAnimation)
        {
            animations.Remove(removedAnimation);
        }
    }
}