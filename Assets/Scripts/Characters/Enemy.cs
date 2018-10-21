using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private float _speed = 5;

	public Transform Target
	{
		get { return _target; }
		set { _target = value; }
	}

	public float Speed => _speed;
	
	// Update is called once per frame
	private void Update ()
	{
		var target = Target;
		if (target == null)
		{
			return;
		}

		MoveTowardsTarget(transform.position, target.position, Speed * Time.deltaTime);
	}

	private void MoveTowardsTarget(Vector3 current, Vector3 target, float maxDistanceDelta)
	{
		transform.position = Vector3.MoveTowards(current, target, maxDistanceDelta);
	}
}
