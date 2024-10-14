using UnityEngine;
using UnityEngine.EventSystems;

namespace SRS.UI
{
	public class FirstSelected : MonoBehaviour
	{
		private void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(gameObject);
		}
	}
}