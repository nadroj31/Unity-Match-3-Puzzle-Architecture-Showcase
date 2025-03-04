using DG.Tweening;
using System;
using UnityEngine;

public class BrickShow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Brick brick;
    private Action<Brick> onClick;

    public void SetData(Brick brick)
    {
        this.brick = brick;
        transform.localPosition = brick.Position;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetOnClickAction(Action<Brick> action)
    {
        onClick = action;
    }

    public void OnMouseUp()
    {
        onClick?.Invoke(brick);
    }

    public BrickShow TweenMove(float originPosY, float targetPosY)
    {
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(transform.localPosition.x, originPosY);
        transform.DOMoveY(targetPosY, 0.3f).SetEase(Ease.InOutQuad).SetDelay(0.01f);

        return this;
    }
}
