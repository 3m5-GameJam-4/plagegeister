using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Enemy : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private float _speed = 5;
	[SerializeField] private float _attackRange;
	[SerializeField] private float _maxDegrees = 10;
	[SerializeField] private float _anger = 0.5f;
	[SerializeField] private float _sightDistance = 10.0f;

	public Transform Target
	{
		get { return _target; }
		set { _target = value; }
	}

	public float Speed => _speed;
	public float Anger => _anger;
	public float SightDistance => _sightDistance;

	private ICharacterAnimation CharacterAnimation { get; set; }
	private float AttackRange => _attackRange;
	private EnemyRegistry EnemyRegistry { get; set; }

	private void Start()
	{
		CharacterAnimation = GetComponent<ICharacterAnimation>();
		EnemyRegistry = GameObject.FindGameObjectWithTag("EnemyRegister")?.GetComponent<EnemyRegistry>();
		EnemyRegistry?.RegisterEnemy(this);
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

		DrawAttackRange();
		DrawSightRange();
		
		var distance = Vector3.Distance(transform.position, target.position);
		if (TargetInRange(distance))
		{
			CharacterAnimation?.Attack();
		}
		else
		{
			RotateTowardsTarget();
			MoveTowardsTarget(transform.position, target.position, Speed * Time.deltaTime);
		}
	}

	private void MoveTowardsTarget(Vector3 current, Vector3 target, float maxDistanceDelta)
	{
		transform.position = Vector3.MoveTowards(current, target, maxDistanceDelta);
		CharacterAnimation?.Move();
	}

	private void RotateTowardsTarget()
	{
		var targetDir = Target.position - transform.position;
		var step = Speed * Time.deltaTime;
		var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
		
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	private bool TargetInRange(float distance)
	{
		return distance <= AttackRange;
	}

	private void OnDestroy()
	{
		EnemyRegistry?.UnRegisterEnemy(this);
		CharacterAnimation?.Die();
	}

	public void Die()
	{
		Destroy(this);
	}

	[Conditional("UNITY_EDITOR")]
	private void DrawAttackRange()
	{
		var step = Speed * Time.deltaTime;
		var targetDir = Target.position - transform.position;
		var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
		Debug.DrawRay(transform.position + new Vector3(0,1,0), newDir * _attackRange, Color.red);
	}
	
	[Conditional("UNITY_EDITOR")]
	private void DrawSightRange()
	{
		var step = Speed * Time.deltaTime;
		var targetDir = Target.position - transform.position;
		var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
		Debug.DrawRay(transform.position + new Vector3(0,2,0), newDir * _sightDistance, Color.green);
	}
}
