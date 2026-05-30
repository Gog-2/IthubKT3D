using UnityEngine;

public class TestTarget : MonoBehaviour, IInteractable, IDamageable
{
    [SerializeField] private int currentHealth = 100;

    public string GetInteractionPrompt() 
    {
        return $"Ящик (HP: {currentHealth}) - Нажмите [E]";
    }

    public void Interact() 
    {
        Debug.Log("<color=green>Вы пнули ящик! Взаимодействие сработало.</color>");
    }

    public void TakeDamage(int damage) 
    {
        currentHealth -= damage;
        Debug.Log($"Ящик получил урон. Осталось HP: {currentHealth}");
        
        if (currentHealth <= 0)
        {
            Debug.Log("Ящик уничтожен!");
            Destroy(gameObject);
        }
    }
}