using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SRS.UI.PageManagement
{
	public class Page : MonoBehaviour
	{

		[SerializeField] private PanelTransition entryTransition;
		public PanelTransition EntryTransition
		{
			get { return entryTransition; }

			set { entryTransition = value; }
		}

		[SerializeField] private PanelTransition exitTransition;
		public PanelTransition ExitTransition
		{
			get { return exitTransition; }
			set { exitTransition = value; }
		}
		[SerializeField] private GameObject firstSelected;

		public UnityEvent returnAction;

		public UnityEvent OnBeforeEntry;
		public UnityEvent OnBeforeExit;
		public UnityEvent OnAfterEntry;
		public UnityEvent OnAfterExit;

		private void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(firstSelected);
			OnAfterEntry?.Invoke();
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
			StopAllCoroutines();
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
			StopAllCoroutines();
			OnBeforeExit?.Invoke();

			if(exitTransition != null)
			{
				yield return StartCoroutine(exitTransition.Animate(gameObject));
			}

			OnAfterExit?.Invoke();

			gameObject.SetActive(false);
		}

		public void Return()
		{
			returnAction?.Invoke();
		}
	}
}