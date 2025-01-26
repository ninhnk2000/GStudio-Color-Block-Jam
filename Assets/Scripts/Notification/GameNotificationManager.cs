using Unity.Notifications.Android;
using UnityEngine;

public class GameNotificationManager : MonoBehaviour
{
    private void Awake()
    {
        // if (Application.platform == RuntimePlatform.Android)
        // {
        //     AndroidNotificationCenter.Initialize();
        // }

#if UNITY_ANDROID
        SendNotification();
#endif
    }

    private void Register()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.Initialize();

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void SendNotification()
    {
        Register();

        var notification = new AndroidNotification();
        notification.Title = "Saferio";
        notification.Text = "Saferio - Sample Notication";
        notification.FireTime = System.DateTime.Now.AddMinutes(1);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
}
