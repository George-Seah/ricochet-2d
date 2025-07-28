using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    Scene currentScene;

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void InstantRestart()
    {
        currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex, LoadSceneMode.Single);
    }
    public void LoadSpecificScene(int scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
