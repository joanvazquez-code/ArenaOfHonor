using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float distanciaAtaque = 2f;
    public int dañoPorGolpe = 10;
    public LayerMask capaEnemigos;
    public PlayerMovement playerMovement;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Atacar();
        }
    }

    void Atacar()
    {
        if (playerMovement == null) return;
        if (!playerMovement.tieneArma) return; // usas la variable que ya tienes

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaAtaque, capaEnemigos))
        {
            enemigo enemigo = hit.collider.GetComponent<enemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirDaño(dañoPorGolpe);  // baja de 10 en 10
            }
        }
    }
}
