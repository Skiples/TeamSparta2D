using Rito.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] string prefabName = "Monster";
    [SerializeField] Transform[] lineTrans;
    List<Monster> monsters = new List<Monster>(128);
    float spawnDelay = .8f;
    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnDelay, spawnDelay);
    }

    void SpawnEnemy()
    {
        int hp = 100;
        int line = Random.Range(0, lineTrans.Length);
        var obj = ObjectPoolManager.I.Spawn(prefabName);

        if (!obj.TryGetComponent(out Monster mob))
            Debug.LogError("Monster Spawn Error");

        mob.SetData(hp, line);
        obj.transform.position = GetLineTrans(line);
        monsters.Add(mob);
    }
    public Monster NearestMob(Vector2 _pos)
    {
        ListCheck();
        Monster mob = null;
        float dis = float.MaxValue;
        for (int i = 0,len = monsters.Count; i < len; i++)
        {
            float current = (_pos - (Vector2)monsters[i].transform.position).sqrMagnitude;
            if (dis > current)
            {
                dis = current;
                mob = monsters[i];
            }
        }
        return mob;
    }
    void ListCheck()
    {
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            if (monsters[i].GetHp() <= 0 || monsters[i] == null)
            {
                monsters.RemoveAt(i);
            }
        }

    }
    Vector2 GetLineTrans(int _line)
    {
        return lineTrans[_line].position;
    }
}
