using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyScroller : MonoBehaviour
{
    public float scrollSpeed;

    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        if (transform.position.x < -13) // �ʿ信 ���� ����
        {
            Destroy(gameObject);
        }
    }
}