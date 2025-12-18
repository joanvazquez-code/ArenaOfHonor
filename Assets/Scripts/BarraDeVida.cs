using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    public Image fill; // Asignar en el inspector (usa el objeto 'fill')
    public PlayerMovement playerScript;
    private float maxVida;

    void Start()
    {
      /*  GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {*/
           // playerScript = playerObj.GetComponent<PlayerMovement>();
            maxVida = playerScript.maxVida;
        //}
    }

    public void ActualizarBarra()
    {
        if (playerScript != null && fill != null)
            fill.fillAmount = Mathf.Clamp01(playerScript.vidaActual / maxVida);
    }
}