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
			Vector2 initialPosition = transform.anchoredPosition;

			t = initialPosition.InverseLerp(startPosition, endPosition);

            while( t <= 1)
			{
				transform.anchoredPosition = Vector2.Lerp(initialPosition, endPosition, t/slideTime);
				t += Time.deltaTime/slideTime;
				yield return null;
			}
        }
    }
}