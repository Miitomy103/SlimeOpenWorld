// ================================================================================
// QuestTrigger.cs - クエストトリガー（場所到達用）
// ================================================================================
using UnityEngine;

public class QuestTrigger : MonoBehaviour,ILocation
{
    [SerializeField] private string locationID;
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered = false;

    public string LocationID => locationID;
    public Transform Transform => transform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject==PlayerController.Instance.HostBase.gameObject)
        {
            if (triggerOnce && hasTriggered) return;

            GameEvents.LocationReached(locationID);
            hasTriggered = true;

            Debug.Log($"Player reached location: {locationID}");
        }
    }
}