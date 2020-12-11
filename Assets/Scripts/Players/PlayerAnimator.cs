using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAnimator
{
    private Player player;
    private string currAnim;
    public float medKick = 0.65f;
    public float medPunch = 0.55f;
    private float timer = 0;

    private Dictionary<string, PlayerAnimations> animations = new Dictionary<string, PlayerAnimations>();

    public string CurrAnim { get => currAnim; }
    public float Timer { get => timer; set => timer = value; }

    public PlayerAnimator(Player player)
    {
        this.player = player;
        currAnim = "idle";
    }

    private void Awake()
    {

    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            EndAnim(currAnim);
            timer = 0;
        }
    }

    public void AddAnimations(params PlayerAnimations[] newAnimations)
    {
        for (int i = 0; i < newAnimations.Length; i++)
        {
            this.animations.Add(newAnimations[i].animName, newAnimations[i]);
        }
    }

    public void SetAnimation(string name)
    {
        PlayAnimation(ref currAnim);
        void PlayAnimation(ref string currAnim)
        {
            if (currAnim == "")
            {
                animations[name].Active = true;
                currAnim = name;
            }
            else if (currAnim != name && !animations[name].higherPrio.Contains(currAnim) || !animations[currAnim].Active)
            {
                animations[currAnim].Active = false;
                animations[name].Active = true;
                currAnim = name;
            }
        }
        Animate();
    }

    public void Animate()
    {
        foreach (string key in animations.Keys)
        {
            player.Anim.SetBool(key, animations[key].Active);
        }
    }

    public void EndAnim(string anim)
    {

        if (currAnim == anim)
        {
            animations[anim].Active = false;
            animations["idle"].Active = true;
            currAnim = "idle";
        }
        SetAnimation("idle");
        player.State.SetState(PLAYERSTATE.IDLE);
    }
}

public class PlayerAnimations
{
    public bool Active { get; set; }
    public string AnimName { get => animName; set => animName = value; }

    public string animName;
    public string[] higherPrio { get; set; }

    public PlayerAnimations(string name, params string[] prio)
    {
        higherPrio = prio;
        animName = name;
    }
}
