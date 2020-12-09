using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour
{

	public PLAYERSTATE currentState;
	public PLAYERSTATE subState;

	public void SetState(PLAYERSTATE state)
	{
		currentState = state;
	}

	public void SetSubState(PLAYERSTATE state)
    {
		subState = state;
    }
}

public enum PLAYERSTATE
{
	IDLE,
	WALK,
	BLOCKING,
	GROUNDED,
	JUMPING,
	ATTACKING,
	DEFENDING,
	HIT,
	DEATH,
	PAUSED,
};
