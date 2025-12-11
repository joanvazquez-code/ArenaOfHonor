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
        if (puedeRecoger)
        {
             if(Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("🎯 Recogiendo espada...");
                PlayerMovement playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                Debug.Log("✅ PlayerMovement encontrado en el jugador");
                if (playerMovement != null)
                {
                    playerMovement.tieneArma = true;
                     Destroy(espada); // Destruye la espada del suelo
                    puedeRecoger = false;
                   
    
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puedeRecoger = true;
        }
    }
}
