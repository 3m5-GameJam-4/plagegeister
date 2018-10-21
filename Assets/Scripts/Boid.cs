using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    public Rigidbody Rb;
    public Transform Destination;
    public Swarm Swarm;

    public float MinSpeed = 1f;
    public float MaxSpeed = 2f;
    public float MaxForce = 1f;
    public ForceMode ForceMode = ForceMode.Impulse;

    public float CohesionRadius = 1f;
    public float CohesionFactor = 1f;
    public float CohesionDampingRadius = 1f;

    public float AlignmentRadius = 1f;
    public float AlignmentFactor = 1f;

    public float SeparationFactor = 1f;
    public float SeparationRadius = 1f;

    public float DestinationFactor = 1f;
    public float DestinationRadius = 1f;

    public float RandomRange = 1f;

    private void Start()
    {
        if (!Rb) Rb = GetComponent<Rigidbody>();
        Rb.velocity = new Vector3(Rand(), Rand(), Rand());
    }

    private float Rand()
    {
        return Random.Range(-RandomRange, RandomRange);
    }

    private void FixedUpdate()
    {
        var neighbors = Swarm?.Boids;
        var force = Flock(neighbors);
        Rb.AddForce(force, ForceMode);
        transform.rotation = Quaternion.LookRotation(Rb.velocity);
    }

    private Vector3 Flock(ICollection<GameObject> neighbors)
    {
        if (neighbors.Count > 0)
        {
            var rbs = ToRigidbodies(neighbors);
            var cohesion = Cohere(rbs) * CohesionFactor;
            var alignment = Align(rbs) * AlignmentFactor;
            var separation = Separate(rbs) * SeparationFactor;
            var destination = ToDestination(Destination) * DestinationFactor;
            var random = new Vector3(Rand(), Rand(), Rand());
            var forward = (Rb.velocity == Vector3.zero ? Vector3.left : Rb.velocity.normalized) * Mathf.Max(0, MinSpeed - Rb.velocity.magnitude);
            return cohesion + alignment + separation + destination + random + forward;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 ToDestination(Transform dest)
    {
        return dest && (dest.position - Rb.position).magnitude > DestinationRadius ? SteerTo(dest.position) : Vector3.zero;
    }

    private Vector3 Separate(ICollection<Rigidbody> neighbors)
    {
        var near = neighbors.Where((boid) =>
        {
            if (boid == Rb) return false;
            var d = (boid.position - Rb.position).magnitude;
            return d > 0 && d < SeparationRadius;
        });
        if (!near.Any()) return Vector3.zero;
        var mean = near
            .Select((boid) => boid.position - Rb.position)                
            .Aggregate((current, diff) => current - MaxForce * diff.normalized / diff.magnitude);
        return mean / neighbors.Count;
    }

    private Vector3 Align(ICollection<Rigidbody> neighbors)
    {
        var near = neighbors.Where((boid) =>
        {
            if (boid == Rb) return false;
            var d = (boid.position - Rb.position).magnitude;
            return d > 0 && d < AlignmentRadius;
        });
        if (!near.Any()) return Vector3.zero;
        var center = near
            .Select((boid) => boid.velocity)
            .Aggregate(Vector3.zero, (current, vel) => current + vel);
        return Vector3.ClampMagnitude(center / neighbors.Count, MaxForce);
    }

    private Vector3 Cohere(ICollection<Rigidbody> neighbors)
    {
        var near = neighbors.Where((boid) =>
        {
            if (boid == Rb) return false;
            var d = (boid.position - Rb.position).magnitude;
            return d > 0 && d < CohesionRadius;
        });
        if (!near.Any()) return Vector3.zero;
        var center = near
            .Select((boid) => boid.position)
            .Aggregate(Vector3.zero, (current, pos) => current + pos);
        return CoherSteerTo(center / neighbors.Count);
    }

    private Vector3 CoherSteerTo(Vector3 to)
    {
        var desired = to - Rb.position;
        var d = desired.magnitude;

        if (d <= 0f) return Vector3.zero;

        desired.Normalize();
        desired *= MaxSpeed;

        if (d < CohesionDampingRadius)
        {
            desired *= d / CohesionDampingRadius;
        }

        var steer = desired - Rb.velocity;
        return Vector3.ClampMagnitude(steer, MaxSpeed);
    }

    private Vector3 SteerTo(Vector3 to)
    {
        var desired = to - Rb.position;
        return desired.normalized * MaxForce * (Rb.velocity.magnitude > MaxSpeed ? 0 : 1);
    }

    private Rigidbody[] ToRigidbodies(IEnumerable<GameObject> objs)
    {
        return objs.Select((obj) => obj.GetComponent<Rigidbody>()).ToArray();
    }
}