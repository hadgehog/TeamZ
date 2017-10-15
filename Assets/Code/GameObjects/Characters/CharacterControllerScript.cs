﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterControllerScript : MonoBehaviour
{
    public float RunSpeed;
    public float CreepSpeed;
    public float JumpForce;

    public Transform GroundCheck;
    public LayerMask WhatIsGround;

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum FightMode
    {
        None = -1,
        Punch = 0,
        Kick,
        TailHit,
        HullHit
    }

    protected bool IsGrounded = true;
    protected float GroundRadius = 0.15f;

    protected Animator anim;
    protected Rigidbody2D rigidBody;
    protected Direction currentDirection = Direction.Right;

    protected ICharacter Character;

    protected FightMode fightMode = FightMode.None;

    // Use this for initialization
    protected virtual void Start()
    {
        this.anim = GetComponent<Animator>();
        this.rigidBody = GetComponent<Rigidbody2D>();

        this.Character = GetComponent<Lizard>();
    }

    protected virtual void FixedUpdate()
    {
        this.IsGrounded = Physics2D.OverlapCircle(this.GroundCheck.position, this.GroundRadius, this.WhatIsGround);

        this.anim.SetBool("Ground", this.IsGrounded);
        this.anim.SetFloat("JumpSpeed", this.rigidBody.velocity.y);

        float move = Input.GetAxis("Horizontal");

        this.anim.SetFloat("Speed", Mathf.Abs(move));

        this.rigidBody.velocity = new Vector2(move * this.RunSpeed, this.rigidBody.velocity.y);

        Direction tempDirection = this.currentDirection;

        if (move < 0)
            tempDirection = Direction.Left;
        else if (move > 0)
            tempDirection = Direction.Right;

        if (tempDirection != this.currentDirection)
        {
            this.currentDirection = tempDirection;
            this.Flip();
        }
    }

    // called once per frame
    protected virtual void Update()
    {
        this.fightMode = FightMode.None;
        this.anim.SetInteger("HitType", -1);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            this.fightMode = FightMode.Punch;
            this.anim.SetInteger("HitType", (int)this.fightMode);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            this.fightMode = FightMode.Kick;
            this.anim.SetInteger("HitType", (int)this.fightMode);
            this.rigidBody.AddForce(new Vector2(0.0f, 800.0f));
        }

        if (this.IsGrounded && Input.GetKeyDown(KeyCode.Space) && this.fightMode == FightMode.None)
        {
            this.anim.SetBool("Ground", false);
            this.rigidBody.AddForce(new Vector2(0.0f, this.JumpForce));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<FirstAidKit>() != null)
        {
            // TODO: add effect of flying aid kit to health bar on HUD            
            Destroy(col.gameObject);
        }

        if (col.gameObject.GetComponent<ArmorKit>() != null)
        {
            // TODO: add effect of flying armor kit to armor bar on HUD            
            Destroy(col.gameObject);
        }

        if (col.gameObject.GetComponent<AbyssCollider>() != null)
        {
            // TODO: call LoadAsync instead LoadScene
            SceneManager.LoadScene("Laboratory");
        }
    }

    private void Flip()
    {
        Vector3 currentScale = this.transform.localScale;
        currentScale.x *= -1;

        this.transform.localScale = currentScale;
    }
}