using UnityEngine;
using UnityEngine.SceneManagement;


public class BeattedButton : MonoBehaviour, IInteractable
{
    [SerializeField] private int idScene;
    public string GetInteractionPrompt()
    {
        return "Пройти игру";
    }

    public void Interact()
    {
        SceneManager.LoadScene(idScene);
    }
}
