using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Commands
{
    public KeyCode Key { get; private set; }

    public Commands(KeyCode key)
    {
        this.Key = key;
    }

    public virtual void GetKeyDown()
    {

    }

    public virtual void GetKeyUp()
    {

    }

    public virtual void GetKey()
    {

    }
}

public class JumpCommand : Commands
{
    private Player player;
    public JumpCommand(Player player, KeyCode key) : base(key)
    {
        this.player = player;
    }

    public override void GetKeyUp()
    {
        player.Movement.Jump();
    }
}

public class MKCommand : Commands
{
    private Player player;
    public MKCommand(Player player, KeyCode key) : base(key)
    {
        this.player = player;
    }

    public override void GetKeyUp()
    {
        player.Movement.mediumKick();
    }
}

public class MPCommand : Commands
{
    private Player player;

    public MPCommand(Player player, KeyCode key) : base(key)
    {
        this.player = player;
    }

    public override void GetKeyUp()
    {
        player.Movement.mediumPunch();
    }
}

public class PauseCommand : Commands
{
    private Player player;

    public PauseCommand(Player player, KeyCode key) : base(key)
    {
        this.player = player;
    }

    public override void GetKeyUp()
    {
        if (player.State.currentState != PLAYERSTATE.PAUSED)
        {
            player.PauseGame();
        }
        else
        {
            player.UnPauseGame();
        }
    }
}
