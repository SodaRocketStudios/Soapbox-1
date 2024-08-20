using UnityEngine;

namespace SRS.UI.PageManagement
{
	public class PageManager : MonoBehaviour
	{
		[SerializeField] private Page initialPage;

		private Page currentPage;

		public Page CurrentPage
		{
			get { return currentPage; }
		}

		private void Start()
		{
			SetPageWithoutTransition(initialPage);
		}

		public void SwapPage(Page nextPage)
		{
			if(currentPage != null)
			{
				StartCoroutine(currentPage.Exit());
			}

			currentPage = nextPage;

			StartCoroutine(currentPage.Enter());
		}

		public void SetPageWithoutTransition(Page page)
		{
			if(currentPage != null)
			{
				currentPage.ExitWithoutTransition();
			}
			currentPage = page;
			currentPage.EnterWithoutTransition();
		}
	}
}