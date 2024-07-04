using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float scrollSpeed = 2.0f;

    void Start()
    {
        PlatformScroller platformScroller = GetComponent<PlatformScroller>();
        if (platformScroller != null)
        {
            scrollSpeed = platformScroller.scrollSpeed;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        if (transform.position.x < -20) // 필요에 따라 조정
        {
            Destroy(gameObject);
        }
    }
}