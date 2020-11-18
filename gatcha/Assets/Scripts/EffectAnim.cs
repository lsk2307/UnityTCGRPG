using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnim : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo animator =  anim.GetCurrentAnimatorStateInfo(0);

        if (animator.normalizedTime >= 0.99f)
            gameObject.SetActive(false);
    }
}
