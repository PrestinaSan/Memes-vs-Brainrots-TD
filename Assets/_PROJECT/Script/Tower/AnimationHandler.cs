using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    public void OnAttackAnimationEndFreddy()
    {
        animator.SetTrigger("Idle");
    }
    public void OnAttackAnimationStartFreddy()
    {
        animator.SetTrigger("Attack");
    }
    public void OnBuffAnimationEndBongo()
    {
        animator.Play("Bongo Cat+");
    }
    public void OnBuffAnimationStartBongo(bool _isBuffing)
    {
        animator.SetBool("Is Drumming",_isBuffing);
    }
    public void OnPizzaEatFreddy()
    {
        animator.SetTrigger("Pizza");
    }
}
