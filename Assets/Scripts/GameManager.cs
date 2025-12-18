using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Estado del Juego")]
    public bool juegoEnCurso = false;
    public bool jugadorMuerto = false;
    public int puntuacion = 0;

    [Header("Rondas / Enemigos")]
    public int rondaActual = 1;
    public int killsEstaRonda = 0;
    public int maxEnemigosSimultaneos = 10;
    public int enemigosActuales = 1;      // cuántos enemigos simultáneos en esta ronda

    [Header("Escalado")]
    public float multiplicadorVidaGlobal = 1f;   // se multiplicará por 1.25 tras cada jefe

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

        // Reset sistema de rondas
        rondaActual = 1;
        killsEstaRonda = 0;
        enemigosActuales = 1;
        multiplicadorVidaGlobal = 1f;
        
        Debug.Log("🔄 Nueva partida iniciada");
        
        ReiniciarJugador();
        IniciarRonda();
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

    // ===================== RONDAS =====================

    public void IniciarRonda()
    {
        killsEstaRonda = 0;

        Debug.Log($"▶️ Iniciando ronda {rondaActual} con {enemigosActuales} enemigos (multVida: {multiplicadorVidaGlobal})");

        // true para incluir enemigos desactivados
        enemigo[] enemigos = GameObject.FindGameObjectsWithTag("enemigo")
                                    .Select(go => go.GetComponent<enemigo>())
                                    .ToArray();

        for (int i = 0; i < enemigos.Length; i++)
        {
            if (i < enemigosActuales)
            {
                enemigos[i].gameObject.SetActive(true);
                enemigos[i].ReiniciarEnemigo(multiplicadorVidaGlobal);
            }
            else
            {
                enemigos[i].gameObject.SetActive(false);
            }
        }
    }

    // Llamar desde enemigo cuando muera
    public void RegistrarKillEnemigo()
    {
        killsEstaRonda++;
        Debug.Log("☠️ Kill registrada. Total esta ronda: " + killsEstaRonda);

        // Si es la ronda 10 (enemigo especial)
        if (rondaActual == 10)
        {
            ReiniciarRondasDespuesDeJefe();
            return;
        }

        // Aumentar número de enemigos simultáneos hasta 10
        if (enemigosActuales < maxEnemigosSimultaneos)
        {
            enemigosActuales++;   // 1, 2, 3, ... 10
        }

        // Si ya estamos en el máximo, la siguiente es la ronda 10 (jefe)
        if (enemigosActuales >= maxEnemigosSimultaneos)
        {
            rondaActual = 10;     // ronda del “jefe”
            enemigosActuales = 1; // solo 1 enemigo en la ronda 10
        }
        else
        {
            rondaActual++;
        }

        IniciarRonda();
    }

    void ReiniciarRondasDespuesDeJefe()
    {
        Debug.Log("🏆 Jefe derrotado. Reiniciando rondas con enemigos más fuertes");

        // Vida global +25%
        multiplicadorVidaGlobal *= 1.25f;

        // Reiniciar a ronda 1 con 1 enemigo
        rondaActual = 1;
        enemigosActuales = 1;
        killsEstaRonda = 0;

        IniciarRonda();
    }
}
