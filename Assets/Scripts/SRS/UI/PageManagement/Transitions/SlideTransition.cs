using System.Collections;
using UnityEngine;
using SRS.Extensions.Vector;

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

			t = transform.anchoredPosition.InverseLerp(startPosition, endPosition);

            while(t <= 1)
			{
				transform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
				t += Time.deltaTime/slideTime;
				yield return null;
			}
        }
    }
}