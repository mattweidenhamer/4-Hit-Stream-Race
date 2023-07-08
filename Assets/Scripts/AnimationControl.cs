using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    Animator animator;   
    public enum AnimationState {stretch, ready, run, tired, bump, fallOver, won, loss};
    [SerializeField] AnimationState animationState = AnimationState.stretch;


    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("AnimationState", (int)animationState);
    }
    public void setAnimationState(AnimationState newAnimationState){
        animationState = newAnimationState;
        animator.SetInteger("AnimationState", (int)animationState);
    }
}
