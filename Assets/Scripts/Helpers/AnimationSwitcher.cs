using UnityEngine;

public class AnimationSwitcher : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void SwitchOn()
    {
        anim.enabled = true;
    }

    private void SwitchOff()
    {
        anim.enabled = false;
    }
}
