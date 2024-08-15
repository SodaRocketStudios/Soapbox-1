using UnityEngine;

namespace SRS.UI.PageManagement
{
	public class PageManager : MonoBehaviour
	{
		private GameObject currentPage;

		public void SwapPage(PageTransition transition, GameObject nextPage)
		{
			transition.Transition(currentPage, nextPage);
			currentPage = nextPage;
		}

		public void SetCurrentPage(GameObject currentPage)
		{
			this.currentPage = currentPage;
		}
	}
}