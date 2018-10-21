using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour {

	public GameObject ParticleEffectPrefab;
	
	public int gatherableMaxHealth = 4;
	
	[SerializeField] private int _swarmHealth1 = 10;
	[SerializeField] private int _swarmHealth2 = 10;
	
	[SerializeField] private int _score = 0;
	[SerializeField] private int _res = 0;

	[SerializeField] private TextMeshProUGUI _swarm1Text;
	[SerializeField] private TextMeshProUGUI _swarm2Text;
	[SerializeField] private TextMeshProUGUI _scoreText;
	
	public int SwarmHealth1
	{
		get { return _swarmHealth1; }
		set { _swarmHealth1 = value; }
	}
	
	public int SwarmHealth2
	{
		get { return _swarmHealth2; }
		set { _swarmHealth2 = value; }
	}
	
	public int Score
	{
		get { return _score; }
		set { _score = value; }
	}
	
	public int Res
	{
		get { return _res; }
		set { _res = value; }
	}
	
	// Use this for initialization
	void Start ()
	{
		OnSwarmHealth1Changed();
		OnSwarmHealth2Changed();
		OnScoreChanged();
	}

	private void OnSwarmHealth1Changed()
	{
		_swarm1Text.text = SwarmHealth1.ToString();
	}

	private void OnSwarmHealth2Changed()
	{
		_swarm2Text.text = SwarmHealth2.ToString();
	}
	
	private void OnScoreChanged()
	{
		_scoreText.text = Score.ToString();
	}
}
