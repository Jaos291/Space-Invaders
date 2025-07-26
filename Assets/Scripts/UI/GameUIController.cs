using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetPlayerSprite(Sprite sprite)
    {
        playerSprite.sprite = sprite;
    }

    public void SetLives(int lives)
    {
        livesText.text = lives.ToString();
    }

    public void SetLevel(int level)
    {
        levelText.text = level.ToString();
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
