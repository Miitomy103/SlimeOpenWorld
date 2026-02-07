using UnityEngine;
using StateMachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkullBoss : MonoBehaviour,IIsAutoRotator
{
    [Header("Parameters")]
    [SerializeField] int maxHP = 100;
    [SerializeField,ReadOnly] int currentHP;

    public bool IsAutoRotator { get; set; } = true;
    public bool IsGravity {get;set;} = true;

    [SerializeField] float rotateSpeed = 1f;
    [SerializeField]float velocity;
    [Header("Referents")]
    [SerializeField] CharacterController controller;
    [SerializeField] Slider hpSlider;

    Transform playerTransform => PlayerController.Instance.HostBase.transform;

    #region State Machine
    /// <summary>
    /// 
    /// </summary>
    protected StateMachine<string> stateMachine;

    /// <summary>
    /// 
    /// </summary>
    protected StateBase<string>[] states;

    /// <summary>
    /// 
    /// </summary>
    /// 
    protected StateBase<string>[] CreateStates()
    { 
        return GetComponents<StateBase<string>>();
    }

    private void InitializeStateMachie()
    {
        states = CreateStates();

        stateMachine = new StateMachine<string>();
        stateMachine.RegisterState(states);
        foreach (var state in states)
        {
            state.InitializeState();
        }

        stateMachine.Transition(states[0].Key);
    }

    #endregion

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    protected virtual void Start()
    {
        InitializeStateMachie();
        currentHP = maxHP;
        if (hpSlider != null) hpSlider.maxValue = 1;
    }

    protected virtual void Update()
    {
        if(IsAutoRotating())
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
            }
        }

        if (IsGravity)
        {
            // 重力を考慮（必要なら）
            velocity += Physics.gravity.y * Time.deltaTime;
            controller.Move((Vector3.up * velocity) * Time.deltaTime);

        }
    }

    bool IsAutoRotating()
    {
        Transform playerTransform = PlayerController.Instance.HostBase.transform;
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < 5f) // 例えば10ユニット以内なら自動回転を有効にする
            {
                return false;
            }
        }

        if (IsAutoRotator || PlayerController.Instance.HostBase as SkullHost)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 返ってきた骨のダメージ
    /// </summary>
    public void SkullDamage()
    {
        TakeDamage(20);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        if (hpSlider!=null) hpSlider.value = (float)currentHP / maxHP;
        if (currentHP <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        stateMachine.Transition(SkullBossDie.key);
        IsAutoRotator = false;
        IsGravity = false;
    }
}
