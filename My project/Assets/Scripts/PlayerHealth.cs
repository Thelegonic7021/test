using UnityEngine;
using UnityEngine.SceneManagement; // Si quieres reiniciar escena

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Vida inicial del jugador: " + currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return; // Evitar daño inválido

        currentHealth -= amount;
        Debug.Log("¡El jugador recibió daño! Vida actual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("¡El jugador murió!");

        // Puedes reiniciar la escena si quieres
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        // O mostrar pantalla de Game Over, desactivar movimiento, etc.
        gameObject.SetActive(false);
    }
}
