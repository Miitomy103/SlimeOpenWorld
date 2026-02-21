using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class BossScene : MonoBehaviour
{
    [SerializeField] SkullBoss boss;
    float time;
    bool isBossDie;

    [SerializeField] CinemachineCamera bossCamera;

    [Header("UI")]
    [SerializeField] Canvas clearUI;
    [SerializeField] Text timeText;
    [SerializeField] Text hpText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject highScoreUI;
    private void Start()
    {
        QuestManager.Instance.StartQuest("quest_003");
        boss.OnDie += () =>
        {
            isBossDie = true;
            OnClear();
        };
    }

    private void Update()
    {
        if (isBossDie) return;

        time += Time.deltaTime;
    }

    public void OnClear()
    {
        Canvas[] canvas=GameObject.FindObjectsOfType<Canvas>();
        foreach (var c in canvas)
        {
            c.gameObject.SetActive(false);
        }


        clearUI.gameObject.SetActive(true);

        int totalSeconds = (int)time;

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timeText.text = $"{minutes:00}:{seconds:00}";

        int totalHp=(int)PlayerController.Instance.HostBase.CurrentHP;
        hpText.text = $"{totalHp}";

        int score = 10000 - (totalSeconds + totalHp);
        scoreText.text = $"{score}";

        if (time < PlayerPrefs.GetFloat("HighScore", score))
        {
            PlayerPrefs.SetFloat("HighScore", score);
            highScoreUI.SetActive(true);
        }
        else
        {
            highScoreUI.SetActive(false);
        }

        CameraManager.Instance.SetCinemachine(bossCamera);

        InputData.Instance.IsUsingUI = true;
    }
}
