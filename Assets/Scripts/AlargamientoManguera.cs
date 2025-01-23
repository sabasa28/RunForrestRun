using UnityEngine;

public class AlargamientoManguera : MonoBehaviour
{
    public Transform HuesoCabeza;
    public string BoneName;
    public float VelocidadAlargamiento;
    public MovimientoJugador movimientoJugador;
    public bool ColisionConJugador;
    public BoxCollider boxCollider;
    private bool siguiendoJugador;

    void Start()
    {
        HuesoCabeza = EncontraHueso(transform, BoneName);
        siguiendoJugador = false;
        ColisionConJugador = false;
    }

    void Update()
    {
        if (HuesoCabeza != null && ColisionConJugador)
        {
            Vector3 direction = movimientoJugador.ObtenerDireccionMovimiento();
            if (Input.GetKeyDown(KeyCode.R))
            {
                siguiendoJugador = !siguiendoJugador;
                movimientoJugador.AgarroManguera = !movimientoJugador.AgarroManguera;
            }

            if (siguiendoJugador)
            {
                HuesoCabeza.Rotate(0, 0, -direction.x * Time.deltaTime * 90);
                HuesoCabeza.localPosition += new Vector3(
                    VelocidadAlargamiento * (direction.x * Time.deltaTime),
                    VelocidadAlargamiento * (direction.z * Time.deltaTime),
                    0
                );
            }
        }
    }

    private Transform EncontraHueso(Transform parent, string BoneName)
    {
        foreach (Transform child in parent)
        {
            Debug.Log(child.name);
            if (child.name == BoneName)
            {
                Debug.Log("Se encontro el hueso" + child.name);
                return child;
            }
            else
            {
                Transform found = EncontraHueso(child, BoneName);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }
}