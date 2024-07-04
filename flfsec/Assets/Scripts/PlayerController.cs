using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f; // ���� ��
    public float downwardForce = -50f; // ���߿��� ���� �� �Ʒ��� �������� ��
    public LayerMask groundLayer; // ���� ���̾� ����ũ
    public float groundCheckDistance = 1.0f; // ���� �˻� �Ÿ�
    public float fixedXPosition = -5.5f; // �÷��̾��� ������ x ��ǥ

    private Rigidbody2D rb;
    private bool isGrounded; // ���� ���� �ִ��� ���� üũ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CheckGrounded();
    }

    void Update()
    {
        // ���� �Է� üũ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                // ���߿��� ���� Ű�� ������ �� �Ʒ��� ���� ���� ���� �����ϵ��� ó��
                ApplyDownwardForce();
            }
        }
    }

    void FixedUpdate()
    {
        CheckGrounded();

        Vector3 position = transform.position;
        position.x = fixedXPosition;
        transform.position = position;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // ���� �Ŀ��� ���߿� �ִ� ���·� ����
    }

    void ApplyDownwardForce()
    {
        rb.velocity = new Vector2(rb.velocity.x, downwardForce);
    }

    void CheckGrounded()
    {
        // ���� ���� �ִ��� ���� üũ�ϴ� ����ĳ��Ʈ
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        if (hit.collider != null && hit.collider.CompareTag("Platform"))
        {
            if (Mathf.Abs(hit.distance) < 0.1f)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }
}