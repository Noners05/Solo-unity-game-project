using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class DeathEvent : MonoBehaviour
{
    public GameObject container;
    public static DeathEvent instance;

    private void Awake()
    {
        instance = this;
    }

    public void EnableDeathScreen()
    {
        container.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
