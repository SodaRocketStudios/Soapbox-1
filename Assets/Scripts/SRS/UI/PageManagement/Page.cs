using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SRS.UI.PageManagement
{
	public class Page : MonoBehaviour
	{
		public UnityEvent OnBeforeEntry;
		public UnityEvent OnBeforeExit;
		public UnityEvent OnAfterEntry;
		public UnityEvent OnAfterExit;

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

		public void EnterWithoutTransition()
		{
			OnBeforeEntry?.Invoke();
			gameObject.SetActive(true);
			OnAfterEntry?.Invoke();
		}

		public void ExitWithoutTransition()
		{
			OnBeforeExit?.Invoke();
			gameObject.SetActive(false);
			OnAfterExit?.Invoke();
		}

		public IEnumerator Enter()
		{
			gameObject.SetActive(true);

			OnBeforeEntry?.Invoke();

			if(entryTransition != null)
			{
				yield return StartCoroutine(entryTransition.Animate(gameObject));
			}

			OnAfterEntry?.Invoke();
		}

		public IEnumerator Exit()
		{
			OnBeforeExit?.Invoke();

			if(exitTransition != null)
			{
				yield return StartCoroutine(exitTransition.Animate(gameObject));
			}

			OnAfterExit?.Invoke();

			gameObject.SetActive(false);
		}
	}
}