using Rito.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShotGun : MonoBehaviour
{
    [Header("Angle Setting")]
    [SerializeField] private bool debug = false;
    [Range(0f, 360f)]
    [SerializeField] private float horizontalAngle = 0f;
    [SerializeField] private float radius = 1f;
    [Range(-180f, 180f)]
    [SerializeField] public float rotate = 0f;

    [Header("---")]
    [SerializeField] Transform muzzle;
    [SerializeField] Transform gun;
    [SerializeField] Transform test;
    Character player;
    Transform target;

    float radian;
    float horizontalHalfAngle = 0f;

    float shotDelay = 2;
    float currentDhotDelay = 2;

    private void Awake()
    {
        player = GetComponentInParent<Character>();
    }
    private void Update()
    {
        currentDhotDelay -= Time.deltaTime;
        if (target && currentDhotDelay <= 0 &&
            (target.transform.position - gun.position).magnitude <= radius)
        {
            currentDhotDelay = shotDelay;
            OnShot();
        }
    }
    public void SetData(float _delay)
    {
        currentDhotDelay = shotDelay = _delay;
        //InvokeRepeating("OnShot", shotDelay, shotDelay);
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
        Vector2 dir = target.transform.position - gun.position;
        if (dir.magnitude > radius) return; 
        //var euler = Quaternion.LookRotation(dir).eulerAngles;
        var rot = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 33);
        //Debug.Log($"{rot.z} / {}")
        gun.rotation = rot;
    }

    void OnShot()
    {
        var euler = gun.eulerAngles;
        float half = horizontalAngle * 0.5f;
        for (int i = 0; i < 4; i++)
        {
            var obj = ObjectPoolManager.I.Spawn("Bullet");
            obj.TryGetComponent(out Bullet bullet);
            float ran = Random.Range(-half, half);
            float spd = Random.Range(1,1.5f);
            var rot = Quaternion.Euler(euler.x, euler.y, euler.z - rotate + ran);
            bullet.SetData(player.GetAtk(), muzzle.position, rot, radius,spd);
        }
    }

    Vector2 AngleToDirZ(float angleInDegree)
    {
        radian = (angleInDegree - gun.eulerAngles.z) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
    }
    void OnDrawGizmos()
    {
        if (!debug) return;

        horizontalHalfAngle = horizontalAngle * 0.5f;

        Vector3 originPos = muzzle.position;

        Gizmos.DrawWireSphere(originPos, radius);

        Vector2 horizontalRightDir = AngleToDirZ(horizontalHalfAngle + rotate);
        Vector2 horizontalLeftDir = AngleToDirZ(-horizontalHalfAngle + rotate);
        Vector2 lookDir = AngleToDirZ(rotate);

        Debug.DrawRay(originPos, horizontalLeftDir * radius, Color.cyan);
        Debug.DrawRay(originPos, lookDir * radius, Color.green);
        Debug.DrawRay(originPos, horizontalRightDir * radius, Color.cyan);

        var euler = gun.eulerAngles;
        test.SetPositionAndRotation(muzzle.position, Quaternion.Euler(euler.x, euler.y, euler.z - rotate));

        Gizmos.DrawLine(gun.position, gun.position + gun.right);
    }
}
