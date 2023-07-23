using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersManager : MonoBehaviour
{
    [HideInInspector] public MoneyManager moneyManager;
    [HideInInspector] public UIManager UIManager;
    [HideInInspector] public ObjectiveManager objectiveManager;
    [HideInInspector] public ShopManger shopManger;
    [HideInInspector] public TimeManager timeManager;

    private void Awake() 
    {
        moneyManager = FindObjectOfType<MoneyManager>();
        UIManager = FindObjectOfType<UIManager>();
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        shopManger = FindObjectOfType<ShopManger>();
        timeManager = FindObjectOfType<TimeManager>();
    }
}
