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
            Importance = Importance.High,
            CanShowBadge = true,
            EnableVibration = true,
            EnableLights = true,
            LockScreenVisibility = LockScreenVisibility.Public,
            Description = "Spin Ready",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void SendNotification(string title, string text, string channel, TimeSpan repeatTime, bool repeat, int id)
    {
        var fireTime = DateTime.Now.Add(repeatTime);
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = fireTime;
        notification.LargeIcon = "icon_0";
        notification.SmallIcon = "icon_1";
        if (repeat) notification.RepeatInterval = repeatTime;

        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, channel, id);
    }
}
