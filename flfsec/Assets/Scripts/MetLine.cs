using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetLine : MonoBehaviour
{ 
    void Start()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        // 선의 색상과 투명도 설정
        Color lineColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 회색, 알파 값 0.5
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        // 선의 너비 설정
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // 선의 위치 설정 (위 아래로 긴 선)
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(0, -5, 0)); // 시작점
        lineRenderer.SetPosition(1, new Vector3(0, 5, 0));  // 끝점
    }
}
