using UnityEngine;

namespace Naku.WheelOfFortune
{
    public class WheelSounds : MonoBehaviour
    {
        private AudioSource m_audioSource;
        private WheelOfFortune m_wheelOfFortune;
        private WheelSpinner m_wheelSpinner;
        [SerializeField] private AudioClip m_tickClip;
        [SerializeField] private AudioClip m_winClip;
        [SerializeField] private AudioClip m_spinErrorClip;
        private float lastRotateZ;

        private void Awake()
        {
            m_wheelOfFortune = GetComponent<WheelOfFortune>();
            m_wheelSpinner = GetComponent<WheelSpinner>();
            m_audioSource = GetComponentInChildren<AudioSource>();
        }

        private void OnEnable()
        {
            m_wheelSpinner.OnSpinEnd += WinRewardSound;
            m_wheelSpinner.OnSpinError += SpinErrorSound;
        }
        private void OnDisable()
        {
            m_wheelSpinner.OnSpinEnd -= WinRewardSound;
            m_wheelSpinner.OnSpinError -= SpinErrorSound;
        }

        private void Tick()
        {
            m_audioSource.PlayOneShot(m_tickClip);
        }

        public void WinRewardSound()
        {
            m_audioSource.PlayOneShot(m_winClip);
        }
        public void SpinErrorSound()
        {
            m_audioSource.PlayOneShot(m_spinErrorClip);
        }

        private void Update()
        {
            if (Mathf.Abs(lastRotateZ - transform.rotation.eulerAngles.z) >= m_wheelOfFortune.PieceAngle)
            {
                lastRotateZ = transform.rotation.eulerAngles.z;
                Tick();
            }
        }
    }
}