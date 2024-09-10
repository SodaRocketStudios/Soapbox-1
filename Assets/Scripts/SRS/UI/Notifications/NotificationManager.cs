using System.Collections.Generic;
using UnityEngine;

namespace SRS.UI.Notifications
{
	public class NotificationManager : MonoBehaviour
	{
		private Queue<Notification> notificationQueue;

		private bool isShowing;

		public void scheduleNotification(Notification notification)
		{
			if(isShowing)
			{
				notificationQueue.Enqueue(notification);
				return;
			}

			notification.Show();
		}
	}
}