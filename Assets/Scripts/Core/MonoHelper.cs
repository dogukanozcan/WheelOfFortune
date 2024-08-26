using System;
using System.Collections;
using UnityEngine;

public class MonoHelper : MonoSingleton<MonoHelper>
{
    public static void Delay(Action action, float delay)
    {
        Instance.StartCoroutine(Instance.Delay_Handler(action, delay));
    }
    private IEnumerator Delay_Handler(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
