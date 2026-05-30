using UnityEngine;

public class TurretPowerSwitch : MonoBehaviour, IInteractable
{
    [Header("References")]
    public TurretAI targetTurret;

    public string GetInteractionPrompt()
    {
        if (targetTurret == null) return "Турель не подключена";
        
        string action = targetTurret.isPowered ? "Выключить" : "Включить";
        return $"[E] {action} турель";
    }

    public void Interact()
    {
        if (targetTurret != null)
        {
            targetTurret.isPowered = !targetTurret.isPowered;
            Debug.Log($"Турель переключена: {(targetTurret.isPowered ? "ВКЛЮЧЕНА" : "ВЫКЛЮЧЕНА")}");
        }
    }
}