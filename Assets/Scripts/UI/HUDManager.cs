using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Entities;

/// <summary>
/// Manages UI elements in the separate UIScene.
/// Handles main menu, game over popup display, and scene navigation.
/// Subscribes to GameEventBridge events for communication with ECS systems.
/// </summary>
public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPopup;
    [SerializeField] private UnityEngine.UI.Button startGameButton;
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private UnityEngine.UI.Button goBackToMenuButton;

    private void Awake()
    {
        ShowMenu();
        HideGameOverPopup();
    }
    
    private void Start()
    {
        // Ensure menu is shown and game over popup is hidden on scene load/reload
        ShowMenu();
        HideGameOverPopup();
    }
    
    private void ShowMenu()
    {
        if (menuPopup != null)
        {
            menuPopup.SetActive(true);
        }
    }
    
    private void HideMenu()
    {
        if (menuPopup != null)
        {
            menuPopup.SetActive(false);
        }
    }
    
    private void HideGameOverPopup()
    {
        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GameEventBridge.OnGameOver += ShowGameOver;

        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(StartGame);
        }

        if (goBackToMenuButton != null)
        {
            goBackToMenuButton.onClick.AddListener(GoBackToMenu);
        }
    }

    private void OnDisable()
    {
        GameEventBridge.OnGameOver -= ShowGameOver;
        
        if (startGameButton != null)
        {
            startGameButton.onClick.RemoveListener(StartGame);
        }
        
        if (goBackToMenuButton != null)
        {
            goBackToMenuButton.onClick.RemoveListener(GoBackToMenu);
        }
    }

    /// <summary>
    /// Hides the menu and starts the game by setting IsGameStarted flag.
    /// </summary>
    private void StartGame()
    {
        HideMenu();
        
        var world = World.DefaultGameObjectInjectionWorld;
        if (world != null && world.IsCreated)
        {
            var entityManager = world.EntityManager;
            var gameStateQuery = entityManager.CreateEntityQuery(typeof(GameState));
            if (gameStateQuery.HasSingleton<GameState>())
            {
                var gameStateEntity = gameStateQuery.GetSingletonEntity();
                var gameState = entityManager.GetComponentData<GameState>(gameStateEntity);
                gameState.IsGameStarted = true;
                entityManager.SetComponentData(gameStateEntity, gameState);
            }
        }
    }

    private void ShowGameOver(int finalScore)
    {
        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true);
        }
        
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {finalScore}";
        }
    }

    /// <summary>
    /// Reloads the active game scene to return to menu and restart the game.
    /// </summary>
    private void GoBackToMenu()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }
}
