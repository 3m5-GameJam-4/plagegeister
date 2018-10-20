using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
	[SerializeField] private UnityEvent _onLoadFinished;

	private List<AsyncOperation> _asyncOperations = new List<AsyncOperation>();
	
	public void LoadSceneByIndex(int sceneBuildIndex)
	{
		StartCoroutine(LoadSceneByIndexAsync(sceneBuildIndex, LoadSceneMode.Single));
	}

	public void LoadSceneByName(string sceneName)
	{
		StartCoroutine(LoadSceneByNameAsync(sceneName, LoadSceneMode.Single));
	}
	
	public void LoadSceneAdditiveByIndex(int sceneBuildIndex)
	{
		StartCoroutine(LoadSceneByIndexAsync(sceneBuildIndex, LoadSceneMode.Additive));
	}

	public void LoadSceneAdditiveByName(string sceneName)
	{
		StartCoroutine(LoadSceneByNameAsync(sceneName, LoadSceneMode.Additive));
	}
	
	private IEnumerator LoadSceneByIndexAsync(int sceneBuildIndex, LoadSceneMode loadSceneMode)
	{
		var asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneMode);
		asyncLoad.allowSceneActivation = false;
		/*while (!asyncLoad.isDone)
		{
			yield return null;
		}*/
		
		_asyncOperations.Add(asyncLoad);
		_onLoadFinished.Invoke();
		yield return null;
	}
	
	private IEnumerator LoadSceneByNameAsync(string sceneName, LoadSceneMode loadSceneMode)
	{
		var asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
		asyncLoad.allowSceneActivation = false;
		/*while (!asyncLoad.isDone)
		{
			yield return null;
		}*/
		
		_asyncOperations.Add(asyncLoad);
		_onLoadFinished.Invoke();
		yield return null;
	}


	public void ActivateScenes()
	{
		foreach (var asyncOperation in _asyncOperations)
		{
			asyncOperation.allowSceneActivation = true;
		}
		_asyncOperations.Clear();
	}
}
