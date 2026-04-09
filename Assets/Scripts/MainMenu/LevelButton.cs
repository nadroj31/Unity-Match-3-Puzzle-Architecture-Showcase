using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Level-select button. Stores the chosen level number in <see cref="GameSession"/>
/// before loading the gameplay scene via the injected <see cref="ISceneNavigator"/>.
/// </summary>
public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button          levelButton;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameSession     gameSession;

    [Header("Gem Icon")]
    [Tooltip("The circular gem image on the left of the button. Sprite cycles through gemCycleSprites based on level number.")]
    [SerializeField] private Image    gemIcon;
    [Tooltip("Sprites cycled by level number: index 0 = level 1, 4, 7 … · index 1 = level 2, 5, 8 … · index 2 = level 3, 6, 9 …")]
    [SerializeField] private Sprite[] gemCycleSprites = new Sprite[3];

    private int              level;
    private ISceneNavigator  sceneNavigator;

    private void Awake()
    {
        levelButton.onClick.RemoveAllListeners();
        levelButton.onClick.AddListener(OpenGamePlayScene);
    }

    /// <summary>
    /// Injects the scene navigator used when this button is clicked.
    /// Must be called by the owner (e.g. <see cref="RecycledScrollView"/>) after instantiation.
    /// </summary>
    public void SetNavigator(ISceneNavigator navigator) => sceneNavigator = navigator;

    /// <summary>Configures the displayed level number for this button and updates the gem icon sprite.</summary>
    public void SetLevelText(int levelNumber)
    {
        levelText.text = $"Level {levelNumber}";
        level          = levelNumber;

        if (gemIcon != null && gemCycleSprites != null && gemCycleSprites.Length > 0)
            gemIcon.sprite = gemCycleSprites[(levelNumber - 1) % gemCycleSprites.Length];
    }

    private void OpenGamePlayScene()
    {
        // sceneNavigator is stored as ISceneNavigator (interface); the underlying
        // MonoBehaviour may have been destroyed without the C# reference becoming null.
        // Cast to UnityEngine.Object to use Unity's overloaded == operator, which
        // correctly detects destroyed instances.
        ISceneNavigator nav = sceneNavigator;
        if (nav is Object navObj && navObj == null)
            nav = ScenesManager.Instance;

        if (nav == null)
        {
            Debug.LogError("[LevelButton] No valid ISceneNavigator found.", this);
            return;
        }

        gameSession.SelectedLevel = level;
        nav.LoadGamePlayScene();
    }
}
