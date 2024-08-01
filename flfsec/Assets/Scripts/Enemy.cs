using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    private string animationToPlay;
    public void SetAnimation(string animationName)
    {
        if (animator != null)
        {
            animationToPlay = animationName;
            if (gameObject.activeInHierarchy)
            {
                animator.Play(animationName);
            }
        }
    }
}