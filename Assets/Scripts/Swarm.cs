using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Rigidbody Rb;
    public GameObject BoidPrefab;
    public GameObject Dynamic;

    public Vector3 Direction = Vector3.zero;
    public float Force = 0.5f;
    public ForceMode ForceMode = ForceMode.Impulse;
    public int Boids = 10;

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
        
        for (var i = 0; i < Boids; i++)
        {
            var obj = Instantiate(BoidPrefab, transform.position + Vector3.left * i, transform.rotation, Dynamic.transform);
            var boid = obj.GetComponent<Boid>();
            boid.Destination = transform;
        }
    }

    private void FixedUpdate()
    {
        Rb.AddForce(Direction * Force, ForceMode.Impulse);
    }
}