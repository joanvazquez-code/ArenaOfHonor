using UnityEngine;

public class ArmaRecogible : MonoBehaviour
{
    public float distanciaRecogida = 3f;
    private GameObject jugador;
    private PlayerMovement playerScript;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            playerScript = jugador.GetComponent<PlayerMovement>();
            Debug.Log($"🎯 Jugador encontrado: {jugador.name}");
            
            // Verificar si el punto de sujeción está asignado
            if (playerScript.puntoSujecionArma == null)
            {
                Debug.LogError("❌ PUNTO SUJECIÓN NO ASIGNADO en el jugador");
            }
            else
            {
                Debug.Log($"✅ Punto sujeción: {playerScript.puntoSujecionArma.name}");
            }
        }
    }

    void Update()
    {
        if (jugador == null || playerScript == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.transform.position);
        
        // Debug de distancia
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"📏 Distancia a {gameObject.name}: {distancia:F1}");
        }
        
        if (distancia <= distanciaRecogida && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"🔼 E presionada - Intentando recoger {gameObject.name}");
            
            if (!playerScript.tieneArma)
            {
                RecogerArma();
            }
            else
            {
                Debug.Log("❌ Jugador ya tiene arma");
            }
        }
    }

    void RecogerArma()
    {
        Debug.Log($"🎯 Recogiendo {gameObject.name}...");
        playerScript.RecogerArma(this.gameObject);
    }
}