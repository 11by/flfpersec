using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public float minSize = 3.0f; // 최소 카메라 사이즈
    public float maxSize = 5.0f; // 최대 카메라 사이즈

    private Camera mainCamera;
    private float originalSize;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mainCamera = Camera.main;
        originalSize = mainCamera.orthographicSize;
    }

    public IEnumerator Shake(float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float newSize = Random.Range(minSize, maxSize);
            mainCamera.orthographicSize = newSize;

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.orthographicSize = originalSize;
    }

    public void StartShake(float duration)
    {
        StartCoroutine(Shake(duration));
    }
}
