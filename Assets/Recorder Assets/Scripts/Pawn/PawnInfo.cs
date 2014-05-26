using UnityEngine;
using System.Collections;

public class PawnInfo {

	public int team;
	//player = 0 is not a player
	public int player;

	public PawnInfo(int player, int team)
	{
		this.player = player;
		this.team = team;
	}
}
