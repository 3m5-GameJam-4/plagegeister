using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneCounter : MonoBehaviour
{
	[SerializeField] private int _targetCount = 2;
	[SerializeField] private UnityEvent _onCountReached;
	private int _internalCount;

	public void IncreaseCount(int value)
	{
		TargetCount += value;
	} 
	
	private int TargetCount
	{
		get { return _internalCount; }
		set
		{
			if (_internalCount == value)
			{
				return;
			}
			
			_internalCount = value;
			OnCountChanged();
		}
	}

	private void OnCountChanged()
	{
		if (TargetCount == _targetCount)
		{
			_onCountReached.Invoke();
		}
	}
}
