using UnityEngine;

/// <summary>
/// プレイヤーがインタラクト可能なオブジェクトを検出し、インタラクトUIを表示するクラス
/// </summary>
public class Interact : MonoBehaviour
{
    [SerializeField] InteractTextControl interactTextControl;

    [SerializeField]float detectRadius = 5f;

    PossessRange possessRange;

    private void Awake()
    {
        possessRange = GetComponent<PossessRange>();
    }

    private void Update()
    {
        if(interactTextControl == null || possessRange == null)
        {
            return;
        }

        IInteractable[] nearestEnemies = possessRange.DetectEnemies<IInteractable>();

        IInteractable nearestEnemy = null;

        float nearestDist = Mathf.Infinity;
        foreach (var enemy in nearestEnemies)
        {
            if (!enemy.CanInteract) continue;
            float dist = Vector3.Distance(possessRange.PositionTransform.transform.position, ((MonoBehaviour)enemy).transform.position);
            if (dist < nearestDist && dist <= possessRange.detectRadius)
            {
                nearestDist = dist;
                nearestEnemy = enemy;
            }
        }


        if (nearestEnemy != null)
        {
            interactTextControl.EnterInteract(nearestEnemy, gameObject);
        }
        else
        {
            interactTextControl.ExitInteract();
        }
    }
}
