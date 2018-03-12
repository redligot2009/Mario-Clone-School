using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [HideInInspector]
    public PhysicsObject po;
    public float jumpSpeed = 8f, moveSpeed = 8f, maxFall = 10f;
    float eps = 0.75f;

    public float accelerationSpeed = 2f;
    public bool dead = false;

    Animator anim;
    SpriteRenderer sprite;

    void Start()
    {
        po = GetComponent<PhysicsObject>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    float smoothDampVelocityX = 0f;
    float jumpTimer = 0, jumpStartTime = 0.25f;
    int jumpstate = 0;

    void Update()
    {
        float xdir = Input.GetAxisRaw("Horizontal");
        float sprintCoefficient = (Input.GetKey(KeyCode.X) ? 1.5f : 1);
        if (po.velocity.x < -eps)
        {
            sprite.flipX = true;
        }
        else if (po.velocity.x > eps)
        {
            sprite.flipX = false;
        }
        if (po.collisions.below)
        {
            anim.SetBool("Ground", true);
            anim.SetBool("isJumping", false);
            if (!dead)
            {
                if (po.velocity.x < -eps || po.velocity.x > eps)
                {
                    anim.SetBool("isRunning", true);
                }
                else if (po.velocity.x >= -eps && po.velocity.x <= eps)
                {
                    anim.SetBool("isRunning", false);
                }
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
        }
        else
        {
            anim.SetBool("Ground", false);
            anim.SetBool("isJumping", true);
        }
        anim.speed = sprintCoefficient;
        po.velocity.x = Mathf.Clamp(po.velocity.x, -moveSpeed * sprintCoefficient, moveSpeed * sprintCoefficient);
        if (!dead)
        {
            //enemy stomp behavior
            RaycastHit2D enemyStomp = po.CheckVerticalHit(LayerMask.GetMask("enemy"), 0.1f);
            if (enemyStomp)
            {
                po.velocity.y = jumpSpeed * 0.65f;
                enemyStomp.collider.GetComponent<GoombaAI>().Die();
            }
            //enemy kill behavior
            if (po.CheckHorizontal(LayerMask.GetMask("enemy"), 0.1f))
            {
                dead = true;
                anim.SetBool("isJumping", false);
                anim.SetBool("isRunning", false);
            }
            //Hit Blocks behavior
            RaycastHit2D hitQuestionBlock = po.CheckVerticalHit(LayerMask.GetMask("questionblock"), 0.1f,1);
            if (hitQuestionBlock)
            {
                float xdiff = Mathf.Abs(hitQuestionBlock.collider.transform.position.x - transform.position.x);
                if (xdiff < 0.55f)
                    hitQuestionBlock.collider.GetComponent<QuestionBlock>().bounce = true;
            }
            RaycastHit2D hitBrickBlock = po.CheckVerticalHit(LayerMask.GetMask("brickblock"), 0.1f, 1);
            if (hitBrickBlock)
            {
                float xdiff = Mathf.Abs(hitBrickBlock.collider.transform.position.x - transform.position.x);
                if(xdiff < 0.55f)
                    hitBrickBlock.collider.GetComponent<DestroyableBrick>().Die();
            }
            //setting velocity to zero on hitting walls
            if (po.collisions.below && po.velocity.y < 0)
            {
                po.velocity.y = 0;
            }
            if (po.collisions.above && po.velocity.y > 0)
            {
                po.velocity.y = -2;
            }
            if (po.collisions.left && xdir < 0 || po.collisions.right && xdir > 0)
            {
                po.velocity.x = 0;
            }
            //movement
            if (xdir < 0 && !po.collisions.left)
            {
                if (po.velocity.x > eps)
                {
                    po.velocity.x = Mathf.Lerp(po.velocity.x, 0f, Time.deltaTime * 10f / sprintCoefficient);
                }
                else
                {
                    po.velocity.x = Mathf.SmoothDamp(po.velocity.x, -moveSpeed * sprintCoefficient, ref smoothDampVelocityX, Time.deltaTime * accelerationSpeed);
                }
            }
            else if (xdir > 0 && !po.collisions.right)
            {
                if (po.velocity.x < -eps)
                {
                    po.velocity.x = Mathf.Lerp(po.velocity.x, 0f, Time.deltaTime * 10f / sprintCoefficient);
                }
                else
                {
                    po.velocity.x = Mathf.SmoothDamp(po.velocity.x, moveSpeed * sprintCoefficient, ref smoothDampVelocityX, Time.deltaTime * accelerationSpeed);
                }
            }
            else if (xdir == 0)
            {
                smoothDampVelocityX = 0;
                po.velocity.x = Mathf.Lerp(po.velocity.x, 0f, Time.deltaTime * 10f);
            }
            //Jumping Mechanics
            if ((Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Z)) && po.collisions.below) jumpstate = 1;
            if ((Input.GetKey(KeyCode.Space)||Input.GetKey(KeyCode.Z)) && po.collisions.below && jumpstate == 1)
            {
                if (jumpTimer <= 0)
                {
                    jumpTimer = jumpStartTime;
                }
                po.velocity.y = Mathf.Lerp(po.velocity.y, jumpSpeed, Time.deltaTime * 55f);
            }
            else if (Input.GetKeyUp(KeyCode.Space)||Input.GetKeyUp(KeyCode.Z))
            {
                if (po.velocity.y > 0)
                {
                    jumpTimer = 0;
                    po.velocity.y = po.velocity.y * 0.65f;
                }
            }
            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
            }
            //physics shit
            if (jumpTimer <= 0)
            {
                if (!po.collisions.below)
                    jumpstate = 0;
                po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime * (po.velocity.y < 0 ? 1.5f : 1);
            }
            po.velocity.y = Mathf.Clamp(po.velocity.y, -maxFall, Mathf.Infinity);
            po.Move(po.velocity * Time.deltaTime);
        }
    }
}