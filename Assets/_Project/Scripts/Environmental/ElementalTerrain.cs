using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalTerrain : MonoBehaviour
{
        // [SerializeField] private bool equal = true;
        [SerializeField] private ElementalSystemTypes type;
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        PlayerElementalSystemSwitchElement.StaticPlayerElementType.onElementTypeChange.AddListener(Change);
        Change();
    }

    // Update is called once per frame
    public void Change()
    {
       

        for (int i = 0; i < System.Enum.GetValues(typeof(ElementalSystemTypes)).Length; i++)
        {
            if (type == PlayerElementalSystemSwitchElement.StaticPlayerElementType.Type)
            {
                Physics.IgnoreCollision(player.GetComponent<BoxCollider>(), target.GetComponent<BoxCollider>(), true);
            }
            else
            {
                Physics.IgnoreCollision(player.GetComponent<BoxCollider>(), target.GetComponent<BoxCollider>(), false);
            }
        }
    }
}
