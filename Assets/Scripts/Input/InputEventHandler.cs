using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Essential.Core.UserInput
{
	public class InputEventHandler : MonoBehaviour
	{
		[Serializable]
		private struct KeyboardButton
		{
			public string name;
			public KeyCode keycode;
			public UnityEvent KeyDown;
			public UnityEvent KeyUp;
			public UnityEvent KeyPressed;
		}

		[SerializeField] private KeyboardButton[] _keyboardButtons;

		private void Start()
		{
			if (_keyboardButtons.Length == 0)
			{
				throw new ArgumentException("Found empty array");
			}

			if (_keyboardButtons.Any(element => element.keycode == KeyCode.None))
			{
				throw new ArgumentException("Keycode 'None' was found in array");
			}
		}
		
		private void Update ()
		{
			foreach(var button in _keyboardButtons)
			{
				if(Input.GetKeyDown(button.keycode))
				{
					Debug.Log("KeyDown: " + button.KeyDown + " " + button.name);
					button.KeyDown.Invoke();
				}
				if (Input.GetKeyUp(button.keycode))
				{
					Debug.Log("KeyUp: " + button.KeyUp + " " + button.name);
					button.KeyUp.Invoke();
				}
				if (Input.GetKey(button.keycode))
				{
					Debug.Log("KeyPressed: " + button.KeyPressed + " " + button.name);
					button.KeyPressed.Invoke();
				}
			}
		}
	}
}