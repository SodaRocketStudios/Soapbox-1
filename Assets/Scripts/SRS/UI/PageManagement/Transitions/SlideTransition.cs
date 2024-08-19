using System.Collections;
using UnityEngine;

namespace SRS.UI.PageManagement
{
	[CreateAssetMenu(menuName = "Page Transitions/Slide Transition")]
    public class SlideTransition : PageTransition
    {
		[SerializeField, Min(0.1f)] private float slideTime;

		[SerializeField] private Vector2 startPosition;
		[SerializeField] private Vector2 endPosition;

        public override IEnumerator Animate(GameObject page)
        {
			float t = 0;
			RectTransform transform = page.GetComponent<RectTransform>();
            while( t <= slideTime)
			{
				transform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t/slideTime);
				t += Time.deltaTime;
				yield return null;
			}
        }
    }
}