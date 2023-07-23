using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManaget : MonoBehaviour
{
    [SerializeField] GameObject rightClickImage;
    [SerializeField] GameObject FirstPart;
    [SerializeField] EnemyCounter enemyCounter;
    [SerializeField] GameObject[] bounds;

    string oldText;
    UIManager uIManager;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<SaucerController>().transform;
        uIManager = FindObjectOfType<UIManager>();
        oldText = uIManager.boundsText.text;
        uIManager.boundsText.text = "You must retrieve the first part to continue";
    }

    // Update is called once per frame
    void Update()
    {
        if(FirstPart == null)
        {
            for (int i = 0; i < bounds.Length; i++)
            {
                bounds[i].SetActive(false);
            }
            uIManager.boundsText.text = oldText;
            return;
        }

       if(!FirstPart.GetComponent<Rigidbody>().isKinematic) 
       {
           rightClickImage.SetActive(false);
           return;
       }
       if(enemyCounter.GetEnemyCount() == 0)
       {
           rightClickImage.SetActive(true);
           rightClickImage.transform.LookAt(player,Vector3.up);
       }
    }

}
