using UnityEngine;

namespace SRS.UI.PageManagement
{
	public class PageManager : MonoBehaviour
	{
		[SerializeField] private Page initialPage;

		private Page currentPage;

		public void SwapPage(Page nextPage)
		{
			if(currentPage != null)
			{
				StartCoroutine(currentPage.Exit());
			}

			currentPage = nextPage;

			StartCoroutine(currentPage.Enter());
		}
	}
}