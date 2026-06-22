using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 進捗バーとブラックアウトを表示しつつ、SceneNameStaticで指定されたシーン
/// (未指定時はdefaultSceneName)を非同期で読み込むブートローダー。
/// </summary>
public class BootLoader : MonoBehaviour
{
    [SerializeField] string defaultSceneName = "TitleScene";
    [SerializeField] GameObject iSlider;

    [Header("ロード完了までの待機時間")]
    [SerializeField] float waitLoadDelay;
    [SerializeField] AnimationCurve progressCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Header("ブラックアウト")]
    [SerializeField] Image blackOutImage;
    [SerializeField] bool useBlackOut;
    [SerializeField] float blackOutDuration = 1f;

    IEnumerator Start()
    {
        yield return null;

        ISlider progressBar = iSlider?.GetComponent<ISlider>();

        string sceneName =
            string.IsNullOrEmpty(SceneNameStatic.SceneName)
                ? defaultSceneName
                : SceneNameStatic.SceneName;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            timer += Time.unscaledDeltaTime;

            // 実ロード進捗（0〜1）
            float loadProgress = Mathf.Clamp01(op.progress / 0.9f);

            // 時間進捗（0〜1）
            float timeProgress = waitLoadDelay > 0f
                ? Mathf.Clamp01(timer / waitLoadDelay)
                : 1f;

            // カーブ適用
            float curvedProgress = progressCurve.Evaluate(timeProgress);

            // 表示用進捗（ロードが追いつくまで待つ）
            float displayProgress = Mathf.Min(loadProgress, curvedProgress);



            // 両方完了で遷移
            if (loadProgress >= 1f && timeProgress >= 1f)
            {
                if (useBlackOut)
                {
                    yield return BlackOut();
                }
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    IEnumerator BlackOut()
    {
        if (blackOutImage == null) yield break;
        float timer = 0f;
        Color color = blackOutImage.color;
        while (timer < blackOutDuration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(timer / blackOutDuration);
            blackOutImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }
}
