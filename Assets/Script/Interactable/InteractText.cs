using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// インタラクト対象の近くに表示する操作テキスト。プールして再利用する。
/// </summary>
public class InteractText : PoolObject
{
    [SerializeField] Text uiText;
    [SerializeField] Button button;

    IInteractable interactable;
    public RectTransform RectTransform => (RectTransform)transform;

    GameObject player;

    bool isEnable = true;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }
    private void Update()
    {
        if(!isEnable) return;
        PlayerInput input = InputData.Instance.InputAction();
        if(input.Interact.onDown)
        {
            OnClick();
        }
    }
    /// <summary>
    /// 表示する対象のインタラクト内容をセットする。
    /// </summary>
    public void SetText(IInteractable interactable,GameObject p)
    {
        this.interactable = interactable;
        uiText.text = interactable.GetInteractText;
        player = p;
    }
    void OnClick()=>interactable.Interact(player);

    public void Selected()=> interactable.Interact(player);

   public override void DoDisable()
    {
        base.DoDisable();
        isEnable = false;
    }
    private void OnEnable()
    {
        isEnable = true;
    }
}
