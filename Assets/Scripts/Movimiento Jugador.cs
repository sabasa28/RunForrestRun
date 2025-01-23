using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
 private CharacterController controller;
    public float speed = 6.0f;
    private Vector3 velocity;
    private Vector3 direccionMovimiento;
    public bool AgarroManguera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        AgarroManguera = false;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        direccionMovimiento = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        controller.Move(move * speed * Time.deltaTime);
    }

    public Vector3 ObtenerDireccionMovimiento ()
    {
        float movimientoVertical = Input.GetAxis("Vertical");
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        return new Vector3 (movimientoHorizontal, 0, movimientoVertical);
    }
}