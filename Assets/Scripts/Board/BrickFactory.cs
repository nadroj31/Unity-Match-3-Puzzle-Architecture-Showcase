using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This part is the brick factory.
/// Different bricks will be generated according to different methods.
/// New types of bricks can be freely expanded.
/// </summary>
public class BrickFactory : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;

    private static BrickFactory instance;
    public static BrickFactory Instance => instance ??= FindObjectOfType<BrickFactory>();

    private static readonly Dictionary<BrickType, Func<Brick, GameObject, BrickShow>> brickCreators = new Dictionary<BrickType, Func<Brick, GameObject, BrickShow>>
    {
        { BrickType.BLUE_BRICK, CreateColorBrick },
        { BrickType.GREEN_BRICK, CreateColorBrick },
        { BrickType.YELLOW_BRICK, CreateColorBrick },
        { BrickType.RED_BRICK, CreateColorBrick }
    };

    public BrickShow CreateBrick(Brick brick, Transform parent)
    {
        if (brick.BrickType == BrickType.NONE) 
        {
            return null;
        }

        var brickObject = Instantiate(brickPrefab, parent);
        brickObject.name = $"Brick({brick.X},{brick.Y})";
        if (brickCreators.TryGetValue(brick.BrickType, out var createBrick))
        {
            return createBrick(brick, brickObject);
        }

        Debug.LogError("Can't create this kind of brick: " + brick.BrickType);
        return null;
    }

    private static BrickShow CreateColorBrick(Brick brick, GameObject brickObject)
    {
        BrickShow brickShow = brickObject.GetComponent<BrickShow>();
        brickShow.SetData(brick);
        return brickShow;
    }
}
