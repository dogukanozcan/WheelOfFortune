using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationMaster : MonoSingleton<NotificationMaster>
{
    [SerializeField] private AndroidNotificationManager m_androidNotification;
    private const string m_spinReadyChannelName = "spin_ready";

    private void Start()
    {
        m_androidNotification.RequestPermission();
        m_androidNotification.RegisterNotificationChannel();
       

        //OnNoticationClick CallBack
        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
          delegate (AndroidNotificationIntentData data)
          {
              if (data.Channel == m_spinReadyChannelName)
              {
                  PageController.Instance.ChangePage(PageController.Instance.GetPageByName(nameof(Page.WheelPage)));
              }
          };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus) 
        {
            //OnNoticationClick
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
            if (notificationIntentData != null)
            {
                if (notificationIntentData.Channel == m_spinReadyChannelName)
                {
                    PageController.Instance.ChangePage(PageController.Instance.GetPageByName(nameof(Page.WheelPage)));
                }
            }


        }
    }

    public void SetSpinNotification(TimeSpan repeatTime)
    {
        AndroidNotificationCenter.CancelNotification(101);
        AndroidNotificationCenter.CancelScheduledNotification(101);
        //FirstNotification is Exact Time notification, one time proc
        m_androidNotification.SendNotification("Spin Ready!", "Your spins are ready, come and win your prize!", m_spinReadyChannelName, repeatTime, false,100);
        //SeconNotification for Repeating notification
        m_androidNotification.SendNotification("Spin Ready!", "Your spins are ready, come and win your prize!", m_spinReadyChannelName, repeatTime+repeatTime, true,101);


       
    }
}
