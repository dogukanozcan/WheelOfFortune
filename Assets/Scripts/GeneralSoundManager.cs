using Naku.WheelOfFortune;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSoundManager : MonoSingleton<GeneralSoundManager>
{
    private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_genericClick;
    private void Awake()
    {
        m_audioSource = GetComponentInChildren<AudioSource>();
    }
    public void GenericClick()
    {
        m_audioSource.PlayOneShot(m_genericClick);
    }
}
