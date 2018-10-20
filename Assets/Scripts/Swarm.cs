using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Rigidbody2D Rb;

    public Vector2 Direction = Vector2.zero;
    public float Force = 0.5f;
    public ForceMode2D ForceMode = ForceMode2D.Impulse;

    public Color Color
    {
        get { return Renderer.color; }
        set { Renderer.color = value; }
    }

    private void Start()
    {
        if (!Renderer) Renderer = GetComponent<SpriteRenderer>();
        if (!Rb) Rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Rb.AddForce(Direction * Force, ForceMode2D.Impulse);
    }
}