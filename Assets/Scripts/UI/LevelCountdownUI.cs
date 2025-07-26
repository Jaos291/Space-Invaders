using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelCountdownUI : MonoBehaviour
{
    // --- Serialized fields ---
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private float countdownTime = 1f;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private TextMeshProUGUI victoryScoreText;

    // --- Private fields ---
    private Coroutine countdownCoroutine;

    // --- Unity event methods ---
    // Subscribe to events
    private void OnEnable()
    {
        EnemySpawner.OnLevelChanged += OnLevelChanged;
        PlayerController.OnPlayerDied += OnPlayerDied;
    }

    // Unsubscribe from events
    private void OnDisable()
    {
        EnemySpawner.OnLevelChanged -= OnLevelChanged;
        PlayerController.OnPlayerDied -= OnPlayerDied;
    }

    // --- Event handlers ---
    // Called when level changes, triggers countdown or victory
    private void OnLevelChanged(int level)
    {
        if (level > GameManager.Instance.LevelConfig.Length)
        {
            int score = 0;
            if (GameManager.Instance.player != null)
            {
                var playerController = GameManager.Instance.player.GetComponent<PlayerController>();
                if (playerController != null)
                    score = playerController.score;
            }
            ShowVictoryScreen(score);
            return;
        }
        GameManager.Instance.canPlay = false;
        StartCountdown();
    }

    // Called on player death, shows game over UI
    private void OnPlayerDied()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }

    // --- Public methods ---
    // Show victory UI and score
    public void ShowVictoryScreen(int score)
    {
        if (victoryScreen != null)
            victoryScreen.SetActive(true);
        if (victoryScoreText != null)
            victoryScoreText.text = score.ToString();
    }

    // Public trigger for countdown (e.g. from button)
    public void StartCountdownForUI()
    {
        StartCountdown();
    }

    // Start countdown with optional callback
    public void StartCountdown(System.Action onComplete = null)
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
        countdownCoroutine = StartCoroutine(CountdownRoutine(onComplete));
    }

    // --- Countdown coroutine ---
    // Coroutine for countdown display and canPlay control
    private IEnumerator CountdownRoutine(System.Action onComplete)
    {
        backgroundObject.SetActive(true);
        countdownText.gameObject.SetActive(true);
        GameManager.Instance.canPlay = false;
        float timer = countdownTime;
        while (timer > 0)
        {
            countdownText.text = Mathf.CeilToInt(timer).ToString();
            yield return null;
            timer -= Time.deltaTime;
        }
        countdownText.gameObject.SetActive(false);
        backgroundObject.SetActive(false);
        GameManager.Instance.canPlay = true;
        onComplete?.Invoke();
    }
}
