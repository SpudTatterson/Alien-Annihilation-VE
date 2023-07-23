using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterSystem : MonoBehaviour
{
    [SerializeField] ParticleSystem BackThruster;
    [SerializeField] float backThrusterStrengh;
    [SerializeField] ParticleSystem[] rightThrusters;
    [SerializeField] ParticleSystem[] leftThrusters;
    [SerializeField] AudioSource engineAudio;
    
    SaucerController player;

    float yaw;
    float forward;
    float absForward;
    float vertical;
    float velocityModifier;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<SaucerController>();
        UpdateBackBurnerStrength();
        UpdateRightSideBurnersStrength();
        UpdateLeftSideBurnersStrength();
    }

    // Update is called once per frame
    void Update()
    {
        velocityModifier = GetVelocityModifier();
        HandleInputs();
        if(forward > 0) UpdateBackBurnerStrength();
        if(yaw <= 0) UpdateRightSideBurnersStrength();
        if(yaw >= 0) UpdateLeftSideBurnersStrength();
    }
    void HandleInputs()
    {
        forward = Input.GetAxis("Forward");
        absForward = Mathf.Abs(forward);
        yaw = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
    void UpdateBackBurnerStrength()
    {  
        var main = BackThruster.main;
        main.simulationSpeed = 1 + (backThrusterStrengh * absForward * (velocityModifier + 1));
        main.startSpeed = Mathf.Lerp(1, 8, absForward);
    }
    void UpdateRightSideBurnersStrength()
    {
        for (int i = 0; i < rightThrusters.Length; i++)
        {
            float absYaw = Mathf.Abs(yaw);
            var main = rightThrusters[i].main;
            main.simulationSpeed = 0.5f + (backThrusterStrengh * absYaw);
            main.startSpeed = Mathf.Lerp(1, 8, absYaw);
        }
    }
    void UpdateLeftSideBurnersStrength()
    {
        for (int i = 0; i < leftThrusters.Length; i++)
        {
            float absYaw = Mathf.Abs(yaw);
            var main = leftThrusters[i].main;
            main.simulationSpeed = 0.5f + (backThrusterStrengh * absYaw);
            main.startSpeed = Mathf.Lerp(1, 8, absYaw);
        }
    }
    float GetVelocityModifier()
    {
        
        return Mathf.InverseLerp(0,200f,player.GetVelocity());
    }
    float GetABSFowardvelocity()
    {
        return Mathf.InverseLerp(0,2,absForward + velocityModifier);
    }
}
