using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public void SetAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
