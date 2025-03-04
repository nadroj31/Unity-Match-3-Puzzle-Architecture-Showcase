using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayBoard : MonoBehaviour
{
    [SerializeField] private SpriteRenderer boardBackground;
    [SerializeField] private Transform bricksParent;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int minMatchCount = 2;
    [SerializeField] private Image goal;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private Button goBackMainMenuButton;

    private const float BackgroundOffset = 0.3f;
    private const float DropHeightOffset = 2.3f;

    private LevelDetails levelDetails;
    private Brick[,] bricks;
    private BrickShow[,] brickShows;
    private List<BrickShow> matchBrickShows;
    private bool isFillingBoard;
    private bool isVictory = false;
    private BrickType goalType;
    private int goalCount;

    private void Awake()
    {
        goBackMainMenuButton.onClick.RemoveAllListeners();
        goBackMainMenuButton.onClick.AddListener(GoBackToMainMenu);
        levelDetails = LevelData.Instance.GetLevelDetails(PlayerPrefs.GetInt("SelectLevel"));
        Initialize();
    }

    private void Initialize()
    {
        boardBackground.size = new Vector2(levelDetails.gridWidth + BackgroundOffset, levelDetails.gridHeight + BackgroundOffset);
        bricks = new Brick[levelDetails.gridWidth, levelDetails.gridHeight];
        brickShows = new BrickShow[levelDetails.gridWidth, levelDetails.gridHeight];
        matchBrickShows = new List<BrickShow>();

        PopulateBricks();
        SetGoal();
    }

    private void PopulateBricks()
    {
        for (int i = 0; i < levelDetails.gridWidth; ++i)
        {
            for (int j = 0; j < levelDetails.gridHeight; ++j)
            {
                var brick = new Brick(i, j, levelDetails.gridData[i, j]);
                brick.SetPosition(levelDetails.gridWidth, levelDetails.gridHeight);
                bricks[i, j] = brick;

                BrickShow brickShow = BrickFactory.Instance.CreateBrick(brick, bricksParent);
                if (brickShow != null)
                {
                    brickShow.SetSprite(sprites[(int)brick.BrickType - 1]);
                    brickShow.SetOnClickAction(OnBrickClick);
                    brickShows[i, j] = brickShow;
                }
            }
        }
    }

    private void SetGoal()
    {
        goalType = levelDetails.goal switch
        {
            "b" => BrickType.BLUE_BRICK,
            "g" => BrickType.GREEN_BRICK,
            "y" => BrickType.YELLOW_BRICK,
            "r" => BrickType.RED_BRICK,
            _ => BrickType.RANDOM_BRICK,
        };

        goal.sprite = sprites[(int)goalType - 1];
        goalCount = levelDetails.goalNumber;
        goalText.text = goalCount.ToString();
    }

    private void OnBrickClick(Brick clickedBrick)
    {
        if (isFillingBoard || isVictory)
        {
            return;
        }

        var matchBricks = GamePlayBoardHandler.Instance.FindMatchBricks(clickedBrick, bricks);
        if (matchBricks.Count < minMatchCount)
        {
            return;
        }

        isFillingBoard = true;
        ProcessMatchedBricks(matchBricks);
    }

    private void ProcessMatchedBricks(List<Brick> matchBricks)
    {
        BrickType matchBrickType = matchBricks[0].BrickType;

        foreach (var brick in matchBricks)
        {
            var brickShow = brickShows[brick.X, brick.Y];
            matchBrickShows.Add(brickShow);
            brickShow.transform.localScale = Vector3.zero;
        }

        if (isFillingBoard)
        {
            GamePlayBoardHandler.Instance.FillingBoard(matchBricks, OnFillingBoard);
            UpdateGoal(matchBrickType, matchBricks.Count);
            StartCoroutine(ResetFillingState());
        }
    }

    private IEnumerator ResetFillingState()
    {
        yield return new WaitForSeconds(0.4f);
        isFillingBoard = false;
    }

    private void OnFillingBoard(Brick fromBrick, Brick toBrick)
    {
        var bottomBrickShow = brickShows[toBrick.X, toBrick.Y];
        bottomBrickShow.transform.localScale = Vector3.one;
        bottomBrickShow.SetSprite(sprites[(int)(fromBrick?.BrickType ?? toBrick.BrickType) - 1]);
        bottomBrickShow.TweenMove(fromBrick?.Position.y ?? levelDetails.gridHeight / 2 + DropHeightOffset, toBrick.Position.y);
    }

    private void UpdateGoal(BrickType brickType, int matchCount)
    {
        if (goalType == BrickType.RANDOM_BRICK || goalType == brickType)
        {
            goalCount = Mathf.Max(goalCount - matchCount, 0);
            goalText.text = goalCount.ToString();

            if (goalCount <= 0)
            {
                isVictory = true;
                OpenVictoryScreen();
            }
        }
    }

    private void OpenVictoryScreen()
    {
        victoryUI.SetActive(true);
    } 

    private void GoBackToMainMenu()
    {
        DOTween.KillAll();
        ScenesManager.Instance.LoadMainMenu();
    }
}

