using UnityEngine;
using System.Collections;


public class PlayerState : MonoBehaviour
{

	public PLAYERSTATE currentState = PLAYERSTATE.IDLE;

	public void SetState(PLAYERSTATE state)
	{
		currentState = state;
	}
}

public enum PLAYERSTATE
{
	IDLE,
	WALK,
	JUMPING,
	JUMPKICK,
	PUNCH,
	KICK,
	DEFENDING,
	THROWKNIFE,
	HIT,
	PICKUPITEM,
	KNOCKDOWN,
	DEATH,
	PAUSED,
};
