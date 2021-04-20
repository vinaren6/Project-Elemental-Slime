using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgWithWrongElement : MonoBehaviour
{
    [SerializeField] private ElementalSystemTypes type;
    bool DealDmg;
    float Timer;
    // Start is called before the first frame update
    void Start()
    {
        PlayerElementalSystemSwitchElement.StaticPlayerElementType.onElementTypeChange.AddListener(Change);
        Change();
    }
    private void Change()
    {
        if (type == PlayerElementalSystemSwitchElement.StaticPlayerElementType.Type )
        {
            DealDmg = false;
        }
       
        else
        {
            DealDmg = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {      
        if (other.tag == "Player")
        {
            Timer = 0f; 
        }  
    }

    private void OnTriggerStay(Collider other)
    {
        if (DealDmg)
        {
            if (other.tag == "Player")
            {
                Timer += Time.deltaTime;
                if (Timer >= 1f)
                {
                    if (other.TryGetComponent(out IHealth health))
                    {
                        health.ReceiveDamage(type, 1);
                    }
                    Timer = 0;
                }
            }
        }
    }
}
