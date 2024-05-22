using UnityEngine;
using UnityEditor;

namespace SRS.Extensions.Curves.Editor
{
	[CustomPropertyDrawer(typeof(CurveAttribute))]
	public class AnimationCurveDrawer: PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float height = (attribute as CurveAttribute).Height;

			AnimationCurve curve = property.animationCurveValue;

			int peakIndex = 0;

			for (int i = 0; i < curve.length; i++)
			{
				if(curve[i].value > curve[peakIndex].value)
				{
					peakIndex = i;
				}
			}

			Rect range = new(0, 0, curve.keys[curve.length-1].time, curve.keys[peakIndex].value*1.1f);

			EditorGUI.indentLevel = 0;

			position.height = height*EditorGUIUtility.singleLineHeight;

			EditorGUI.CurveField(position, property, Color.green, range);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (attribute as CurveAttribute).Height*EditorGUIUtility.singleLineHeight;
		}
	}
}
