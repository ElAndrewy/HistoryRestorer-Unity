using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal; // IMPORTANTE para controlar luces 2D

public class RestorationManager : MonoBehaviour
{
    // Hacemos el manager accesible desde cualquier lugar (Singleton)
    public static RestorationManager Instance;

    [Header("Configuración de Restauración")]
    public float targetIntensity = 1.0f; // Intensidad de la luz al restaurarse
    public float restorationSpeed = 2f;    // Qué tan rápido se "enciende" la luz

    // Lista de las luces puntuales sobre las estanterías (en orden de izquierda a derecha)
    public List<Light2D> shelfLights;

    // Lista para guardar el estado de cada sección (false = apagada, true = restaurada)
    private List<bool> sectionRestoredStatus;

    void Awake()
    {
        // Configuración del Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        InitializeLibraryState();
    }

    void Update()
    {
        // Mecánica de suavizado: Enciende gradualmente las luces restauradas
        HandleLightTransitions();
    }

    private void InitializeLibraryState()
    {
        sectionRestoredStatus = new List<bool>();

        // Inicializamos todas las secciones como NO restauradas e intensidad 0
        for (int i = 0; i < shelfLights.Count; i++)
        {
            sectionRestoredStatus.Add(false);
            if (shelfLights[i] != null)
            {
                shelfLights[i].intensity = 0f; // Aseguramos que empiecen apagadas
            }
        }
    }

    // Método que llamaremos desde la estantería cuando el niño gane el desafío
    public void RestoreSection(int sectionIndex)
    {
        if (sectionIndex >= 0 && sectionIndex < sectionRestoredStatus.Count)
        {
            if (!sectionRestoredStatus[sectionIndex])
            {
                sectionRestoredStatus[sectionIndex] = true;
                Debug.Log($"¡Sección {sectionIndex} restaurada con éxito!");

                // AQUÍ PODRÍAS AÑADIR UN EFECTO DE SONIDO DE "MAGIA" O UN EFECTO DE PARTÍCULAS
            }
        }
    }

    private void HandleLightTransitions()
    {
        for (int i = 0; i < shelfLights.Count; i++)
        {
            if (shelfLights[i] == null) continue;

            // Si la sección está marcada como restaurada, aumentamos la intensidad suavemente
            if (sectionRestoredStatus[i])
            {
                // Usamos MoveTowards para una transición suave y controlada
                shelfLights[i].intensity = Mathf.MoveTowards(
                    shelfLights[i].intensity,
                    targetIntensity,
                    restorationSpeed * Time.deltaTime
                );
            }
        }
    }
}