using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyParticleController : MonoBehaviour
{
    private ParticleSystem particle;
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
    }

    public void enableEmission()
    {
        if (!particle.isPlaying)
        {
            particle.Play();
        }
        var emission = particle.emission;
        emission.enabled = true;
    }

    public void disableEmission()
    {
        var emission = particle.emission;
        emission.enabled = false;
    }

    public void playParticleSystem()
    {
        particle.Play();
    }

    public void stopParticleSystem()
    {
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
