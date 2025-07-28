using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] float restartCountdown = 3f;
    InputActionAsset inputActionAsset;

    CanvasScript canvas;
    GameObject[] targets;
    Scene currentScene;
    int currentSceneIndex;

    void Awake()
    {
        // —– Singleton enforcement —–
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // —– initialize —–
        currentScene = SceneManager.GetActiveScene();
        currentSceneIndex = currentScene.buildIndex;
        targets = GameObject.FindGameObjectsWithTag("Target");
        inputActionAsset = FindFirstObjectByType<PlayerInput>().actions;
        
    }

    void GetActiveScene()
    {
        currentScene = SceneManager.GetActiveScene();
        currentSceneIndex = currentScene.buildIndex;
    }
    public void GetTargetCount()
    {
        targets = null;
        targets = GameObject.FindGameObjectsWithTag("Target");
        GetActiveScene();
        if (targets.Length <= 0)
        {
            SceneManager.LoadScene(currentSceneIndex + 1, LoadSceneMode.Single);
        }
    }

    public IEnumerator RestartLevel()
    {
        Debug.Log("Should be restarting.");
        GetActiveScene();
        yield return new WaitForSeconds(restartCountdown);
        SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
    }
    public void StartRestartCountdown()
    {
        StartCoroutine(RestartLevel());
    }
    public void Pause()
    {
        canvas = FindFirstObjectByType<CanvasScript>(FindObjectsInactive.Include);
        canvas.Pause();
    }
}
