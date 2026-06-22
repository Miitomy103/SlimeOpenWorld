using UnityEngine;

/// <summary>
/// 現在最も近いインタラクト対象に対して、操作テキストの表示/非表示を切り替える。
/// </summary>
public class InteractTextControl : MonoBehaviour
{
    [SerializeField] InteractText interactTextPrefab;

    //[SerializeField] ObjectPool<InteractText> textPool;

    [Header("TextSettings")]
    [SerializeField] Vector2 textPos;
    [SerializeField] Vector2 offset = Vector2.one;

    IInteractable currentInteractable;
    InteractText interactText;

    //[SerializeField] Dictionary<IInteractable, InteractText> interactTexts = new Dictionary<IInteractable, InteractText>();


    private void Awake()
    {
        //textPool = new ObjectPool<InteractText>(interactTextPrefab, 1,transform);
    }

    private void Start()
    {
        //interactText= Instantiate(interactTextPrefab,textPos,Quaternion.identity, transform);
        interactText=interactTextPrefab;
        interactText.gameObject.SetActive(false);
    }

    public void EnterInteract(IInteractable interactable, GameObject player)
    {
        if(currentInteractable == interactable) return;

        currentInteractable = interactable;
        interactText.SetText(interactable, player);
        interactText.gameObject.SetActive(true);
    }

    public void ExitInteract()
    {
        currentInteractable = null;
        interactText.gameObject.SetActive(false);
    }

}
