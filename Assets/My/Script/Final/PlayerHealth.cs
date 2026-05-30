using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Damage Effect (Red Screen)")]
    public Image damageFlashImage;
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);
    public float flashSpeed = 5f;

    [Header("Death Settings")]
    public float restartDelay = 2f;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (damageFlashImage != null)
        {
            damageFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        }
    }

    void Update()
    {
        if (damageFlashImage != null && damageFlashImage.color.a > 0f)
        {
            Color c = damageFlashImage.color;
            c.a -= flashSpeed * Time.deltaTime;
            if (c.a < 0f) c.a = 0f;
            damageFlashImage.color = c;
        }
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Получен урон: {damage}. Осталось здоровья: {currentHealth}");

        // Вспышка красного экрана
        if (damageFlashImage != null)
        {
            damageFlashImage.color = flashColor;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Игрок умер! Перезагрузка сцены...");
        
        FirstPersonController controller = GetComponent<FirstPersonController>();
        if (controller != null)
        {
            controller.playerCanMove = false;
            controller.cameraCanMove = false;
        }
        
        if (damageFlashImage != null)
        {
            Color deathColor = flashColor;
            deathColor.a = 0.9f;
            damageFlashImage.color = deathColor;
        }
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {

        yield return new WaitForSeconds(restartDelay);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(90);
        }
    }
}