using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingUI;

    private static ScenesManager instance;
    public static ScenesManager Instance => instance = instance != null ? instance : FindObjectOfType<ScenesManager>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadGamePlayScene()
    {
        StartCoroutine(LoadAsyncScene("GamePlayScene"));
    }

    private IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        loadingUI.SetActive(true);

        yield return new WaitUntil(() => asyncLoad.isDone);
        yield return new WaitForSeconds(0.5f);

        loadingUI.SetActive(false);
    }
}
