using UnityEngine;

namespace SRS.UI.PageManagement
{
	public abstract class PageTransition : ScriptableObject
	{
		public void Transition(GameObject currentPage, GameObject nextPage)
		{
			nextPage.SetActive(true);
			Animate(currentPage, nextPage);
			currentPage.SetActive(false);
		}

		protected abstract void Animate(GameObject currentPage, GameObject nextPage);
	}
}