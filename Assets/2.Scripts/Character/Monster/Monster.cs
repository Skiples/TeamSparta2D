using Rito.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    [SerializeField] string targetTag;
    Animator ani;
    MonsterMove movement;
    bool readyAttack;
    float atkDelay =2;
    private void Awake()
    {
        if (!TryGetComponent(out movement))
            Debug.LogError($"No {movement.GetType().Name} in {name}");
        ani = GetComponentInChildren<Animator>();
        if(!ani)
            Debug.LogError($"No {ani.GetType().Name} in {name}");
    }

    private void OnCollisionStay2D(Collision2D _col)
    {
        if (!_col.gameObject.CompareTag(targetTag)) return;
        if (!_col.gameObject.TryGetComponent(out Character character)) return;
        if (!readyAttack) return;
        readyAttack = false;
        character.OnDamage(GetAtk());
        ani.SetBool("IsAttacking", true);
    }
    public void OnEndAttack()
    {
        ani.SetBool("IsIdle", true);
        ani.SetBool("IsAttacking", false);
        Invoke("Delay", atkDelay);
    }
    public void SetData(int _maxHp, int _line)
    {
        readyAttack = true;
        SetHp(_maxHp);
        movement.SetData(_line);
        ani.SetBool("IsDead", false);
        ani.SetBool("IsIdle", true);
        ani.SetBool("IsAttacking", false);
    }
    public override bool OnDamage(int _dmg)
    {
        bool b = base.OnDamage(_dmg);
        if(!b)
            movement.Knockback();
        return b;
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        ani.SetBool("IsDead", true);
        ani.SetBool("IsIdle", false);
        ani.SetBool("IsAttacking", false);
    }
    void Delay()
    {
        readyAttack = true;
    }
}
