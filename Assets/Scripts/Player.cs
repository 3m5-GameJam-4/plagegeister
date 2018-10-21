using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color Color;
    public string InputHorizontalAxis;
    public string InputVerticalAxis;
    public string InputPrevSwarm;
    public string InputNextSwarm;
    public string InputSpawnSwarm;

    public GameObject SwarmPrefab;
    public GameObject Dynamic;
    public GameObject Camera;

    private int _swarmIndex = 0;
    private List<Swarm> _swarms = new List<Swarm>();

    private void Start()
    {
        if (!Dynamic) Dynamic = GameObject.Find("_Dynamic");
    }

    public void nextSwarm()
    {
        if (_swarms.Any()) _swarmIndex = Math.Abs((_swarmIndex + 1) % _swarms.Count);
    }

    public void prevSwarm()
    {
        if (!_swarms.Any()) return; 
        _swarmIndex = (_swarmIndex - 1) % _swarms.Count;
        if (_swarmIndex < 0) _swarmIndex += _swarms.Count;
    }

    public void spawnSwarm()
    {
        var obj = Instantiate(SwarmPrefab, transform.position, transform.rotation, Dynamic.transform);
        var swarm = obj.GetComponent<Swarm>();
        swarm.Color = Color;
        _swarms.Add(swarm);
    }

    private void Update()
    {
        _swarms.ForEach((swarm) => swarm.Direction = Vector3.zero);
        if (currentSwarm())
        {
            var hori = Input.GetAxis(InputHorizontalAxis);
            var vert = Input.GetAxis(InputVerticalAxis);
            currentSwarm().Move(hori, vert);
        }

        if (Input.GetButtonDown(InputPrevSwarm))
        {
            prevSwarm();
        }

        if (Input.GetButtonDown(InputNextSwarm))
        {
            nextSwarm();
        }

        if (Input.GetButtonDown(InputSpawnSwarm))
        {
            spawnSwarm();
        }
    }

    private Swarm currentSwarm()
    {
        return _swarms.Count > 0 ? _swarms[_swarmIndex] : null;
    }
}