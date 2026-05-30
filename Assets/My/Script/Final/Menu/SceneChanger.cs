using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(int id) => SceneManager.LoadScene(id);

    public void ResetProggres()
    {
        PlayerPrefs.SetString("CollectedCards", "");
        PlayerPrefs.Save();
    }

}
