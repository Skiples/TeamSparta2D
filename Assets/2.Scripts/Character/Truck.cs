using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    [SerializeField] Transform carryTrans;
    [SerializeField] List<Character> boxs = new List<Character>();
    float height;
    private void Awake()
    {
        Application.targetFrameRate = 30;
        if (boxs.Count == 0) return;
        boxs[0].transform.Find("SpritesParent/Box").TryGetComponent(out SpriteRenderer render);
        height = render.bounds.size.y-0.01f;
    }
    void Update()
    {
        CheckBoxs();
    }

    void CheckBoxs()
    {
        for (int i = boxs.Count - 1; i >= 0; i--)
        {
            if (boxs[i].GetHp() <= 0 || boxs[i] == null)
            {
                boxs.RemoveAt(i);
            } 
        }

        if(boxs.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 carryPos = carryTrans.position;
        for (int i = 0,len = boxs.Count; i < len; i++)
        {
            boxs[i].transform.position = carryPos + new Vector2(0, height * i);
        }

    }
}
