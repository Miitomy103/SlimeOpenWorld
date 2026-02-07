using System;
using UnityEngine;
using UnityEngine.UI;

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
