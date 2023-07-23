using UnityEngine;
using UnityEngine.Events;


public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] UnityEvent weapon1OnEvents;
    [SerializeField] UnityEvent weapon1OffEvents;
    [SerializeField] UnityEvent weapon2OnEvents;
    [SerializeField] UnityEvent weapon2OffEvents;
    

    // Start is called before the first frame update
    void Start()
    {
        weapon1OnEvents.Invoke();
        weapon2OffEvents.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Weapon1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Weapon2();
        }
    }

    public void Weapon2()
    {
        weapon1OffEvents.Invoke();
        weapon2OnEvents.Invoke();
    }

    public void Weapon1()
    {
        weapon1OnEvents.Invoke();
        weapon2OffEvents.Invoke();
    }
    public void AllWeaponsOff()
    {
        weapon1OffEvents.Invoke();
        weapon2OffEvents.Invoke();
    }
}
