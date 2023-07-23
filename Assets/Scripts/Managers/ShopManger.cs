using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopManger : MonoBehaviour
{
    [SerializeField] Camera shopCamera;
    [SerializeField] Camera mainCamera;
    [SerializeField] KeyCode shopKeyCode;
    [SerializeField] UnityEvent shopOnEvents;
    [SerializeField] UnityEvent shopOffEvents;
    [SerializeField] Animation shopAnimation;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float moneyToStartAnimation = 1000f;
    [SerializeField] float maxMoneyToStartAnimation = 3000f;

    bool ShopActive;
    ManagersManager managersManager;
    MoneyManager moneyManager;
    Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        managersManager = FindObjectOfType<ManagersManager>();
        moneyManager = managersManager.moneyManager;
        playerHealth = FindObjectOfType<SaucerController>().GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!ShopActive && Input.GetKeyDown(shopKeyCode))
        {
            CamSwitch(false);
            ShopActive = true;
            shopOnEvents.Invoke();
            return;
        }
        if(ShopActive && (Input.GetKeyDown(shopKeyCode) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CamSwitch(true);
            ShopActive = false;    
            shopOffEvents.Invoke();        
        }
        if(moneyManager.CanAfford(moneyToStartAnimation) && moneyToStartAnimation <= maxMoneyToStartAnimation)
        {   
            shopAnimation.Play();
            audioSource.Play();
            moneyToStartAnimation += 1000;
        }
    }
    void CamSwitch(bool toggle)
    {
        mainCamera.gameObject.SetActive(toggle);
        shopCamera.gameObject.SetActive(!toggle);
    }
    void SetNewAnimationMoney(float moneyToStartAnimation)
    {
        this.moneyToStartAnimation = moneyToStartAnimation;
    }
}
