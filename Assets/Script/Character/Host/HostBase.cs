using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ホスト（動かせるキャラクター）の基底クラス
/// </summary>
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

    /// <summary>
    /// ホストを開始する。基本的にはプレイヤーがホストをのっとるときに呼ばれる
    /// </summary>
    /// <param name="ago"></param>
    public virtual void StartHost(HostBase ago)
    {
        Possessor = ago;
        gameObject.SetActive(true);
        StartCoroutine(HostStart());
        UIManager.Instance?.HPVar.SetHPVar(currentHp, maxHp);
    }

    /// <summary>
    /// ホストを終了する。基本的にはプレイヤーがホストをのっとるのをやめるときに呼ばれる
    /// </summary>
    public virtual void EndHost()
    {
        isPossessed = false;
    }

    /// <summary>
    /// ホストの更新処理。
    /// </summary>
    public virtual void UpdateHost()
    {
        HostBack();
        Velocity();
    }

    /// <summary>
    /// プレイヤーに重力を毎フレーム与える
    /// </summary>
    protected virtual void Velocity()
    {
        Vector3 gravity = Physics.gravity * Time.deltaTime;
        if (controller != null&&controller.enabled&& gameObject.activeInHierarchy)
        {
            controller.Move(gravity);
        }
    }

    /// <summary>
    /// 乗っ取り状態を解除入力を確認し、解除する
    /// </summary>
    public void HostBack()
    {
        if (Possessor == null||this is SlimeHost||!isPossessed) return;

        PlayerInput input = InputData.Instance.InputAction();

        if(input.Button3.onButton)
        {
            ForceHostBack();
        }
    }

    /// <summary>
    /// 乗っ取り状態を解除する
    /// </summary>
    public void ForceHostBack()
    {
        if (Possessor == null) return;
        PlayerController.Instance.SetHost(Possessor, this);
        Possessor = null;
    }

    protected virtual void Start()
    {
        
    }

    IEnumerator HostStart()
    {
        yield return new WaitForSeconds(0.5f);
        isPossessed = true;
    }
}
