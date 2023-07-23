using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] int numberOfEnemies;
    ManagersManager managersManager;
    List<AiAim> enemies;
    SaucerController player;
    int maxEnemies;


    // Start is called before the first frame update
    void Start()
    {
        managersManager = FindObjectOfType<ManagersManager>();
        enemies = Utility.GetComponentsInRadius<AiAim>(transform.position, radius);
        player = FindObjectOfType<SaucerController>();
        numberOfEnemies = enemies.Count;
        maxEnemies = numberOfEnemies;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            ToggleUI(true);      
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.tag == "Player")
        {
            ToggleUI(false);
        }    
    }


    public void EnemyKilled()
    {
        numberOfEnemies--;
        UpdateUI();
    }
    public int GetEnemyCount()
    {
        return numberOfEnemies;
    }
    private void UpdateUI()
    {
        float precent = Mathf.InverseLerp(0, maxEnemies, numberOfEnemies);
        managersManager.UIManager.enemeyCounterFillUpBar.fillAmount = precent;
    }
    
    private void ToggleUI(bool toggle)
    {
        managersManager.UIManager.enemyCounter.SetActive(toggle);
        UpdateUI();
    }
}
