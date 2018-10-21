using System.Collections.Generic;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
	[SerializeField] private Transform _target;
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
		InvokeRepeating(nameof(RecalculateAnger), 5, 5);
	}

	private void RecalculateAnger()
	{
		foreach (var enemy in _enemies)
		{
			var anger = enemy.Anger;
			var value = Random.Range(0.0f, 1.0f);
			if (value < anger)
			{
				enemy.Target = _target;
			}
			else
			{
				enemy.Target = null;
			}

			Debug.Log(value);
		}
	}
}
