using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private float _speed = 5;
	[SerializeField] private float _attackRange;

	public Transform Target
	{
		get { return _target; }
		set { _target = value; }
	}

	public float Speed => _speed;

	private ICharacterAnimation CharacterAnimation { get; set; }
	private float AttackRange => _attackRange;

	private void Start()
	{
		CharacterAnimation = GetComponent<ICharacterAnimation>();
	}
	
	// Update is called once per frame
	private void Update ()
	{
		var target = Target;
		if (target == null)
		{
			CharacterAnimation?.Idle();
			return;
		}

		var distance = Vector3.Distance(transform.position, target.position);
		if (TargetInRange(distance))
		{
			CharacterAnimation?.Attack();
		}
		else
		{
			MoveTowardsTarget(transform.position, target.position, Speed * Time.deltaTime);
		}
	}

	private void MoveTowardsTarget(Vector3 current, Vector3 target, float maxDistanceDelta)
	{
		transform.position = Vector3.MoveTowards(current, target, maxDistanceDelta);
		CharacterAnimation?.Move();
	}

	private bool TargetInRange(float distance)
	{
		return distance <= AttackRange;
	}

	private void OnDestroy()
	{
		CharacterAnimation?.Die();
	}

	public void Die()
	{
		Destroy(this);
	}
}
