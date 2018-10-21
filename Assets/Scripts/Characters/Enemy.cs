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
	[SerializeField] private float _attackCooldown = 5;

	public Transform Target
	{
		get { return _target; }
		set { _target = value; }
	}

	public float Speed => _speed;
	public float Anger => _anger;
	public float SightDistance => _sightDistance;
	public float AttackCooldown => _attackCooldown;
	public float Cooldown { get; private set; }

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

		DecreaseCooldown(Time.deltaTime);
		if (target == null || Cooldown > 0)
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
			IncreaseCooldown(_attackCooldown);
		}
		else
		{
			RotateTowardsTarget();
			MoveTowardsTarget(transform.position, target.position, Speed * Time.deltaTime);
		}
	}

	private void DecreaseCooldown(float deltaTime)
	{
		if (Cooldown > 0)
		{
			Cooldown = Mathf.Clamp(Cooldown - deltaTime, 0, float.MaxValue);
		}
	}

	private void IncreaseCooldown(float attackCooldown)
	{
		Cooldown += attackCooldown;
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
		EnemyRegistry?.UnregisterEnemy(this);
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
//		Debug.DrawRay(transform.position + new Vector3(0,1,0), newDir * _attackRange, Color.red);
	}
	
	[Conditional("UNITY_EDITOR")]
	private void DrawSightRange()
	{
		var step = Speed * Time.deltaTime;
		var targetDir = Target.position - transform.position;
		var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
//		Debug.DrawRay(transform.position + new Vector3(0,2,0), newDir * _sightDistance, Color.green);
	}
	
	private float time = 0.0f;
	public float interpolationPeriod = 1.0f;
	private Swarm swarm;
	
	private void OnTriggerStay(Collider other) {
		time += Time.deltaTime;
		if (
			other.gameObject.layer == LayerMask.NameToLayer("Player1")
			|| other.gameObject.layer == LayerMask.NameToLayer("Player2")
			) {

			Debug.Log("hit");
			
			// every second
			if (time >= interpolationPeriod) {
				time = 0.0f;
				Debug.Log("Kill");

				// kill a slime from swarm
				Debug.Log(other.gameObject);
				other.gameObject.GetComponent<Boid>().Swarm.killSlime();
			}
		}
	}
}
