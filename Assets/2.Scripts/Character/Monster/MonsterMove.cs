using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Monster data;
    Collider2D col;
    Rigidbody2D rig;
    int line = -1;
    [SerializeField] float climbpower = 50;
    float climbDelay = 2;
    bool readyClimb;
    Vector2 dir;
    public void SetData(int _line)
    {
        line = _line;
        gameObject.layer = line + 6;
        readyClimb = true;
        rig.velocity = Vector2.left * data.GetSpd();

        col.enabled = true;
        rig.isKinematic = false;
    }
    void Awake()
    {
        if (! TryGetComponent(out rig) )
            Debug.LogError($"No {rig.GetType().Name} in {name}");
        if (!TryGetComponent(out col))
            Debug.LogError($"No {col.GetType().Name} in {name}");
        if (! TryGetComponent(out data) ) 
            Debug.LogError($"No {data.GetType().Name} in {name}");
    }

    void FixedUpdate()
    {
        if (rig == null) return;
        float spd = data.GetSpd();
        if (col.enabled && !Physics2D.Raycast((Vector2)transform.position + new Vector2(-.5f, .5f), Vector2.left, .1f, gameObject.layer))
            rig.AddForce(Vector2.left * 1000, ForceMode2D.Impulse);
        var dir = rig.velocity;
        dir.x = Mathf.Clamp(dir.x, -spd, spd);
        rig.velocity = dir;
    }
    private void OnCollisionStay2D(Collision2D _col)
    {
        if (!_col.gameObject.TryGetComponent(out Monster monster)) return;
        Vector2 colPos = _col.transform.position;
        Vector2 pos = transform.position;
        if (Physics2D.Raycast(pos + Vector2.up, Vector2.up,.15f, gameObject.layer))
        {
            CancelInvoke();
            Invoke("Delay", climbDelay);
            readyClimb = false;

            return;
        }

        if (colPos.x > pos.x || colPos.y - pos.y > .1f) return;
        Climb();
    }

    public void Knockback()
    {
        StartCoroutine(KnockbackRutine());
    }
    IEnumerator KnockbackRutine()
    {
        rig.isKinematic = true;
        col.enabled = false;
        rig.velocity = new Vector2(1,0) * climbpower;

        yield return YieldCache.WaitForSeconds(.1f);

        col.enabled = true;
        rig.isKinematic = false;
    }

    void Climb()
    {
        if (!readyClimb) return;
        rig.AddForce(Vector2.up * climbpower,ForceMode2D.Impulse);
        readyClimb = false;
        Invoke("Delay", climbDelay);
    }

    void Delay()
    {
        readyClimb = true;
    }
}
