using UnityEngine;

public class RecogerEspada : MonoBehaviour
{
    PlayerMovement playerMovement;
    public GameObject espada;
    bool puedeRecoger = true;
   // public bool isArmeria = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎯 Jugador en zona de recogida de espada");
            if(Input.GetKeyDown(KeyCode.E) && puedeRecoger)
            {
                Debug.Log("🎯 Recogiendo espada...");
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
                Debug.Log("✅ PlayerMovement encontrado en el jugador");
                if (playerMovement != null)
                {
                    playerMovement.tieneArma = true;
                    puedeRecoger = false;
                    Destroy(espada); // Destruye la espada del suelo
    
                }
            }
            
        }
    }
}
