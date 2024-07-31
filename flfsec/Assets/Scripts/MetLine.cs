using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetLine : MonoBehaviour
{ 
    void Start()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        // ���� ����� ���� ����
        Color lineColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // ȸ��, ���� �� 0.5
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        // ���� �ʺ� ����
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // ���� ��ġ ���� (�� �Ʒ��� �� ��)
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(0, -5, 0)); // ������
        lineRenderer.SetPosition(1, new Vector3(0, 5, 0));  // ����
    }
}
