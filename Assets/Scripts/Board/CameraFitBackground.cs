using UnityEngine;

/// <summary>
/// Scales this GameObject's SpriteRenderer to exactly fill the orthographic camera's
/// visible area, so the background always covers the full screen regardless of resolution
/// or board size.
///
/// Attach to the background SpriteRenderer GameObject.
/// Runs in Start() so it executes after GamePlayBoard.Awake() has already called
/// AdjustCamera() and set the final orthographic size for the current level.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class CameraFitBackground : MonoBehaviour
{
    [Tooltip("The camera to fit. Defaults to Camera.main if left empty.")]
    [SerializeField] private Camera targetCamera;

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (targetCamera == null)
        {
            Debug.LogWarning("[CameraFitBackground] No camera found.", this);
            return;
        }

        var sr = GetComponent<SpriteRenderer>();
        if (sr.sprite == null)
        {
            Debug.LogWarning("[CameraFitBackground] SpriteRenderer has no sprite assigned.", this);
            return;
        }

        // For an orthographic camera: visible height = 2 × orthographicSize (world units)
        float camHeight  = 2f * targetCamera.orthographicSize;
        float camWidth   = camHeight * targetCamera.aspect;

        Vector2 spriteSize = sr.sprite.bounds.size;
        transform.localScale = new Vector3(
            camWidth  / spriteSize.x,
            camHeight / spriteSize.y,
            1f);

        // Centre on the camera.
        transform.position = new Vector3(
            targetCamera.transform.position.x,
            targetCamera.transform.position.y,
            transform.position.z);
    }
}
