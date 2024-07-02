using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformScroller : MonoBehaviour
{
    public float scrollSpeed;

    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        if (transform.position.x < -10)
        {
            Destroy(gameObject);
        }
    }
}