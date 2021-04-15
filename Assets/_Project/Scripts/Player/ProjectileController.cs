using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class ProjectileController : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player")) { return; }
            gameObject.SetActive(false);
        }
    }
}
