using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SRS.UI.PageManagement
{
	public class Page : MonoBehaviour
	{
		[SerializeField] private PageTransition entryTransition;
		public PageTransition EntryTransition
		{
			get { return entryTransition; }

			set { entryTransition = value; }
		}

		[SerializeField] private PageTransition exitTransition;
		public PageTransition ExitTransition
		{
			get { return exitTransition; }
			set { exitTransition = value; }
		}
		[SerializeField] private GameObject firstSelected;

		private void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(firstSelected);
		}

		public IEnumerator Enter()
		{
			gameObject.SetActive(true);
			if(entryTransition != null)
			{
				yield return StartCoroutine(entryTransition.Animate(gameObject));
			}
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