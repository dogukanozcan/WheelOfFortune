using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationCenter : MonoSingleton<NotificationCenter>
{
    [SerializeField] private AndroidNotificationManager m_androidNotification;

    private void Start()
    {
        m_androidNotification.RequestPermission();
        m_androidNotification.RegisterNotificationChannel();
       

        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
          delegate (AndroidNotificationIntentData data)
          {
              if (data.Channel == "spin_ready")
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
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
            if (notificationIntentData != null)
            {
                if (notificationIntentData.Channel == "spin_ready")
                {
                    PageController.Instance.ChangePage(PageController.Instance.GetPageByName(nameof(Page.WheelPage)));
                }
            }


        }
    }

    public void SetSpinNotification(TimeSpan repeatTime)
    {
        AndroidNotificationCenter.CancelAllNotifications();
        m_androidNotification.SendNotification("Spin Ready!", "Your spins are ready, come and win your prize!", repeatTime);
    }
}
