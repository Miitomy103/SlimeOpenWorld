using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance =>instance;

    [SerializeField] HPVar hpVar;
    public HPVar HPVar => hpVar;

    private void Awake()
    {
        instance = this;
    }
}
