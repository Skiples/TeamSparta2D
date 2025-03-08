using Rito.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 startPos;
    int atk;
    float range;
    float spd;
    public void SetData(int _dmg, Vector2 _pos, Quaternion _rot, float _range, float _spd)
    {
        atk = _dmg;
        spd = _spd;
        range = _range;
        startPos = _pos;
        transform.SetPositionAndRotation(_pos, _rot);
    }

    private void Update()
    {
        if (((Vector2)transform.position - startPos).magnitude > range)
            ObjectPoolManager.I.Despawn(gameObject);

        transform.position += transform.up * (Time.deltaTime * spd * 10);
    }
    private void OnTriggerEnter2D(Collider2D _col)
    {
        if(_col.TryGetComponent(out Monster monster))
        {
            monster.OnDamage(atk);
            ObjectPoolManager.I.Despawn(gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position+ transform.up);
    }
}
