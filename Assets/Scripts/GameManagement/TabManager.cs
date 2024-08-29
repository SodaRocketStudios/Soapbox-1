using System;
using System.Linq;
using UnityEngine;
using Soap.Input;
using SRS.UI.PageManagement;


namespace Soap.GameManagement
{
	public class TabManager : MonoBehaviour
	{
		[SerializeField] private PageManager pageManager;

		[SerializeField] private Page[] pages;

		[SerializeField] private PageTransition enterLeftTransition;
		[SerializeField] private PageTransition enterRightTransition;
		[SerializeField] private PageTransition exitLeftTransition;
		[SerializeField] private PageTransition exitRightTransition;

		private void OnEnable()
		{
			InputHandler.OnTabLeftInput += OnTabLeft;
			InputHandler.OnTabRightInput += OnTabRight;
			InputHandler.OnReturnInput += OnReturn;

		}

		private void OnDisable()
		{
			InputHandler.OnTabLeftInput -= OnTabLeft;
			InputHandler.OnTabRightInput -= OnTabRight;
			InputHandler.OnReturnInput -= OnReturn;
		}

		public void OnTabLeft()
		{
			if(!pages.Contains(pageManager.CurrentPage))
			{
				return;
			}

			int pageIndex = Array.IndexOf(pages, pageManager.CurrentPage);
			pageIndex--;

			if(pageIndex < 0)
			{
				pageIndex = 0;
				return;
			}

			pageManager.CurrentPage.ExitTransition = exitRightTransition;
			Page nextPage = pages[pageIndex];
			nextPage.EntryTransition = enterLeftTransition;

			pageManager.SwapPage(nextPage);
		}

		public void OnTabRight()
		{
			if(!pages.Contains(pageManager.CurrentPage))
			{
				return;
			}
			
			int pageIndex = Array.IndexOf(pages, pageManager.CurrentPage);
			pageIndex++;

			if(pageIndex >= pages.Length)
			{
				pageIndex = pages.Length - 1;
				return;
			}

			pageManager.CurrentPage.ExitTransition = exitLeftTransition;
			Page nextPage = pages[pageIndex];
			nextPage.EntryTransition = enterRightTransition;

			pageManager.SwapPage(nextPage);
		}

		public void OnReturn()
		{
			pageManager.Back();
		}	
	}
}