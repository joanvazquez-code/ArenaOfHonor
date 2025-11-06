/*using UnityEngine;
using TMPro;

public class UIVidas : MonoBehaviour
{
    public TextMeshProUGUI vidasText; // Texto en pantalla
    public PlayerMovement player;     // Referencia al jugador

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (player != null && vidasText != null)
        {
            vidasText.text = "VIDAS: " + player.vida.ToString();
        }
    }
}*/
