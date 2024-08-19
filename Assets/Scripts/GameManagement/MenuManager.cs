using UnityEngine;
using SRS.Input;
using SRS.UI.PageManagement;

namespace Soap.GameManagement
{
	public class MenuManager : MonoBehaviour
	{
		[SerializeField] private InputReader input;

		[SerializeField] private PageManager pageManager;

		[SerializeField] private Page[] pages;

		private int pageIndex;

		private void OnEnable()
		{
			input.OnTabLeftInput += OnTabLeft;
			input.OnTabRightInput += OnTabRight;
		}

		private void OnDisable()
		{
			input.OnTabLeftInput -= OnTabLeft;
			input.OnTabRightInput -= OnTabRight;
		}

		public void OnTabLeft()
		{
			pageIndex--;

			if(pageIndex < 0)
			{
				pageIndex = 0;
				return;
			}

			pageManager.SwapPage(pages[pageIndex]);
		}

		public void OnTabRight()
		{
			pageIndex++;

			if(pageIndex >= pages.Length)
			{
				pageIndex = pages.Length - 1;
				return;
			}

			pageManager.SwapPage(pages[pageIndex]);
		}
	}
}