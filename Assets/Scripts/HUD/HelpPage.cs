using UnityEngine;
using TMPro;
using Soap.Input;

namespace Soap.HUD
{
	public class HelpPage : MonoBehaviour
	{
		[SerializeField, TextArea(4, 20)] string text;

		[SerializeField] private TMP_SpriteAsset GamepadSpriteAsset;
		[SerializeField] private TMP_SpriteAsset KeyboardSpriteAsset;

		[SerializeField] private Sprite[] xBoxSprites;
		[SerializeField] private Sprite[] psSprites;
		[SerializeField] private Sprite[] kbSprites;


		private Sprite[] activeSprites;

		private TMP_Text textContainer;

		private void Awake()
		{
			textContainer = GetComponent<TMP_Text>();
		}

		private void Start()
		{
			UpdateText();
		}

		private void OnEnable()
		{
			InputHandler.OnDeviceChange += UpdateText;
		}

		private void OnDisable()
		{
			InputHandler.OnDeviceChange -= UpdateText;
		}

		private void UpdateText()
		{
			textContainer.text = GetFormattedText();
		}

		private void UpdateText(Input.DeviceType deviceType)
		{
			textContainer.text = GetFormattedText();
		}

		private string GetFormattedText()
		{
			string formattedText = "";
			char[] separators = {'<', '>'};

			switch(InputHandler.deviceType)
			{
				case Input.DeviceType.Xbox:
					activeSprites = xBoxSprites;
					textContainer.spriteAsset = GamepadSpriteAsset;
					break;
				case Input.DeviceType.Playstation:
					activeSprites = psSprites;
					textContainer.spriteAsset = GamepadSpriteAsset;
					break;
				case Input.DeviceType.Keyboard:
					activeSprites = kbSprites;
					textContainer.spriteAsset = KeyboardSpriteAsset;
					break;
				default:
					break;
			}

			Debug.Log(InputHandler.deviceType);

			foreach(string temp in text.Split(separators))
			{
				if(int.TryParse(temp, out int index))
				{
					formattedText += $"<sprite name=\"{activeSprites[index].name}\">";
					continue;
				}

				formattedText += temp;
			}

			return formattedText;
		}
	}
}