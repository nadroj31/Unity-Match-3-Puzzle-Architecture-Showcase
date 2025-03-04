using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject levelSelectUI;
    [SerializeField] private LevelButton levelButtonPrefab;
    [SerializeField] private Transform levelButtonParent;

    private readonly List<LevelButton> levelButtons = new List<LevelButton>();

    private void Awake()
    {
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(CloseLevelSelectMenu);
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(OpenLevelSelectMenu);
    }

    private void CloseLevelSelectMenu()
    {
        levelSelectUI.SetActive(false);
    }

    private void OpenLevelSelectMenu()
    {
        LevelData levelData = LevelData.Instance;
        levelData.LoadLevelData();

        List<int> levelKeys = levelData.GetAllLevelKeys();
        int keysCount = levelKeys.Count;
        int buttonsCount = levelButtons.Count;

        for (int i = 0; i < Mathf.Max(keysCount, buttonsCount); ++i)
        {
            if (i >= buttonsCount)
            {
                var levelButtonCopy = Instantiate(levelButtonPrefab, levelButtonParent);
                levelButtons.Add(levelButtonCopy.GetComponent<LevelButton>());
            }

            bool isActive = i < keysCount;
            levelButtons[i].gameObject.SetActive(isActive);
            if (isActive) 
            {
                levelButtons[i].SetLevelText(levelKeys[i]);
            }
        }

        levelSelectUI.SetActive(true);
    }
}
