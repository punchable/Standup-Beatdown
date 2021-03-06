﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
[System.Serializable]

public class Player : MonoBehaviour
{
    [SerializeField]
    private Master master;
    [SerializeField]
    private UiManager manager;
    [SerializeField]
    private PlayerLog log;
    [SerializeField]
    private PlayerAnimator playerAnimator;

    private InputManager inputs;

    public float Speed;
    public int ID;
    public string fighterSel;

    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField]
    private Collider2D footCollider;
    [SerializeField]
    private Collider2D bodyCollider;
    [SerializeField]
    private Transform fxHitPoint;

    private Collider2D attackCollider;
    private Vector2 direction;

    public GameObject fx;

    [SerializeField]
    private GameObject opponent;
    private Player opponentScript;

    private PlayerMovement movement;
    private PlayerState state;

    public string FighterSel { get => fighterSel; set => fighterSel = value; }
    public Vector2 Direction { get => direction; set => direction = value; }
    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public Collider2D FootCollider { get => footCollider; }
    public Collider2D BodyCollider { get => bodyCollider; }
    public Collider2D AttackCollider { get => attackCollider; }

    public PlayerLog Log { get => log; set => log = value; }
    public PlayerMovement Movement { get => movement; }
    public PlayerAnimator PlayerAnimator { get => playerAnimator; }
    public UiManager Manager { get => manager; }
    public PlayerState State { get => state; set => state = value; }
    public GameObject Opponent { get => opponent; }
    public Player OpponentScript { get => opponentScript; set => opponentScript = value; }

    public List<Commands> commands = new List<Commands>();


    private void Awake()
    {
        if (gameObject.name == "Player1")
        {
            ID = 0;
        }
        else
        {
            ID = 1;
        }

        state = GetComponent<PlayerState>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        attackCollider = GetComponent<Collider2D>();
        Speed = 100f;
        playerAnimator = new PlayerAnimator(this);
        master = FindObjectOfType<Master>();
        manager = FindObjectOfType<UiManager>();
        inputs = FindObjectOfType<InputManager>();


        IdPlayer();
        SetUpFighter();
    }

    private void Start()
    {

        PlayerAnimations[] animations = new PlayerAnimations[]
        {
            new PlayerAnimations("idle", "hit", "block"),
            new PlayerAnimations("walk", "jumping"),
            new PlayerAnimations("walkBack", "jumping"),
            new PlayerAnimations("block"),
            new PlayerAnimations("jumping", "jumpMedKick", "jumpMedPunch"),
            new PlayerAnimations("falling", "jumpMedKick", "jumpMedPunch"),
            new PlayerAnimations("mediumKick", "jumpMedKick"),
            new PlayerAnimations("jumpMedKick"),
            new PlayerAnimations("mediumPunch", "jumpMedPunch"),
            new PlayerAnimations("jumpMedPunch"),
            new PlayerAnimations("hit"),
            new PlayerAnimations("death"),
            new PlayerAnimations("victory"),
            //new PlayerAnimations(""),
        };
        playerAnimator.AddAnimations(animations);

        Master.Instance.GameState = "fighting";
        state.SetState(PLAYERSTATE.IDLE);
    }

    public void Update()
    {
        manager.UpdateHealth(ID, this);
        if (log.CurrHealth > 0)
        {
            movement.HandleInput();
            movement.HandleAir();

            if (log.InvincibleTimer > 0)
            {
                log.InvincibleTimer -= Time.deltaTime;
            }
            else if (log.InvincibleTimer < 0)
            {
                log.InvincibleTimer = 0;
                log.IsInvincible = false;
            }

            if (master.GameState == "paused")
            {
                state.SetState(PLAYERSTATE.PAUSED);
            }
        }
        else
        {
            state.SetState(PLAYERSTATE.DEATH);
            playerAnimator.SetAnimation("death");
        }

        if(state.currentState == PLAYERSTATE.VICTORY)
        {
            playerAnimator.SetAnimation("victory");
        }
    }

    public void FixedUpdate()
    {
        movement.Move(transform);


        if (Master.Instance.ControlState == "controller")
        {
            if (inputs.vertP1Joy < 0 && ID == 0)
            {
                movement.Jump();
            }
            else if (inputs.vertP2Joy < 0 && ID == 1)
            {
                movement.Jump();
            }
        }

        //movement.FaceOpponent();
    }

    public void AdjustHealth(int amount)
    {
        if (amount < 0)
        {
            if (movement.IsBlocking() || log.IsInvincible)
            {
                return;
            }

            playerAnimator.SetAnimation("hit");
            movement.AnimTimer = playerAnimator.hit;
            state.SetState(PLAYERSTATE.HIT);
            log.IsInvincible = true;
            log.InvincibleTimer = log.TimeInvincible;
        }

        log.CurrHealth = Mathf.Clamp(log.CurrHealth + amount, 0, log.MaxHealth);
    }

    public void IdPlayer()
    {
        switch (ID)
        {
            case 0:
                fighterSel = Master.Instance.FighterSel1;

                switch (Master.Instance.ControlState)
                {
                    case "controller":
                        
                        commands.Add(new MKCommand(this, inputs.P1JoyMedKick));
                        commands.Add(new MPCommand(this, inputs.P1JoyMedPunch));
                        commands.Add(new PauseCommand(this, inputs.P1JoyPause));
                        commands.Add(new BlockCommand(this, inputs.P1JoyBlock));
                        break;
                    case "keyboard":
                        commands.Add(new JumpCommand(this, inputs.P1KBJump));
                        commands.Add(new MKCommand(this, inputs.P1KBMedKick));
                        commands.Add(new MPCommand(this, inputs.P1KBMedPunch));
                        commands.Add(new PauseCommand(this, KeyCode.Escape));
                        commands.Add(new BlockCommand(this, inputs.P1KBBlock));
                        break;
                }
                break;

            case 1:
                fighterSel = Master.Instance.FighterSel2;

                switch (Master.Instance.ControlStateP2)
                {
                    case "controller":
                        
                        commands.Add(new MKCommand(this, inputs.P2JoyMedKick));
                        commands.Add(new MPCommand(this, inputs.P2JoyMedPunch));
                        commands.Add(new PauseCommand(this, inputs.P2JoyPause));
                        commands.Add(new BlockCommand(this, inputs.P2JoyBlock));
                        break;
                    case "keyboard":
                        commands.Add(new JumpCommand(this, inputs.P2KBJump));
                        commands.Add(new MKCommand(this, inputs.P2KBMedKick));
                        commands.Add(new MPCommand(this, inputs.P2KBMedPunch));
                        commands.Add(new PauseCommand(this, KeyCode.Escape));
                        commands.Add(new BlockCommand(this, inputs.P2KBBlock));
                        break;
                }
                break;

        }
    }

    public void SetUpFighter()
    {
        anim.runtimeAnimatorController = Resources.Load("Animators/" + fighterSel) as RuntimeAnimatorController;
        
        if (fighterSel == "MuSh")
        {
            log.MaxHealth = 10;
        }
        else if (fighterSel == "BobBig")
        {
            log.MaxHealth = 5;
        }
        else
        {
            log.MaxHealth = 7;
        }

        log.CurrHealth = log.MaxHealth;
    }

    public void PauseGame()
    {
        Master.Instance.GameState = "paused";
        state.SetState(PLAYERSTATE.PAUSED);
    }

    public void UnPauseGame()
    {
        Master.Instance.GameState = "fighting";
        state.SetState(PLAYERSTATE.IDLE);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Player opponent = other.GetComponent<Player>();

        if (opponent != null && log.CurrHealth != 0)
        {
            if (opponent.State.currentState == PLAYERSTATE.ATTACKING && state.currentState != PLAYERSTATE.BLOCKING)
            {
                AdjustHealth(-1);

                //Transform hitPoint = other.gameObject. FXPoint
            }
        }
    }
}
