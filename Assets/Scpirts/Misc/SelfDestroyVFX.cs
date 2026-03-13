using System;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(ps && !ps.IsAlive())
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
