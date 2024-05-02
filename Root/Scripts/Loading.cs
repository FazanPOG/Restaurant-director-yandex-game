using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class Loading : MonoBehaviour
{
    private const string GAME_SCENE_NAME = "Game";

    [SerializeField] private Animation _loadingAnimation;
    [SerializeField] private TextMeshProUGUI _debugText;

    private void Awake()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        _loadingAnimation.Play();

#if UNITY_WEBGL && !UNITY_EDITOR
        yield return new WaitUntil(() => YandexGame.SDKEnabled);
#else
        yield return new WaitForSeconds(3f);
#endif

        LoadGameScene();
    }

    private void LoadGameScene() 
    {
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }
}
