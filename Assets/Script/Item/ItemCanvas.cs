using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemCanvas : MonoBehaviour
{
    static ItemCanvas instance;
    public static ItemCanvas Instance =>instance;

    [SerializeField] GameObject itemPanel;
    [SerializeField] Text itemNameText;
    [SerializeField] Image itemIconImage;

    private void Awake()
    {
        instance = this;
    }
    public void GetItem(ItemData itemData)
    {

        StartCoroutine(ShowItemCoroutine(itemData));
    }

    /// <summary>
    /// アイテム取得パネルを表示し、時間停止とクリック待ちを行ってから閉じる。
    /// </summary>
    IEnumerator ShowItemCoroutine(ItemData itemData)
    {
        yield return new WaitForSeconds(0.2f);
        InputData.Instance.IsUsingUI = true;

        Time.timeScale = 0f;

        itemPanel.SetActive(true);
        itemNameText.text = itemData.ItemName;
        itemIconImage.sprite = itemData.Icon;

        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        InputData.Instance.IsUsingUI = false;
        Time.timeScale = 1f;

        itemPanel.SetActive(false);
    }
}
