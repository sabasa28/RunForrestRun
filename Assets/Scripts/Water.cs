using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            Fire fire = other.gameObject.GetComponent<Fire>();
            fire.DespawnFire();
        }
    }

}
