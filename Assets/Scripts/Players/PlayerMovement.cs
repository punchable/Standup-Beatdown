using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private InputManager inputs;

    private float animTimer = 0;

    public float AnimTimer { get => animTimer; set => animTimer = value; }

    private void Awake()
    {
        player = GetComponent<Player>();
        inputs = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
        if (animTimer > 0)
        {
            animTimer -= Time.deltaTime;
        }
        else if (animTimer < 0)
        {
            player.PlayerAnimator.EndAnim();
            animTimer = 0;
        }
    }

    public void HandleInput()
    {
        if (player.State.currentState != PLAYERSTATE.PAUSED)
        {
            switch (player.ID)
            {
                case 0:
                    if (Master.Instance.ControlState == "keyboard")
                    {
                        player.Direction = new Vector2(inputs.horizP1KB, player.Rb.velocity.y);
                    }
                    else if (Master.Instance.ControlState == "controller")
                    {
                        player.Direction = new Vector2(inputs.horizP1Joy, player.Rb.velocity.y);
                    }
                break;

                case 1:
                    if (Master.Instance.ControlStateP2 == "keyboard")
                    {
                        player.Direction = new Vector2(inputs.horizP2KB, player.Rb.velocity.y);
                    }
                    else if (Master.Instance.ControlStateP2 == "controller")
                    {
                        player.Direction = new Vector2(inputs.horizP2Joy, player.Rb.velocity.y);
                    }
                break;
            }

            foreach (Commands command in player.commands)
            {
                if (Input.GetKeyDown(command.Key))
                {
                    command.GetKeyDown();
                }
                if (Input.GetKeyUp(command.Key))
                {
                    command.GetKeyUp();
                }
                if (Input.GetKey(command.Key))
                {
                    command.GetKey();
                }
            }
        }

    }

    public void HandleAir()
    {
        if (IsFalling() && !IsAttacking())
        {
            player.PlayerAnimator.SetAnimation("falling");
        }
    }

    public void Move(Transform transform)
    {
        player.Rb.velocity = new Vector2(player.Direction.x * player.Speed * Time.deltaTime, player.Rb.velocity.y);

        if (player.Direction.x != 0 && player.Log.isGrounded && !IsAttacking())
        {
            player.State.SetState(PLAYERSTATE.WALK);
            //transform.localScale = new Vector3(player.Direction.x < 0 ? 1 : -1, 1, 1);
            switch (player.ID)
            {
                case 0:
                    if (player.Direction.x < 0)
                    {
                        player.PlayerAnimator.SetAnimation("walkBack");
                    }
                    else if (player.Direction.x > 0)
                    {
                        player.PlayerAnimator.SetAnimation("walk");
                    }
                    break;
                case 1:
                    if (player.Direction.x > 0)
                    {
                        player.PlayerAnimator.SetAnimation("walkBack");
                    }
                    else if (player.Direction.x < 0)
                    {
                        player.PlayerAnimator.SetAnimation("walk");
                    }
                    break;
            }
        }

        if (player.Direction.x == 0 && player.Rb.velocity.y == 0 && IsGrounded() && !IsAttacking())
        {
            player.PlayerAnimator.SetAnimation("idle");
            player.State.SetState(PLAYERSTATE.IDLE);
        }
    }

    public void Jump()
    {
        if (player.Log.isGrounded)
        {
            player.Rb.AddForce(new Vector2(0, player.Log.JumpForce), ForceMode2D.Impulse);
            player.PlayerAnimator.SetAnimation("jumping");
            player.Log.isGrounded = false;
            player.State.SetSubState(PLAYERSTATE.JUMPING);
        }
    }

    public void Block()
    {
        if (player.Log.isGrounded && !IsBlocking())
        {
            player.PlayerAnimator.SetAnimation("block");
        }
    }

    public void mediumKick()
    {
        if (player.Log.isGrounded && player.State.currentState != PLAYERSTATE.ATTACKING)
        {
            player.PlayerAnimator.SetAnimation("mediumKick");
            animTimer = player.PlayerAnimator.medKick;
            player.State.SetState(PLAYERSTATE.ATTACKING);
        }
        else if (!player.Log.isGrounded && player.State.currentState != PLAYERSTATE.ATTACKING)
        {
            player.PlayerAnimator.SetAnimation("jumpMedKick");
            animTimer = player.PlayerAnimator.medKick;
            player.State.SetState(PLAYERSTATE.ATTACKING);
        }
    }

    public void mediumPunch()
    {
        if (player.Log.isGrounded)
        {
            player.PlayerAnimator.SetAnimation("mediumPunch");
            animTimer = player.PlayerAnimator.medPunch;
            player.State.SetState(PLAYERSTATE.ATTACKING);
        }
        else
        {
            player.PlayerAnimator.SetAnimation("jumpMedPunch");
            animTimer = player.PlayerAnimator.medPunch;
            player.State.SetState(PLAYERSTATE.ATTACKING);
        }
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(player.FootCollider.bounds.center,
            player.FootCollider.bounds.size, 0, Vector2.down, 0.1f, player.Log.Stage);

        if (hit.collider != null && (hit.collider != player.BodyCollider || hit.collider != player.AttackCollider || hit.collider != player.FootCollider))
        {
            player.Log.isGrounded = true;
            player.State.SetSubState(PLAYERSTATE.GROUNDED);
        }
        else
        {
            player.Log.isGrounded = false;
            player.State.SetSubState(PLAYERSTATE.JUMPING);
        }
        return player.Log.isGrounded;
    }

    public bool IsFalling()
    {
        if (player.Rb.velocity.y < 0)
        {
            return true;
        }
        return false;
    }

    public bool IsAttacking()
    {
        if (player.State.currentState == PLAYERSTATE.ATTACKING)
        {
            return true;
        }
        return false;
    }

    public bool IsHit()
    {
        if (player.State.currentState == PLAYERSTATE.HIT)
        {
            return true;
        }
        return false;
    }

    public bool IsBlocking()
    {
        if (player.State.currentState == PLAYERSTATE.BLOCKING)
        {
            return true;
        }
        return false;
    }

    public void FaceOpponent()
    {
        RaycastHit2D hitBody = Physics2D.Raycast(player.BodyCollider.bounds.center, Vector2.right, 10f, player.Log.Stage);

        if (hitBody.collider != null && hitBody.collider != player.AttackCollider)// || hitBody.collider != player.BodyCollider)
        {
            Debug.Log("Opponent to the right of " + player.ID.ToString());
            transform.localScale = new Vector3(-1, 1, 1);
            return;
        }
        else
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(player.BodyCollider.bounds.center, Vector2.left, 10f, player.Log.Stage);

            if(hitLeft.collider != null)
            {
                Debug.Log("Opponent to the left of " + player.ID.ToString());
                transform.localScale = new Vector3(1, 1, 1);
                return;
            }
            else
            {
                Debug.Log("Broken");
            }
        }
    }
}