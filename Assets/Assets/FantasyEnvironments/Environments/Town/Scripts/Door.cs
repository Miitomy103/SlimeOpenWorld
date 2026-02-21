using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField] Transform rightDoor;
    [SerializeField] Transform leftDoor;
    [SerializeField] float duration = 1f;

    private void Awake()
    {
        if (rightDoor == null || leftDoor == null)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if(i==0) rightDoor = child;
                else if (i == 1) leftDoor = child;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject==PlayerController.Instance.HostBase.gameObject)
        {
            Debug.Log("Player entered door trigger");
            Vector3 toTarget = (other.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toTarget);

            if (dot > 0)
                MoveDoor(new Vector2(-95f, 95f));
            else
                MoveDoor(new Vector2(95f, -95f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.HostBase.gameObject)
        {
            MoveDoor(Vector2.zero);
        }
    }

    public void MoveDoor(Vector2 rotation)
    {
        StartCoroutine(MoveDoorCoroutine(rotation));
    }

    IEnumerator MoveDoorCoroutine(Vector2 rotation)
    {
        Debug.Log("Starting door movement coroutine with rotation: " + rotation);
        float elapsed = 0f;

        Quaternion rightStartRot = rightDoor.localRotation;
        Quaternion leftStartRot = leftDoor.localRotation;

        Quaternion rightEndRot = Quaternion.Euler(0f, rotation.x, 0f);
        Quaternion leftEndRot = Quaternion.Euler(0f, rotation.y, 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            rightDoor.localRotation = Quaternion.Lerp(rightStartRot, rightEndRot, t);
            leftDoor.localRotation = Quaternion.Lerp(leftStartRot, leftEndRot, t);

            yield return null;
        }

        rightDoor.localRotation = rightEndRot;
        leftDoor.localRotation = leftEndRot;
    }
}