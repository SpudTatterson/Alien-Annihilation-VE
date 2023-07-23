using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] float money = 0;
    ManagersManager managersManager;
    // Start is called before the first frame update
    void Start()
    {
        managersManager = FindObjectOfType<ManagersManager>();
        UpdateMoneyUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            AddMoney(1000f);
        }
    }
    public void AddMoney(float moneyToAdd)
    {
        money += moneyToAdd;
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        managersManager.UIManager.score.text = money.ToString();
    }

    public bool CanAfford(float itemCost)
    {
        if(money >= itemCost) return true;
        else return false;
    }
}
