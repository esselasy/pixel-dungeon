using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speedMultiplier = 8.0f;
    new SpriteRenderer renderer;
    new Rigidbody2D rigidbody;
    Animator animator;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * speedMultiplier, 0.8f),
                                    Mathf.Lerp(0, Input.GetAxis("Vertical") * speedMultiplier, 0.8f));
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            animator.SetTrigger("HeroIdle");
        }
        else
        {
            renderer.flipX = Input.GetAxis("Horizontal") < 0;
            animator.SetTrigger("HeroRun");
        }
    }
}
