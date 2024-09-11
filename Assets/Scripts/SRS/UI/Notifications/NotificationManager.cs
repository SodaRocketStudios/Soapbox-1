using System.Collections.Generic;
using UnityEngine;

namespace SRS.UI.Notifications
{
	public class NotificationManager : MonoBehaviour
	{
		public static NotificationManager Instance;

		private Queue<Notification> notificationQueue;

		private bool isShowing;

		private void Awake()
		{
			if(Instance == null)
			{
				Instance = this;
			}
			else if(Instance != this)
			{
				Destroy(gameObject);
			}
		}

		public void ShowNotification(Notification notification)
		{
			if(isShowing)
			{
				ScheduleNotification(notification);
				return;
			}

			notification.Show();
		}

		public void ShowNext()
		{
			if(notificationQueue.Count > 0)
			{
				notificationQueue.Dequeue().Show();
			}
			else
			{
				isShowing = false;
			}
		}

		private void ScheduleNotification(Notification notification)
		{
			notificationQueue.Enqueue(notification);
		}
	}
}