using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// Bootstraps the game by loading the UIScene additively.
    /// Disables camera rendering until UI is loaded to prevent showing gameplay before menu.
    /// </summary>
    public class GameBootstrap : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        // Disable camera rendering until UI is loaded
        _mainCamera = Camera.main;
        if (_mainCamera != null)
        {
            _mainCamera.enabled = false;
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (!SceneManager.GetSceneByName("UIScene").isLoaded)
        {
            SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
        }
        else
        {
            // If UI is already loaded, enable camera immediately
            EnableCamera();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Enable camera when UIScene is loaded
        if (scene.name == "UIScene")
        {
            EnableCamera();
        }
    }

    private void EnableCamera()
    {
        if (_mainCamera != null)
        {
            _mainCamera.enabled = true;
        }
    }
}
}
