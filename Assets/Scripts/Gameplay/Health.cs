using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float health;
    TextMeshProUGUI healthText;
    [SerializeField] GameObject discombobulatedPrefab;
    [SerializeField] UnityEvent deathEvents;
    [SerializeField]EnemyCounter enemyCounter;
    
    
    public float maxHealth;
    public bool canGetFallDamage;

    ManagersManager managersManager;
    
    Image[] healthImages;
    [HideInInspector] public Collision collision;
    float collisionDamageTimer = 0;

    //bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<AiAim>() && !enemyCounter)
        {
            enemyCounter = Utility.GetComponentInRadius<EnemyCounter>(transform.position, 500f);
        }
        canGetFallDamage = true;
        maxHealth = health;
        managersManager = FindObjectOfType<ManagersManager>();
        if (gameObject.tag == "Player")
        {
            healthText = managersManager.UIManager.playerHealth;
            healthImages = managersManager.UIManager.healthImages;
        }
        if (gameObject.tag == "Enemy") healthText = managersManager.UIManager.enemyHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        UpdateHealthText();
        UpdateHealthImages();
    }

    // Update is called once per frame
    void Update()
    {
        collisionDamageTimer -= Time.deltaTime;
    }
    public void TakeDamage(float damage)
    {     
        if(!CheckIfAlive()) return;

        health -= damage;
        UpdateHealthUI();

        if(!CheckIfAlive()) Die();
    }
    public void GiveHealth(float healthIncrement)
    {
        if(health < maxHealth)  health += healthIncrement;    
        else return;
        if(health > maxHealth) health = maxHealth;
        
        UpdateHealthUI();
    }
    bool CheckIfAlive()
    {
        if(health <= 0)
        {
            return false;
        }
        return true;
    }
    void Die()
    {
        deathEvents.Invoke();
        if(enemyCounter != null)  enemyCounter.EnemyKilled();
        else return;
        //isAlive = false;
    }

    public void UpdateMoney()
    {
        MoneyWorth moneyWorth = GetComponent<MoneyWorth>();
        if(!moneyWorth) return;
        float worth = moneyWorth.moneyWorth;
        managersManager.moneyManager.AddMoney(worth);
    }  
    public void Discombobulate()
    {
        GameObject temp = Instantiate(discombobulatedPrefab, transform.position,transform.rotation);
        temp.transform.localScale = transform.localScale;
        Rigidbody[] rb = temp.GetComponentsInChildren<Rigidbody>();
        Explode explode = temp.GetComponent<Explode>();
        if(explode)
        {
            explode.collision = collision;
            for (int i = 0; i < rb.Length; i++)
            {
                rb[i].maxDepenetrationVelocity = 0;
            }
        }
        Destroy(gameObject);
    }
    void ReciveCollisionDamgae(Collision collision)
    {
        if(!canGetFallDamage || collisionDamageTimer > 0) return;
        if(collision.relativeVelocity.magnitude < 40f)  return;
        if(collision.rigidbody != null && collision.rigidbody.mass < 2f) return;
        TakeDamage(collision.relativeVelocity.magnitude * 2f);
        StartCollisionDamageTimer();
        
    }
    void StartCollisionDamageTimer()
    {
        collisionDamageTimer = 1f;
    }
    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Projectile") return;
        ReciveCollisionDamgae(other);
    }
    void UpdateHealthText()
    {
        if (!healthText) return;
        float healthPrecent = GetHealthPrecentage();
        healthText.text = healthPrecent.ToString("N0") + "%";
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetHealthPrecentage()
    {
        return (100 / maxHealth) * health;
    }

    void UpdateHealthImages()
    {
        
        if(healthImages == null) return;
        float healthPrecent = Mathf.InverseLerp(0, maxHealth, health);
        for (int i = 0; i < healthImages.Length; i++)
        {
            healthImages[i].fillAmount = healthPrecent;
        }
    }
    public bool IsCloseToDeath()
    {
        if(GetHealthPrecentage() < 25f)
        {
            return true;
        }
        return false;
    }
    
}


