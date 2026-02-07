using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            interactText.Selected();
        }
    }

    //ï°êîTextÇèoÇ∑èàóù

    //public void Fire(IInteractable interactable,GameObject player)
    //{
    //    if (interactTexts.ContainsKey(interactable))
    //    {
    //        Debug.Log("NotContain");
    //        return; // ìÒèdìoò^ñhé~
    //    }
    //    Debug.Log("Fire" + interactable.GetInteractText());

    //    var blt = textPool.EnableToPoolObject();
    //    interactTexts.Add(interactable, blt);
    //    blt.SetText(interactable,player);
    //}

    //public void ExitInteract(IInteractable interactable)
    //{
    //    if (!interactTexts.TryGetValue(interactable, out var pool))
    //        return;

    //    textPool.ReturnToPoolObject(pool);
    //    pool.gameObject.SetActive(false);
    //    interactTexts.Remove(interactable);
    //}

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
