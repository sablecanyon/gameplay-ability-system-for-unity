﻿using GAS.Runtime.Attribute;
using GAS.Runtime.AttributeSet;
using GAS.Runtime.Component;
using GAS.Runtime.Tags;
using UnityEngine;

public abstract class FightUnit : MonoBehaviour
{
    protected const float Gravity = 3f;
    protected const float HalfGravity = 1.5f;
    private static readonly int IsInAir = Animator.StringToHash("IsInAir");
    private static readonly int UpOrDown = Animator.StringToHash("UpOrDown");
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Defending = Animator.StringToHash("Defending");
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform _renderer;

    protected Rigidbody2D _rb;
    private int _velocityX;
    protected bool Grounded;
    protected float LastVelocityY;

    public AbilitySystemComponent ASC { get; private set; }

    public Transform Renderer => _renderer;
    public Rigidbody2D Rb => _rb;
    public float VelocityX => _velocityX;
    private bool IsMoving => ASC.HasTag(GameplayTagSumCollection.Event_Moving);
    public Animator Animator => _animator;
    private bool DoubleJumpValid => false; //_asc.HasTag(GameplayTagSumCollection.Event_DoubleJumpValid);


    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = Gravity;
        ASC = GetComponent<AbilitySystemComponent>();
        ASC.InitWithPreset(1);
    }

    protected virtual void FixedUpdate()
    {
        if (!ASC.HasTag(GameplayTagSumCollection.Ban_Motion))
        {
            if (Grounded || IsMoving)
            {
                var velocity = _rb.velocity;
                velocity.x = _velocityX * ASC.AttrSet<AS_Fight>().SPEED.CurrentValue;
                _rb.velocity = velocity;
            }

            LastVelocityY = _rb.velocity.y;
        }

        // 设置动画机参数
        _animator.SetFloat(IsInAir, Grounded ? 0 : 1);
        _animator.SetFloat(UpOrDown, 0.5f * (1 - Mathf.Clamp(LastVelocityY, -1, 1)));
        _animator.SetFloat(Moving, IsMoving ? 1 : 0);
        _animator.SetBool(Defending, ASC.HasTag(GameplayTagSumCollection.Event_Defending));
    }

    protected virtual void OnEnable()
    {
        ASC.AttrSet<AS_Fight>().HP.RegisterPostBaseValueChange(OnHpChange);
    }

    protected virtual void OnDisable()
    {
        ASC.AttrSet<AS_Fight>().HP.UnregisterPostBaseValueChange(OnHpChange);
    }

    public abstract void InitAttribute();

    public void SetIsGrounded(bool grounded)
    {
        if (Grounded != grounded)
        {
            if (grounded)
                ASC.RemoveFixedTag(GameplayTagSumCollection.Event_InAir);
            else
                ASC.AddFixedTag(GameplayTagSumCollection.Event_InAir);
        }

        Grounded = grounded;
    }

    protected void ActivateMove(float direction)
    {
        ASC.TryActivateAbility(MoveName, direction);
    }

    protected void DeactivateMove()
    {
        ASC.TryEndAbility(MoveName);
    }

    protected void Jump()
    {
        if (Grounded || DoubleJumpValid)
            ASC.TryActivateAbility(JumpName, _rb);
    }

    protected void Attack()
    {
        ASC.TryActivateAbility(AttackName);
    }

    protected void ActivateDefend()
    {
        ASC.TryActivateAbility(DefendName);
    }

    protected void DeactivateDefend()
    {
        ASC.TryEndAbility(DefendName);
    }

    protected void Dodge()
    {
        ASC.TryActivateAbility(DodgeName);
    }

    private void OnHpChange(AttributeBase attr, float oldValue, float newValue)
    {
        Debug.Log($"HP changed from {oldValue} to {newValue}");
    }

    public void SetVelocityX(int velocityX)
    {
        _velocityX = velocityX;
    }

    #region AbilityName

    protected abstract string MoveName { get; }
    protected abstract string JumpName { get; }
    protected abstract string AttackName { get; }
    protected abstract string DefendName { get; }
    protected abstract string DodgeName { get; }

    #endregion
}