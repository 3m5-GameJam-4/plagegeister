using System.Collections.Generic;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private float _reactionTime = 1.0f;
	private List<Enemy> _enemies = new List<Enemy>();
	private Random _random;

	public void RegisterEnemy(Enemy enemy)
	{
		if (_enemies.Contains(enemy))
		{
			return;
		}
		
		_enemies.Add(enemy);
	}

	public void UnRegisterEnemy(Enemy enemy)
	{
		_enemies.Remove(enemy);
	}

	private void Start()
	{
		_random = new Random();
		InvokeRepeating(nameof(RecalculateAnger), 0, _reactionTime);
	}

	private void RecalculateAnger()
	{
		foreach (var enemy in _enemies)
		{
			TraceEnemyOnSight(enemy);
			if (enemy.Target == null)
			{
				TraceEnemyOnAnger(enemy);
			}
		}
	}

	private void TraceEnemyOnSight(Enemy enemy)
	{
		var sightDistance = enemy.SightDistance;
		var distanceToNearestTarget = Vector3.Distance(_target.position, enemy.transform.position);

		enemy.Target = (distanceToNearestTarget < sightDistance)? _target : null;
	}

	private void TraceEnemyOnAnger(Enemy enemy)
	{
		var anger = enemy.Anger;
		var value = Random.Range(0.0f, 1.0f);
		
		enemy.Target = (value < anger)? _target : null;
	}
}
