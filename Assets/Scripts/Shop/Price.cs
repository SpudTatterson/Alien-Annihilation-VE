using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Price : MonoBehaviour
{
    [SerializeField] float price;

    MoneyManager moneyManager;
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        CanAfford();
    }
    public void CanAfford()
    {
        if(moneyManager.CanAfford(price))   button.interactable = true;
        else button.interactable = false;
    }
}
