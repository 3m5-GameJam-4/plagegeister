using UnityEngine;

namespace Essential.Core.Utils
{
	/// <inheritdoc />
	/// <summary>
	/// MonoBehaviour providing a method to quit the application.
	/// </summary>
	public class ApplicationQuit : MonoBehaviour {

		/// <summary>
		/// Quit application even when started in Unity Editor.
		/// </summary>
		public void Quit ()
		{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		}
	}
}