﻿using UnityEngine;

public class HeroAnimator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayIdle()
    {
        _animator.Play("Idle");
    }
    public void PlayRun()
    {
        _animator.Play("Run");
    }

    public void LookAt(Vector3 target)
    {
        _spriteRenderer.flipX = target.x < transform.position.x;
    }
}