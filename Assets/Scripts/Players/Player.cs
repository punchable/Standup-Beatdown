using System.Collections;
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

    public GameObject MuShPrefab;
    public float Speed;

    public int ID;
    public string fighterSel;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D footCollider;

    [SerializeField]
    private Collider2D bodyCollider;
    private Collider2D attackCollider;
    private Vector2 direction;

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
        footCollider = GetComponentInChildren<Collider2D>();
        attackCollider = GetComponent<Collider2D>();
        Speed = 100f;
        playerAnimator = new PlayerAnimator(this);
        master = FindObjectOfType<Master>();
        manager = FindObjectOfType<UiManager>();
    }

    private void Start()
    {
        IdPlayer();
        SetUpFighter();

        PlayerAnimations[] animations = new PlayerAnimations[]
        {
            new PlayerAnimations("idle"),
            new PlayerAnimations("walk", "jumping"),
            new PlayerAnimations("walkBack", "jumping"),
            new PlayerAnimations("jumping", "jumpMedKick", "jumpMedPunch"),
            new PlayerAnimations("falling", "jumpMedKick", "jumpMedPunch"),
            new PlayerAnimations("mediumKick", "jumpMedKick"),
            new PlayerAnimations("jumpMedKick"),
            new PlayerAnimations("mediumPunch", "jumpMedPunch"),
            new PlayerAnimations("jumpMedPunch"),
            new PlayerAnimations("hit"),
            //new PlayerAnimations(""),
        };
        playerAnimator.AddAnimations(animations);

        Master.Instance.GameState = "fighting";
    }

    public void Update()
    {
        movement.HandleInput();
        movement.HandleAir();

        manager.UpdateHealth(ID, this);

        if (log.InvincibleTimer > 0)
        {
            log.InvincibleTimer -= Time.deltaTime;
        }
        else if (log.InvincibleTimer < 0)
        {
            log.InvincibleTimer = 0;
            log.IsInvincible = false;
        }
    }

    public void FixedUpdate()
    {
        movement.Move(transform);

        //movement.FaceOpponent();
    }

    public void AdjustHealth(int amount)
    {
        if (amount < 0)
        {
            if (log.IsInvincible)
                return;

            playerAnimator.SetAnimation("hit");
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
                commands.Add(new JumpCommand(this, Log.JumpKey));
                commands.Add(new MKCommand(this, Log.MedKickKey));
                commands.Add(new MPCommand(this, Log.MedPunchKey));
                commands.Add(new PauseCommand(this, KeyCode.Escape));
                break;
            case 1:
                fighterSel = Master.Instance.FighterSel2;
                commands.Add(new JumpCommand(this, Log.JumpKey));
                commands.Add(new MKCommand(this, Log.MedKickKey));
                commands.Add(new MPCommand(this, Log.MedPunchKey));
                commands.Add(new PauseCommand(this, KeyCode.Escape));
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

    public void OnTriggerStay2D(Collider2D other)
    {
        Player opponent = other.GetComponent<Player>();

        if (opponent != null)
        {
            if (opponent.Movement.IsAttacking())
            {
                AdjustHealth(-1);
                playerAnimator.SetAnimation("hit");
                playerAnimator.Timer = 1.0f;
            }
        }
    }
}
