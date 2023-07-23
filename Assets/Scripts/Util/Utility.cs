using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Utility : MonoBehaviour
{

    public static List<Health> GetHealthObjectsInRadius(Vector3 position, float explosionRadius)
    {
        List<Health> hitObjects = new List<Health>();
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            GameObject hitObject = hits[i].gameObject;
            Health health = hitObject.GetComponentInParent<Health>();
            if (!health) continue; 
            hitObjects.Add(health); 
        }
        List<Health> distinctHitObjects = hitObjects.Distinct().ToList();
        return distinctHitObjects;
    }
    public static List<GameObject> GetGameObjectsInRadius(Vector3 position, float explosionRadius)
    {
        List<GameObject> hitObjects = new List<GameObject>();
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            GameObject hitObject = hits[i].gameObject;
            hitObjects.Add(hitObject); 
        }
        List<GameObject> distinctHitObjects = hitObjects.Distinct().ToList();
        return distinctHitObjects;
    }
    public static List<T> GetComponentsInRadius<T>(Vector3 position, float explosionRadius)
    {
        List<T> hitObjects = new List<T>();
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            GameObject hitObject = hits[i].gameObject;
            T t = hitObject.GetComponentInParent<T>();
            if (t == null) continue;
            hitObjects.Add(t);
        }
        List<T> distinctHitObjects = hitObjects.Distinct().ToList();
        return distinctHitObjects;
    }
    public static T GetComponentInRadius<T>(Vector3 position, float explosionRadius)
    {
        List<T> hitObjects = new List<T>();
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            GameObject hitObject = hits[i].gameObject;
            T t = hitObject.GetComponentInParent<T>();
            if (t == null) continue;
            hitObjects.Add(t);
        }
        List<T> distinctHitObjects = hitObjects.Distinct().ToList();
        if(distinctHitObjects.Count != 0) return distinctHitObjects[0];
        else return default;
        
    }
}
