using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCulling : MonoBehaviour
{
    [SerializeField] float distance;

    MeshRenderer[] meshRenderers;
    SaucerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<SaucerController>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetDistanceToPlayer() > distance)
        {
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = false;
            }
        }
        else if(GetDistanceToPlayer() < distance)
        {
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = true;
            }
        }
    }

    float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }
}
