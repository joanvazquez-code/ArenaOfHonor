using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Estado del Juego")]
    public bool juegoEnCurso = false;
    public bool jugadorMuerto = false;
    public int puntuacion = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnEscenaCargada;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEscenaCargada(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Escena cargada: " + scene.name);
        
        if (scene.name == "Partida")
        {
            IniciarNuevaPartida();
        }
        else if (scene.name == "MenuPrincipal")
        {
            jugadorMuerto = false;
            juegoEnCurso = false;
        }
    }

    public void IniciarNuevaPartida()
    {
        juegoEnCurso = true;
        jugadorMuerto = false;
        puntuacion = 0;
        
        Debug.Log("🔄 Nueva partida iniciada");
        
        ReiniciarJugador();
    }

    public void JugadorMurio()
    {
        jugadorMuerto = true;
        juegoEnCurso = false;
        Debug.Log("💀 Jugador murió - GameManager notificado");
    }

    void ReiniciarJugador()
    {
        PlayerMovement jugador = FindObjectOfType<PlayerMovement>();
        if (jugador != null)
        {
            jugador.ReiniciarEstado();
            Debug.Log("✅ Jugador reiniciado por GameManager");
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró el PlayerMovement en la escena");
        }
    }

    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}