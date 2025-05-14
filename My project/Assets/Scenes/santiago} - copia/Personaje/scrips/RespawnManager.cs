using UnityEngine;

// Gestor de checkpoints para respawn del jugador
public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    [Tooltip("Transform con el punto exacto de respawn para el jugador")]
    public Transform spawnPoint; // Punto de inicio default

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (spawnPoint == null)
        {
            // Usar posición del manager como respaldo y avisar al desarrollador
            spawnPoint = transform;
            Debug.LogWarning("[RespawnManager] SpawnPoint no asignado. Usando posición del manager como respaldo.");
        }
    }

    // Método para obtener la posición de respawn
    public Vector3 GetSpawnPosition()
    {
        return spawnPoint.position;
    }
}
