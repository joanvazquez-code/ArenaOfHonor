using UnityEngine;

public class Arma : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Revisa si el objeto que entra en el trigger es el jugador (tag Player)
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerScript = other.GetComponent<PlayerMovement>();
            if (playerScript != null)
            {
                playerScript.PickupWeapon(); // Método del jugador que puedes personalizar
                Destroy(gameObject); // Elimina la arma física de la escena
            }
        }
    }
}
