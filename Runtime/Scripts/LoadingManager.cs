using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    [Header("UI Setting")]
    [SerializeField] private Image fillBar;
    [SerializeField] private TextMeshProUGUI percentText;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        canvasGroup = GetComponent<CanvasGroup>();
        SetCanvasGroup(0f, false, false);
    }

    public void LoadingToChangeScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        StartCoroutine(LoadingScene(sceneName, loadSceneMode));
    }

    private void SetCanvasGroup(float alpha, bool interactable, bool blocksRaycasts)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = blocksRaycasts;
    }

    // Loading to change scene and customizable
    private IEnumerator LoadingScene(string sceneName, LoadSceneMode loadSceneMode)
    {
        SetCanvasGroup(1, true, true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

        while (fillBar.fillAmount < 0.99f)
        {
            var progress = asyncOperation.progress;
            fillBar.fillAmount = progress;
            percentText.text = (progress * 100).ToString("F0") + "%";

            yield return null;
        }

        Debug.Log("Success");

        StartCoroutine(CloseLoadingCanvas());
    }

    private IEnumerator CloseLoadingCanvas()
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= 0.01f;

            yield return null;
        }

        Debug.Log("Close");

        SetCanvasGroup(0f, false, false);
    }
}
