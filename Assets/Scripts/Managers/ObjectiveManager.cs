using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] float objectives;
    float objectivesFound = 0;
    ManagersManager managersManager;
    Health playerHealth;
    
    void Start() 
    {
        managersManager = FindObjectOfType<ManagersManager>();
        playerHealth = FindObjectOfType<SaucerController>().gameObject.GetComponent<Health>();
    }

    public void FoundObjective()
    {
        objectivesFound++;
        managersManager.moneyManager.AddMoney(300f);
        playerHealth.GiveHealth(playerHealth.maxHealth - playerHealth.GetHealth());
        UpdateUI();
        if(objectivesFound == objectives)
        {
            //trigger win
        }
    }
    void UpdateUI()
    {
        managersManager.UIManager.objectivesnumber.text = objectivesFound + "/" + objectives;
    }
}
