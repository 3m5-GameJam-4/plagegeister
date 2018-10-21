﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private float _reactionTime = 1.0f;
	private List<Enemy> _enemies = new List<Enemy>();
	[SerializeField] private List<Player> _players = new List<Player>();
	private Random _random;

	private void Start()
	{
		_random = new Random();
		InvokeRepeating(nameof(RecalculateAnger), 0, _reactionTime);
	}

	private void RecalculateAnger()
	{
		foreach (var enemy in _enemies)
		{
			var nearestPlayer = FindNearestPlayer(enemy);
			if (nearestPlayer == null)
			{
				continue;
			}
			
			TraceTargetOnSight(enemy, nearestPlayer);
			if (enemy.Target == null)
			{
				TraceTargetOnAnger(enemy, nearestPlayer);
			}
		}
	}

	private Player FindNearestPlayer(Enemy enemy)
	{	
		var player = _players.Select(element => new { element, Distance = Vector3.Distance(element.transform.position, enemy.transform.position) })
			.OrderBy(element => element.Distance)
			.FirstOrDefault();

		return player?.element;
	}

	private void TraceTargetOnSight(Enemy enemy, Player player)
	{
		var sightDistance = enemy.SightDistance;
		var distanceToNearestTarget = Vector3.Distance(player.transform.position, enemy.transform.position);

		enemy.Target = (distanceToNearestTarget < sightDistance)? player.transform : null;
	}

	private void TraceTargetOnAnger(Enemy enemy, Player player)
	{
		var anger = enemy.Anger;
		var value = Random.Range(0.0f, 1.0f);
		
		enemy.Target = (value < anger)? player.transform : null;
	}
	
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

	public void RegisterPlayer(Player player)
	{
		if (_players.Contains(player))
		{
			return;
		}
		
		_players.Add(player);
	}
	
	public void UnRegisterPlayer(Player player)
	{
		_players.Remove(player);
	}
}
