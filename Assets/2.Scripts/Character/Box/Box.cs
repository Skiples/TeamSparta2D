using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : Character
{
    Slider hpSlider;
    Canvas canv;
    private void Awake()
    {
        transform.Find("HPPanel/Canvas").TryGetComponent(out canv);
        canv.transform.Find("HpSlider").TryGetComponent(out hpSlider);
        SetData(GetHp());

    }


    public override bool OnDamage(int _dmg)
    {
        bool b = base.OnDamage(_dmg);
        hpSlider.value = GetHp();
        return b;
    }
    public void SetData(int _hp)
    {
        SetHp(_hp);
        hpSlider.value = hpSlider.maxValue = _hp;
    }
}
