using UnityEngine;
using TMPro;
using SRS.UI.Notifications;

namespace Soap.HUD
{
	[RequireComponent(typeof(Notification))]
	public class Penalty : MonoBehaviour
	{
		[SerializeField] private TMP_Text penaltyTimeTextBox;

		private Notification notification;

		private void Awake()
		{
			notification = GetComponent<Notification>();
			notification.OnAfterNotify += PenaltyManager.Instance.ShowNext;
		}

		public void Show(float penaltyTime)
		{
			penaltyTimeTextBox.text = penaltyTime.ToString();
			notification.Show();
		}
	}
}