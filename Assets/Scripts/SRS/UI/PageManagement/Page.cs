using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SRS.UI.PageManagement
{
	public class Page : MonoBehaviour
	{
		[SerializeField] private PageTransition entryTransition;
		[SerializeField] private PageTransition exitTransition;
		[SerializeField] private GameObject firstSelected;

		public IEnumerator Enter()
		{
			gameObject.SetActive(true);
			if(entryTransition != null)
			{
				yield return StartCoroutine(entryTransition.Animate(gameObject));
			}

			EventSystem.current.SetSelectedGameObject(firstSelected);
		}

		public IEnumerator Exit()
		{
			if(exitTransition != null)
			{
				yield return StartCoroutine(exitTransition.Animate(gameObject));
			}
			gameObject.SetActive(false);
		}
	}
}