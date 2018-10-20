using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
	public void LoadSceneByIndex(int sceneBuildIndex)
	{
		StartCoroutine(LoadSceneByIndexAsync(sceneBuildIndex));
	}

	public void LoadSceneByName(string sceneName)
	{
		StartCoroutine(LoadSceneByNameAsync(sceneName));
	}
	
	private static IEnumerator LoadSceneByIndexAsync(int sceneBuildIndex)
	{
		var asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
	
	private static IEnumerator LoadSceneByNameAsync(string sceneName)
	{
		var asyncLoad = SceneManager.LoadSceneAsync(sceneName);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}
