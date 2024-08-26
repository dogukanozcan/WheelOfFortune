using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

public class AndroidNotificationManager : MonoBehaviour
{
    public void RequestPermission()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    public void RegisterNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "spin_ready",
            Name = "Spin Ready",
            Importance = Importance.Default,
            Description = "Spin Ready",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void SendNotification(string title, string text, TimeSpan repeatTime)
    {
        var fireTime = DateTime.Now.Add(repeatTime);
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.LargeIcon = "icon_0";
        notification.SmallIcon = "icon_1";
        notification.FireTime = fireTime;
        notification.ShowTimestamp = true;
        notification.RepeatInterval = repeatTime;
        Debug.Log("FireTime: " + DateTime.Now.Add(repeatTime).ToString("T"));

     
        AndroidNotificationCenter.SendNotification(notification, "spin_ready");

    }
}
