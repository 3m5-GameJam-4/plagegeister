using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    public Rigidbody2D Rb;
    public Transform Destination;

    public float MinSpeed = 1f;
    public float MaxSpeed = 2f;
    public float MaxForce = 1f;
    public ForceMode2D ForceMode = ForceMode2D.Impulse;

    public float CohesionRadius = 1f;
    public float CohesionFactor = 1f;
    public float CohesionDampingRadius = 1f;

    public float AlignmentRadius = 1f;
    public float AlignmentFactor = 1f;

    public float SeparationFactor = 1f;
    public float SeparationRadius = 1f;

    public float DestinationFactor = 1f;

    public float RandomRange = 1f;

    private void Start()
    {
        if (!Rb) Rb = GetComponent<Rigidbody2D>();
        Rb.velocity = new Vector2(Rand(), Rand());
        //Rb.velocity = Vector2.left * MaxSpeed;
    }

    private float Rand()
    {
        return Random.Range(-RandomRange, RandomRange);
    }

    private void FixedUpdate()
    {
        var neighbors = GameObject.FindGameObjectsWithTag("Boid");
        var force = Flock(neighbors);
        Rb.AddForce(force, ForceMode);
    }

    private Vector2 Flock(ICollection<GameObject> neighbors)
    {
        if (neighbors.Count > 0)
        {
            var rbs = ToRigidbodies(neighbors);
            var cohesion = Cohere(rbs) * CohesionFactor;
            var alignment = Align(rbs) * AlignmentFactor;
            var separation = Separate(rbs) * SeparationFactor;
            var destination = ToDestination(Destination) * DestinationFactor;
            var random = new Vector2(Rand(), Rand());
            var forward = (Rb.velocity == Vector2.zero ? Vector2.left : Rb.velocity.normalized) * Mathf.Max(0, MinSpeed - Rb.velocity.magnitude);
            return cohesion + alignment + separation + destination + random + forward;
        }
        else
        {
            return Vector2.zero;
        }
    }

    private Vector2 ToDestination(Transform dest)
    {
        return dest ? SteerTo(dest.position) : Vector2.zero;
    }

    private Vector2 Separate(ICollection<Rigidbody2D> neighbors)
    {
        var near = neighbors.Where((boid) =>
        {
            if (boid == Rb) return false;
            var d = (boid.position - Rb.position).magnitude;
            return d > 0 && d < SeparationRadius;
        });
        if (!near.Any()) return Vector2.zero;
        var mean = near
            .Select((boid) => boid.position - Rb.position)                
            .Aggregate((current, diff) => current - diff.normalized / diff.magnitude);
        return mean / neighbors.Count;
    }

    private Vector2 Align(ICollection<Rigidbody2D> neighbors)
    {
        var near = neighbors.Where((boid) =>
        {
            if (boid == Rb) return false;
            var d = (boid.position - Rb.position).magnitude;
            return d > 0 && d < AlignmentRadius;
        });
        if (!near.Any()) return Vector2.zero;
        var center = near
            .Select((boid) => boid.velocity)
            .Aggregate(Vector2.zero, (current, vel) => current + vel);
        return Vector2.ClampMagnitude(center / neighbors.Count, MaxForce);
    }

    private Vector2 Cohere(ICollection<Rigidbody2D> neighbors)
    {
        var near = neighbors.Where((boid) =>
        {
            if (boid == Rb) return false;
            var d = (boid.position - Rb.position).magnitude;
            return d > 0 && d < CohesionRadius;
        });
        if (!near.Any()) return Vector2.zero;
        var center = near
            .Select((boid) => boid.position)
            .Aggregate(Vector2.zero, (current, pos) => current + pos);
        return CoherSteerTo(center / neighbors.Count);
    }

    private Vector2 CoherSteerTo(Vector2 to)
    {
        var desired = to - Rb.position;
        var d = desired.magnitude;

        if (d <= 0f) return Vector2.zero;

        desired.Normalize();
        desired *= MaxSpeed;

        if (d < CohesionDampingRadius)
        {
            desired *= d / CohesionDampingRadius;
        }

        var steer = desired - Rb.velocity;
        return Vector2.ClampMagnitude(steer, MaxSpeed);
    }

    private Vector2 SteerTo(Vector2 to)
    {
        var desired = to - Rb.position;
        return desired.normalized * MaxForce;
    }

    private Rigidbody2D[] ToRigidbodies(IEnumerable<GameObject> objs)
    {
        return objs.Select((obj) => obj.GetComponent<Rigidbody2D>()).ToArray();
    }
}