using UnityEngine;
using UnityEngine.SceneManagement; // 👈 NECESARIO para cambiar escenas

public class CambioDeScenas : MonoBehaviour
{
  
  public void LoadMainMenu()
    {
        // Carga la escena del menú principal.
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void MainGame()
    {
        // Carga la escena del juego principal.
        UnityEngine.SceneManagement.SceneManager.LoadScene("Partida");
    }
    public void Instrucciones()
    {
        // Carga la escena del juego principal.
        UnityEngine.SceneManagement.SceneManager.LoadScene("ComoJugar");
    }
    
}