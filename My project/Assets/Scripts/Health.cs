using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth = 100;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " ha recibido " + amount + " de daño. Salud actual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        // Aquí puedes añadir la lógica para la muerte del jugador, como desactivar el objeto,
        // mostrar una pantalla de Game Over, etc.
    }
}