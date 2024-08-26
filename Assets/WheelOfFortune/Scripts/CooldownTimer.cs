using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Globalization;
using System.Net;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Unity.VisualScripting;
using Unity.Notifications.Android;

namespace Naku.WheelOfFortune
{
    public class NetTime
    {
        public string currentLocalTime;
    }
    [System.Serializable]
    public class CooldownTimer: MonoBehaviour 
    {
        public int hours;
        public int minutes;
        public int seconds;

        private const string server = "https://timeapi.io/api/timezone/zone?timeZone=UTC";

        private DateTime m_applicationStartTime;
        private DateTime CooldownDoneTime { 

            get => DateTime.FromBinary(
                Convert.ToInt64(
                    PlayerPrefs.GetString("m_cooldownDoneTime", GetDateTime().ToBinary().ToString())
                    )
                ); 

            set => PlayerPrefs.SetString("m_cooldownDoneTime", value.ToBinary().ToString()); 
        } 

        private event Action m_cooldownAction;
        public event Action<TimeSpan> OnCooldownTick;
        public event Action OnCooldownDone;


        private async void Start()
        {
            await AssignInternetTime();
            TimerChecker().Forget();
        }
        public void ResetCooldown(Action action)
        {
            var timeSpan = new TimeSpan(hours, minutes, seconds);
            m_cooldownAction = action;
            CooldownDoneTime = GetDateTime().Add(timeSpan);
         
            NotificationCenter.Instance.SetSpinNotification(timeSpan);

            TimerChecker().Forget();
        }
         
        public void StartCooldown(Action action)
        {
            m_cooldownAction = action;
        }
        public void CooldownDone()
        {
            OnCooldownDone?.Invoke();
            m_cooldownAction?.Invoke();
        }
        //LOOP
        public async UniTaskVoid TimerChecker()
        {
            double secondsRemaining = double.MaxValue;
            while (secondsRemaining > 0) 
            {
                var currentTime = GetDateTime();
                TimeSpan timeRemaining = CooldownDoneTime - currentTime;
                secondsRemaining = (float)timeRemaining.TotalSeconds;
                OnCooldownTick?.Invoke(timeRemaining);
                if (secondsRemaining <= 0)
                {
                    CooldownDone();
                    break;
                }
                    

                await UniTask.WaitForSeconds(1);
            }
        }
        public async UniTask AssignInternetTime()
        {
            m_applicationStartTime = await GetDateFromInternet();

        }
        public DateTime GetDateTime()
        {
            return m_applicationStartTime.Add(new TimeSpan(0, 0, (int)Time.unscaledTime));
        }
       
        public async UniTask<DateTime> GetDateFromInternet()
        {
            bool isTaskCompleted = false;
            DateTime currentTime = DateTime.UtcNow;
            //Debug.Log("<color=GREEN>START</color>");
            var webRequest = UnityWebRequest.Get(server);
            webRequest.SendWebRequest().completed += (async) =>
            {
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // Parse the response to get the current time
                    string responseText = webRequest.downloadHandler.text;
                    var netTime = JsonUtility.FromJson<NetTime>(responseText);
                    currentTime = DateTime.Parse(netTime.currentLocalTime);
                }
                else
                {
                    currentTime = DateTime.UtcNow;
                }
                isTaskCompleted = true;
            };

            await UniTask.WaitUntil(() => isTaskCompleted);
            //   Debug.Log("<color=GREEN>DONE</color>");
            return currentTime;
        }

    }
}