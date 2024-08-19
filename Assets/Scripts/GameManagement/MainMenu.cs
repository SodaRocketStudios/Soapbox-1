using UnityEngine;
using SRS.Input;
using SRS.UI.PageManagement;
using System.Linq;

namespace Soap.GameManagement
{
	public class MainMenu : MonoBehaviour
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
			if(!pages.Contains(pageManager.CurrentPage))
			{
				return;
			}

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
			if(!pages.Contains(pageManager.CurrentPage))
			{
				return;
			}
			
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