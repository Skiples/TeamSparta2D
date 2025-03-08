using Rito.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] int maxHp;
    [SerializeField] int hp;
    [SerializeField] int spd;
    [SerializeField] int atk;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    protected void SetHp(int _maxHp)
    {
        hp = maxHp = _maxHp;
    }


    public virtual bool OnDamage(int _dmg)
    {
        hp -= _dmg;
        if (hp > 0)
            return false;
        OnDeath();
        return true;
    }
    protected virtual void OnDeath()
    {
        hp = 0;
        ObjectPoolManager.I.Despawn(gameObject);
    }



    public int GetSpd()
    {
        return spd;
    }
    public int GetAtk()
    {
        return atk;
    }
    public int GetHp()
    {
        return hp;
    }
}
