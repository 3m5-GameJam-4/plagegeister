using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Swarm : MonoBehaviour, ISwarmControl
{
    public SpriteRenderer Renderer;
    public Rigidbody Rb;
    public GameObject BoidPrefab;
    public GameObject Dynamic;
    public Player Player;

    public Vector3 Direction = Vector3.zero;
    public float Force = 0.5f;
    public ForceMode ForceMode = ForceMode.Impulse;
    public int InitialBoids = 10;

    public List<GameObject> Boids = new List<GameObject>();
    private EnemyRegistry _enemyRegistry;

    public Color Color
    {
        get { return Renderer.color; }
        set { Renderer.color = value; }
    }
    
    private void Start()
    {
        if (!Renderer) Renderer = GetComponent<SpriteRenderer>();
        if (!Rb) Rb = GetComponent<Rigidbody>();
        if (!Dynamic) Dynamic = GameObject.Find("_Dynamic");
        
        for (var i = 0; i < InitialBoids; i++)
        {
            spawnSlime();
        }
        
        _enemyRegistry = GameObject.FindGameObjectWithTag("EnemyRegister")?.GetComponent<EnemyRegistry>();
        _enemyRegistry?.RegisterPlayer(this);
    }

    private void FixedUpdate()
    {
        if (firstBoid()) transform.position = firstBoid().transform.position;
    }

    private GameObject firstBoid()
    {
        return Boids.Count > 0 ? Boids[0] : null;
    }

    public void Move(float hori, float vert)
    {
        var dir = new Vector3(hori, 0, vert);
        Direction = transform.position + dir * 10f;
    }

    public void killSlime()
    {
        Boids.RemoveAt(Mathf.FloorToInt(Random.Range(0, Boids.Count)));

        if (Boids.Count == 0)
        {
            _enemyRegistry?.UnregisterPlayer(this);
            Player.killSwarm(gameObject);
        }
    }

    public void spawnSlime()
    {
        var obj = Instantiate(BoidPrefab, transform.position, transform.rotation, Dynamic.transform);
        var boid = obj.GetComponent<Boid>();
        boid.Swarm = this;
        obj.layer = LayerMask.NameToLayer(Player.gameObject.name);
        
        var rand = boid.DestinationRadius;
        obj.transform.position = new Vector3(Random.Range(-rand, rand), 0, Random.Range(-rand, rand));
        
        Boids.Add(obj);
    }
}