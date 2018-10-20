using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByForce : MonoBehaviour
{
    public Rigidbody2D rb;

    public string horizontalAxis;
    public string verticalAxis;
    public float thrust = 1f;
    public ForceMode2D forceMode = ForceMode2D.Impulse;
    
    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var hori = Input.GetAxis(horizontalAxis);
        var vert = Input.GetAxis(verticalAxis);

        var direction = new Vector2(hori, vert);
        
        rb.AddForce(direction * thrust, ForceMode2D.Impulse);
    }
}