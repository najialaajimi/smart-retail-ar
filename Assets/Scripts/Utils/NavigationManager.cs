using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    private static NavigationManager instance;
    public static NavigationManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("NavigationManager");
                instance = go.AddComponent<NavigationManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private string previousScene = "";
    private string currentScene = "";

    void Awake()
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

    public void NavigateToScene(string sceneName, float delay = 0f)
    {
        if (delay > 0)
        {
            StartCoroutine(NavigateWithDelay(sceneName, delay));
        }
        else
        {
            LoadScene(sceneName);
        }
    }

    private IEnumerator NavigateWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadScene(sceneName);
    }

    private void LoadScene(string sceneName)
    {
        previousScene = currentScene;
        currentScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void NavigateToHome()
    {
        NavigateToScene("HomeScene");
    }

    public void NavigateToScanner()
    {
        NavigateToScene("ScannerScene");
    }

    public void NavigateToProductInfo()
    {
        NavigateToScene("ProductInfoScene");
    }

    public void NavigateToRecommendations()
    {
        NavigateToScene("RecommendationsScene");
    }

    public void NavigateToProfile()
    {
        NavigateToScene("ProfileScene");
    }

    public void NavigateBack()
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            string temp = currentScene;
            currentScene = previousScene;
            previousScene = temp;
            SceneManager.LoadScene(currentScene);
        }
        else
        {
            NavigateToHome();
        }
    }

    public void QuitApplication()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public string GetCurrentScene()
    {
        return currentScene;
    }

    public string GetPreviousScene()
    {
        return previousScene;
    }
}
