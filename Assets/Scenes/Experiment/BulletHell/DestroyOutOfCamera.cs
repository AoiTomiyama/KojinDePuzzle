using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfCamera : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        ps.GetCollisionEvents(other, events);

    }
}
