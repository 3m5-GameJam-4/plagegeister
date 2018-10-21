using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private float _speed = 5;
	[SerializeField] private float _attackRange;
	[SerializeField] private ICharacterAnimation _characterAnimation;

	public Transform Target
	{
		get { return _target; }
		set { _target = value; }
	}

	public float Speed => _speed;
	
	private ICharacterAnimation CharacterAnimation => _characterAnimation;
	private float AttackRange => _attackRange;
	
	// Update is called once per frame
	private void Update ()
	{
		var target = Target;
		if (target == null)
		{
			CharacterAnimation.Idle();
			return;
		}

		MoveTowardsTarget(transform.position, target.position, Speed * Time.deltaTime);
		var distance = Vector3.Distance(transform.position, target.position);
		TargetInRange(distance);
	}

	private void MoveTowardsTarget(Vector3 current, Vector3 target, float maxDistanceDelta)
	{
		transform.position = Vector3.MoveTowards(current, target, maxDistanceDelta);
		CharacterAnimation.Move();
	}

	private void TargetInRange(float distance)
	{
		if (distance > AttackRange)
		{
			return;
		}
		
		CharacterAnimation.Attack();
	}

	private void OnDestroy()
	{
		CharacterAnimation.Die();
	}
}
