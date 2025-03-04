using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button levelButton;
    [SerializeField] private TextMeshProUGUI levelText;

    private int level;

    private void Awake()
    {
        levelButton.onClick.RemoveAllListeners();
        levelButton.onClick.AddListener(OpenGamePlayScene);
    }

    public void SetLevelText(int levelNumber)
    {
        levelText.text = $"Level {levelNumber}";
        level = levelNumber;
    }

    private void OpenGamePlayScene()
    {
        PlayerPrefs.SetInt("SelectLevel", level);
        ScenesManager.Instance.LoadGamePlayScene();
    }
}
