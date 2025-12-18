using UnityEngine;

public class SpawnEnemigos : MonoBehaviour
{
    public GameObject prefabEnemigo;

    private int enemigosPendientesDeGenerar = 8;
    private int enemigosGeneradosALaVez = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemyAtPosition()
    {
        for (int i = 0; i < enemigosGeneradosALaVez; i++)
        {
            if (enemigosPendientesDeGenerar > 0)
            {
                GameObject enemigoNuevo = Instantiate(prefabEnemigo, transform.position, Quaternion.identity);
                enemigoNuevo.transform.position = new Vector3(
                    enemigoNuevo.transform.position.x + Random.Range(-4f, 4f),
                    enemigoNuevo.transform.position.y,
                    enemigoNuevo.transform.position.z
                );
                
                enemigosPendientesDeGenerar--;
            }
        }
        enemigosGeneradosALaVez++;
    }
}
