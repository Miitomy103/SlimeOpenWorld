// ================================================================================
// QuestTrigger.cs - クエストトリガー（場所到達用）
// ================================================================================
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private string locationID;
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerOnce && hasTriggered) return;

            GameEvents.LocationReached(locationID);
            hasTriggered = true;

            Debug.Log($"Player reached location: {locationID}");
        }
    }
}