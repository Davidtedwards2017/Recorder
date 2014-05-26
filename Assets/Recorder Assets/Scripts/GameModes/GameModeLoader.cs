using UnityEngine;
using System.Collections;
using StageFramework;

public class GameModeLoader : MonoBehaviour {

	void Start () {
        GameObject go = GameObject.FindGameObjectWithTag("GameMode");
        go.GetComponent<GameMode>().InitiateGameMode();
	}

	
}
