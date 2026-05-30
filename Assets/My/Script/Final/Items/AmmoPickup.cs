using UnityEngine;

public class AmmoPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private int ammoAmount = 6;
    [SerializeField] private WeaponShooting weaponShooting;

    public string GetInteractionPrompt()
    {
        return $"Патроны ({ammoAmount} шт) - Нажмите [E]";
    }

    public void Interact()
    {
        if (weaponShooting != null)
        {
            weaponShooting.AddAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}