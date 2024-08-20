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
			SwapPageWithoutTransition(initialPage);
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

		public void SwapPageWithoutTransition(Page page)
		{
			if(currentPage != null)
			{
				currentPage.ExitWithoutTransition();
			}

			currentPage = page;

			currentPage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

			currentPage.EnterWithoutTransition();
		}
		
		public void Back()
		{
			currentPage.Return();
		}
	}
}