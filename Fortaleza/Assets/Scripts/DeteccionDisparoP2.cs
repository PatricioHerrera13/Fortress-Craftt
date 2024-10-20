using System.Collections;
using UnityEngine;

public class DeteccionDisparoP2 : MonoBehaviour
{
    public Collider zonaRecarga;
    public GameObject cañon2;  // Cañón izquierdo para player 2
    public Collider p2;  // Collider del jugador 2
    public float VelLen = 1f;  // Velocidad lenta (movimiento hacia atrás)
    public float VelRap = 3f;  // Velocidad rápida (movimiento hacia adelante)
    public float TiempoCarga = 2f;  // Tiempo de carga antes de disparar
    private Vector3 dirIzquierda = Vector3.right;  // Movimiento hacia atrás para cañón izquierdo
    private Vector3 posInicialCañon2;  // Posición inicial del cañón izquierdo
    public GameObject proyectil;
    public Transform puntoDisparo2;  // Punto de salida del proyectil para cañón2
    public float fuerzaDisparo = 20f;  // Fuerza con la que el proyectil será disparado

    private bool haDisparado = false;  // Flag para evitar múltiples disparos
    private bool enZonaRecarga = false;  // Flag para saber si el jugador está en la zona

    void Start()
    {
        Debug.Log("Pos. Inicial Lista para Cañón 2");
        posInicialCañon2 = cañon2.transform.position;
    }

    // Se detecta cuando el jugador entra en la zona de recarga
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other == p2)  // Verificamos que sea el jugador 2
        {
            enZonaRecarga = true;  // El jugador está en la zona de recarga
            Debug.Log("Player 2 ha entrado en la zona de recarga");
        }
    }

    // Se detecta cuando el jugador sale de la zona de recarga
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other == p2)
        {
            enZonaRecarga = false;  // El jugador ha salido de la zona de recarga
            Debug.Log("Player 2 ha salido de la zona de recarga");
        }
    }

    void Update()
    {
        if (enZonaRecarga && Input.GetKey(KeyCode.O) && !haDisparado)
        {
            Debug.Log("Player 2 interactúa con el cañón");
            StartCoroutine(MoverCañon(cañon2, dirIzquierda, posInicialCañon2, puntoDisparo2, Vector3.left));
        }
    }

    IEnumerator MoverCañon(GameObject cañon, Vector3 direccion, Vector3 posInicial, Transform puntoDisparo, Vector3 direccionDisparo)
    {
        Debug.Log("Preparando Cañón: " + cañon.name);
        
        haDisparado = true;  // Evitar múltiples disparos

        // Movimiento hacia atrás (lento)
        float tiempo = 0f;
        Vector3 objetivo = cañon.transform.position + direccion;

        while (tiempo < TiempoCarga)
        {
            cañon.transform.position = Vector3.Lerp(cañon.transform.position, objetivo, tiempo / TiempoCarga);
            tiempo += Time.deltaTime * VelLen;
            yield return null;
        }

        Debug.Log("Pausa...");
        // Pausa en la posición de disparo
        yield return new WaitForSeconds(0.5f);

        // Instanciar el proyectil en la punta del cañón
        DispararProyectil(puntoDisparo, direccionDisparo);

        // Movimiento hacia adelante (rápido)
        tiempo = 0f;
        while (tiempo < TiempoCarga)
        {
            cañon.transform.position = Vector3.Lerp(cañon.transform.position, posInicial, tiempo / TiempoCarga);
            tiempo += Time.deltaTime * VelRap;
            yield return null;
        }

        // Permitir que el cañón vuelva a disparar
        haDisparado = false;
    }

    void DispararProyectil(Transform puntoDisparo, Vector3 direccionDisparo)
    {
        Debug.Log("Disparando proyectil...");
        GameObject proyectilInstanciado = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = proyectilInstanciado.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(direccionDisparo * fuerzaDisparo, ForceMode.Impulse);
        }
    }
}
