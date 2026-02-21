using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class HostBase : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;

    public CharacterController controller { get; private set; }
    public Transform CameraTarget => cameraTarget;
    public PlayerInput Input => InputData.Instance.InputAction();
    public HostBase Possessor { get; set; }

    /// <summary>
    /// のっとり中かどうか
    /// </summary>
    public bool isPossessed;
    [SerializeField]float currentHp = 100f;
    public float CurrentHP => currentHp;
    [SerializeField] float maxHp = 100f;
    public float MaxHp => maxHp;

    protected virtual void Awake()
    {
        if (controller == null) controller = GetComponentInChildren<CharacterController>();
    }
    public void Damage(float damage)
    {
        Debug.Log($"{gameObject.name}は{damage}のダメージを受けた");
        currentHp -= damage;
        if (currentHp <= 0f)
        {
            currentHp = 0f;
            Die();
        }
        UIManager.Instance.HPVar.SetHPVar(currentHp, maxHp);
    }
    public virtual void Die()
    {
        // デフォルトの死亡処理
        gameObject.SetActive(false);
        ForceHostBack();
    }
    public virtual void HandleInput(PlayerInput input)
    {
        //Input = input;
    }

    public virtual void StartHost(HostBase ago)
    {
        Possessor = ago;
        gameObject.SetActive(true);
        StartCoroutine(HostStart());
        UIManager.Instance?.HPVar.SetHPVar(currentHp, maxHp);
    }

    public virtual void EndHost()
    {
        isPossessed = false;
    }

    public virtual void UpdateHost()
    {
        HostBack();
        Velocity();
    }


    protected virtual void Velocity()
    {
        Vector3 gravity = Physics.gravity * Time.deltaTime;
        if (controller != null&&controller.enabled&& gameObject.activeInHierarchy)
        {
            controller.Move(gravity);
        }
    }

    public void HostBack()
    {
        if (Possessor == null||this is SlimeHost||!isPossessed) return;

        PlayerInput input = InputData.Instance.InputAction();

        if(input.Button3.onButton)
        {
            ForceHostBack();
        }
    }

    public void ForceHostBack()
    {
        if (Possessor == null) return;
        PlayerController.Instance.SetHost(Possessor, this);
        Possessor = null;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    IEnumerator HostStart()
    {
        yield return new WaitForSeconds(0.5f);
        isPossessed = true;
    }
}
