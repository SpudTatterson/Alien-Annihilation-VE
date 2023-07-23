using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    ManagersManager managersManager;
    // Start is called before the first frame update
    void Start()
    {
        managersManager = FindObjectOfType<ManagersManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PickUp()
    {
        Invoke("TurnOffParticleSystem", 1f);
        managersManager.objectiveManager.FoundObjective();

    }
    void TurnOffParticleSystem()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
    }
}
