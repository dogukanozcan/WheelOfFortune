using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Naku.WheelOfFortune
{
    public class WheelSpinner : MonoBehaviour
    {
        public event Action OnSpinEnd;
        public event Action OnSpinError;
        [SerializeField] private Ease ease;

        public bool CanSpin;

        private TweenerCore<Quaternion, Vector3, QuaternionOptions> m_spinTween;

        private Vector3 m_dragBeginPosition;
        private float m_dragBeginRotation;
        private Queue<float> m_angleQueue = new Queue<float>();
        private bool m_isSpinStarted = false;
        private bool m_isDragStarted = false;

       

        private void Awake()
        {
           
        }
        public void OnBeginDrag(BaseEventData baseEventData)
        {
            if (StopSpin()) { SpinError(); return; }

            m_isDragStarted = true;
            KillSpinTween();
            m_angleQueue = new Queue<float>();
            m_dragBeginRotation = transform.localRotation.eulerAngles.z; //default rot
            m_dragBeginPosition = Input.mousePosition; //default mouse pos
        }
        public void OnDrag(BaseEventData baseEventData)
        {
            if (StopSpin()) return;

            //
            if (!m_isDragStarted)
            {
                OnBeginDrag(null);
            }

            var startVector = transform.position - m_dragBeginPosition;
            var currentVector = transform.position - Input.mousePosition;
            var angleDiff = Vector3.SignedAngle(startVector, currentVector, Vector3.forward);//dragged angle


            m_angleQueue.Enqueue(angleDiff);
            if (m_angleQueue.Count > 4)
            {
                m_angleQueue.Dequeue();
            }

            transform.localRotation = Quaternion.Euler(Vector3.forward * (m_dragBeginRotation + angleDiff));
        }
        public void OnEndDrag(BaseEventData baseEventData)
        {
            if (StopSpin()) return;

            var angleList = m_angleQueue.ToList();
            if(angleList.Count < 2) return;

            float angleDiff = angleList[^1] - angleList[0];

            //A threshold to prevent inputs from remaining stationary.
            if (Mathf.Abs(angleDiff) >= 1f)
            {
                //print(angleDiff + " <color=red>|||</color> S: " + Screen.width + "," + Screen.height);
                Spin(angleDiff);
            }
        }

        public void SpinError()
        {
            if (m_isSpinStarted) return;
            transform.DOPunchScale(-Vector3.one*.2f,.1f);
            OnSpinError?.Invoke();
        }
        public void Spin_Button()
        {
            var sign = Random.Range(0, 2) == 0 ? 1 : -1;
            Spin(Random.Range(20f, 25f) * sign);
        }

        private void Spin(float spinSpeed)
        {
            if (StopSpin()) { SpinError(); return; }

            //Randomize and Increase "spinSpeed" if "spinSpeed" exceeds spinStartThreshold
            var spinStartThreshold = 15f;
            if (Mathf.Abs(spinSpeed) > spinStartThreshold)
            {
                var normalizedSignedSpeed = spinSpeed / Mathf.Abs(spinSpeed);
                spinSpeed += Random.Range(180f, 360f+180f) * normalizedSignedSpeed;
                m_isSpinStarted = true;
            }

            var baseSpinSpeed = 3f;
            var spinDuration = 2f;

            float lastRotateZ = 0f;
            lastRotateZ = transform.rotation.eulerAngles.z;
            DOTween.KillAll(true);
            m_spinTween = transform.DORotate(baseSpinSpeed * spinSpeed * Vector3.forward, spinDuration, RotateMode.WorldAxisAdd)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    KillSpinTween();

                    if (m_isSpinStarted == true)
                    {
                        OnSpinEnd?.Invoke();
                        m_isSpinStarted = false;
                    }
                });
        }

        public void KillSpinTween()
        {
            if (m_spinTween != null)
            {
                m_spinTween.Pause();
                m_spinTween.Kill();
                m_spinTween = null;
            }
        }

        public bool StopSpin() => m_isSpinStarted || !CanSpin;

    }
}