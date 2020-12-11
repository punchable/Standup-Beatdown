using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class PlayerLog
{
    [SerializeField]
    private LayerMask stage;

    private float jumpForce = 10f;

    public bool isGrounded = true;

    private bool isInvincible;
    private float invincibleTimer;
    private float timeInvincible = 1.0f;

    private int maxHealth;
    private int currHealth;

    public LayerMask Stage { get => stage; }
    public float JumpForce { get => jumpForce; }
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
    public float InvincibleTimer { get => invincibleTimer; set => invincibleTimer = value; }
    public float TimeInvincible { get => timeInvincible; set => timeInvincible = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int CurrHealth { get => currHealth; set => currHealth = value; }
}
