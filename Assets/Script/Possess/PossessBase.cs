using UnityEngine;

public abstract class PossessBase : MonoBehaviour, IPossess
{
    public Transform Transform => transform;

    public virtual bool CanPossess => true;

    [SerializeField]string possessId = "DefaultID";
    public virtual string PossessId =>possessId;

    [SerializeField] HostBase hostPrefab;
    public virtual HostBase GetHost()
    {
        HostBase hostInstance = Instantiate(hostPrefab, Transform.position, Transform.rotation);
        gameObject.SetActive(false);
        return hostInstance;
    }
}
