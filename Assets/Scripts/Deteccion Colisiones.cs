using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeteccionColisiones : MonoBehaviour
{
    public AlargamientoManguera alargamientoManguera;
    private void OnTriggerEnter (Collider otro)
    {
        if (otro.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colision con el jugador");
            alargamientoManguera.ColisionConJugador = true;
        }else if (Input.GetKeyDown(KeyCode.R))
        {
            alargamientoManguera.ColisionConJugador = false;
            alargamientoManguera.boxCollider.isTrigger = true;
        }
    }
}
