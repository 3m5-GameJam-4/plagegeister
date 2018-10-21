using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableController : MonoBehaviour {
//	private float _currentScale = InitScale;
//	private const float TargetScale = 0f;
//
//	private const float InitScale = 1f;
//	private const int FramesCount = 100;
//	private const float AnimationTimeSeconds = 2;
//	private float _deltaTime = AnimationTimeSeconds/FramesCount;
//	private float _dx = (TargetScale - InitScale)/FramesCount;
//	private bool _upScale = true;
//	private IEnumerator Breath()
//	{
//		while (true)
//		{
//			while (_upScale)
//			{
//				_currentScale += _dx;
//				if (_currentScale > TargetScale)
//				{
//					_upScale = false;
//					_currentScale = TargetScale;
//				}
//				transform.localScale = Vector3.one * _currentScale;
//				yield return new WaitForSeconds(_deltaTime);
//			}
// 
//			while (!_upScale)
//			{
//				_currentScale -= _dx;
//				if (_currentScale < InitScale)
//				{
//					_upScale = true;
//					_currentScale = InitScale;
//				}
//				transform.localScale = Vector3.one * _currentScale;
//				yield return new WaitForSeconds(_deltaTime);
//			}
//		}
//	}
    public float MaxHealth = 4;
    public float Health = 4;
    private Vector3 InitScale;
    private IAudio MyAudioInterfacee;
    
    void Start() {
        InitScale = transform.localScale;
        MyAudioInterfacee = GetComponent<IAudio>();
    }
    
    private void Update() {
        Debug.Log(transform.localScale);
        if (Health > 0) {
            Health -= Time.deltaTime;
            transform.localScale = InitScale * (Health / MaxHealth);
//			var newScale = Mathf.Lerp(1.0f, finalScale, timer);
//			transform.localScale = Vector3.one * newScale;
        }
        else {
            // play death animation if player
            // remove from scene
            // play particle effect
            
            MyAudioInterfacee?.PlayAudio("die");
            
            // destroy object after particle effect ended
            Destroy(this.gameObject);
        }
    }


    
    
    
}