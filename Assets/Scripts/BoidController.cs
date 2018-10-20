using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour
{
    public float minVelocity = 5;
    public float maxVelocity = 20;
    public float randomness = 1;
    public int flockSize = 20;
    public GameObject prefab;
    public GameObject chasee;

    public Vector2 flockCenter;
    public Vector2 flockVelocity;

    private GameObject[] boids;

    public Collider2D Collider;

    void Start()
    {
        boids = new GameObject[flockSize];
        for (var i = 0; i < flockSize; i++)
        {
            Vector3 position = new Vector3(
                                   Random.value * Collider.bounds.size.x,
                                   Random.value * Collider.bounds.size.y,
                                   Random.value * Collider.bounds.size.z
                               ) - Collider.bounds.extents;

            GameObject boid = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            boid.transform.parent = transform;
            boid.transform.localPosition = position;
            boid.GetComponent<BoidFlocking>().SetController(gameObject);
            boids[i] = boid;
        }
    }

    void Update()
    {
        Vector2 theCenter = Vector2.zero;
        Vector2 theVelocity = Vector2.zero;

        foreach (GameObject boid in boids)
        {
            if (!boid) continue;
            Vector2 boidPos = boid.transform.localPosition;
            theCenter = theCenter + boidPos;
            theVelocity = theVelocity + boid.GetComponent<Rigidbody2D>().velocity;
        }

        flockCenter = theCenter / flockSize;
        flockVelocity = theVelocity / flockSize;
    }
}