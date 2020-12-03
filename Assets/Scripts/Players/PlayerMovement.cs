using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;

    private float animTimer = 0;

    private void Awake()
    {
        player = GetComponent<Player>();
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
            if (player.ID == 0)
            {
                if (Master.Instance.ControlState == "keyboard")
                {
                    player.Direction = new Vector2(Input.GetAxisRaw("HorizKBP1"), player.Rb.velocity.y);
                }
                else if (Master.Instance.ControlState == "controller")
                {
                    player.Direction = new Vector2(Input.GetAxisRaw("HorizJoyP1"), player.Rb.velocity.y);
                }
            }
            else
            {
                if (Master.Instance.ControlStateP2 == "keyboard")
                {
                    player.Direction = new Vector2(Input.GetAxisRaw("HorizKBP2"), player.Rb.velocity.y);
                }
                else if (Master.Instance.ControlStateP2 == "controller")
                {
                    player.Direction = new Vector2(Input.GetAxisRaw("HorizJoyP2"), player.Rb.velocity.y);
                }
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
            //transform.localScale = new Vector3(player.Direction.x < 0 ? 1 : -1, 1, 1);
            switch (player.ID)
            {
                case 0:
                    if (player.Direction.x < 0)
                    {
                        player.PlayerAnimator.SetAnimation("walkBack");
                        player.State.SetState(PLAYERSTATE.WALK);
                    }
                    else if (player.Direction.x > 0)
                    {
                        player.PlayerAnimator.SetAnimation("walk");
                        player.State.SetState(PLAYERSTATE.WALK);
                    }
                    break;
                case 1:
                    if (player.Direction.x > 0)
                    {
                        player.PlayerAnimator.SetAnimation("walkBack");
                        player.State.SetState(PLAYERSTATE.WALK);
                    }
                    else if (player.Direction.x < 0)
                    {
                        player.PlayerAnimator.SetAnimation("walk");
                        player.State.SetState(PLAYERSTATE.WALK);
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
        if (IsGrounded())
        {
            player.Rb.AddForce(new Vector2(0, player.Log.JumpForce), ForceMode2D.Impulse);
            player.PlayerAnimator.SetAnimation("jumping");
            player.Log.isGrounded = false;
        }
    }

    public void mediumKick()
    {
        if (player.Log.isGrounded)
        {
            player.PlayerAnimator.SetAnimation("mediumKick");
            animTimer = player.PlayerAnimator.medKick;

        }
        else
        {
            player.PlayerAnimator.SetAnimation("jumpMedKick");
            animTimer = player.PlayerAnimator.medKick;
        }
    }

    public void mediumPunch()
    {
        if (player.Log.isGrounded)
        {
            player.PlayerAnimator.SetAnimation("mediumPunch");
            animTimer = player.PlayerAnimator.medPunch;
        }
        else
        {
            player.PlayerAnimator.SetAnimation("jumpMedPunch");
            animTimer = player.PlayerAnimator.medPunch;
        }
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(player.FootCollider.bounds.center,
            player.FootCollider.bounds.size, 0, Vector2.down, 0.1f, player.Log.Stage);

        if (hit.collider != null && (hit.collider != player.BodyCollider || hit.collider != player.AttackCollider || hit.collider != player.FootCollider))
        {
            player.Log.isGrounded = true;
        }
        else
        {
            player.Log.isGrounded = false;
        }
        return player.Log.isGrounded;
        
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
        if (player.PlayerAnimator.CurrAnim == "mediumKick" || player.PlayerAnimator.CurrAnim == "jumpMedKick" || player.PlayerAnimator.CurrAnim == "mediumPunch")
        {
            return true;
        }
        return false;
    }
}