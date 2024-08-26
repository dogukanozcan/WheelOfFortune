using Naku.WheelOfFortune;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CooldownDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_cooldownLabel;

    private CooldownTimer m_timer;
    private void Awake()
    {
        m_timer = GetComponent<CooldownTimer>();
    }
    private void OnEnable()
    {
        m_timer.OnCooldownDone += SetReady;
        m_timer.OnCooldownTick += UpdateCooldown;
    }
    public void UpdateCooldown(TimeSpan remainingTime)
    {
        m_cooldownLabel.text = "NEXT SPIN"+"\n\r"+remainingTime.ToString("hh':'mm':'ss");
    }
    public void SetReady()
    {
        m_cooldownLabel.text = "SPIN READY!";
    }
}
