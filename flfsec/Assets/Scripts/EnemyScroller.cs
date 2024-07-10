using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyScroller : MonoBehaviour
{
    public float scrollSpeed;

    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        if (transform.position.x < -13) // 필요에 따라 조정
        {
            Destroy(gameObject);
        }
    }
}