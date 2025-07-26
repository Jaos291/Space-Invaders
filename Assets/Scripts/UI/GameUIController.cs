using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerController.OnLivesChanged += SetLives;
        PlayerController.OnScoreChanged += SetScore;
        EnemySpawner.OnLevelChanged += SetLevel;
    }

    private void OnDisable()
    {
        PlayerController.OnLivesChanged -= SetLives;
        PlayerController.OnScoreChanged -= SetScore;
        EnemySpawner.OnLevelChanged -= SetLevel;
    }
    [SerializeField] private Image playerSprite;
    [SerializeField] private Text livesText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text scoreText;

    public void SetPlayerSprite(Sprite sprite)
    {
        playerSprite.sprite = sprite;
    }

    public void SetLives(int lives)
    {
        livesText.text = $"Lives: {lives}";
    }

    public void SetLevel(int level)
    {
        levelText.text = $"Level: {level}";
    }

    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}
