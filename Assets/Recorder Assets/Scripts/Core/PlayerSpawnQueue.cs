using UnityEngine;
using System.Collections;
using StageFramework;

public class PlayerSpawnQueue : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	
        if(GameStateManager.Instance.CurrentGameState != GameState.Playing)
            return;

        foreach(PlayerInfo player in gameObject.GetComponentsInChildren<PlayerInfo>())
        {
            player.TryToSpawn();
        }
	}
}
