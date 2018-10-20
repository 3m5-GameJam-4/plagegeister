using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByForce : MonoBehaviour
{
    public Rigidbody2D rb;

    public float thrust = 1f;

    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var hori = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");

        var direction = new Vector2(hori, vert);
        
        rb.AddForce(direction * thrust, ForceMode2D.Impulse);
    }

    public void Foo()
    {
        Debug.Log("XXX");
    }
}