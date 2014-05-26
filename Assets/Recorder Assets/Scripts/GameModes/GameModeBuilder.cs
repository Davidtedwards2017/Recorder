using UnityEngine;
using System.Collections;
using StageFramework;
using System.Linq;

public class GameModeBuilder : MonoBehaviour {

    public GameObject GameModeObject;
  

    public GameMode LoadGameMode(string gameModeType)
    {
        DestroyAnyExistingGameModeObjects();

        GameModeObject = new GameObject("GameMode");
        GameModeObject.tag = "GameMode";

        DontDestroyOnLoad (GameModeObject);
           
        return InitiateGameModeType(gameModeType);
    }

    private GameMode InitiateGameModeType(string gameModeType)
    {
        GameMode gm;

        switch (gameModeType.ToLower())
        {
            case "husk":
                gm = CreateGameMode_Husk();
                break;
            case "versus":
                gm = CreateGameMode_Versus();
                break;
            default:
                gm = null;
                break;
        }

        return gm;
    }

    [System.Serializable]
    public class GameModeBuilderHuskModeSettings
    {
        public float MaxLifetime = 10;
        public float BaseLifeTime = 5;
        public int NumberOfPlayers = 1;
        public int MimicRespawnTime = 3;
        public int PlayerRespawnTime = 1;
        public bool UseTeamSpawns = false;
        public bool UseLifeTime = true;
    }
    public GameModeBuilderHuskModeSettings HuskModeSettings = new GameModeBuilderHuskModeSettings();

    private GameMode CreateGameMode_Husk()
    {
        GameMode gm = GameModeObject.AddComponent<GM_Husk>();
              
        gm.NumberOfPlayers = HuskModeSettings.NumberOfPlayers;
        gm.MaxLifeTime = HuskModeSettings.MaxLifetime;
        gm.BaseLifeTime = HuskModeSettings.BaseLifeTime;
        gm.playerRespawnTime = HuskModeSettings.PlayerRespawnTime;
        gm.mimicRespawnTime = HuskModeSettings.MimicRespawnTime;
        gm.UseTeamSpawns = HuskModeSettings.UseTeamSpawns;
        gm.UseLifeTime = HuskModeSettings.UseLifeTime;

        return gm;
    }

    [System.Serializable]
    public class GameModeBuilderVersusModeSettings
    {
        public float MaxLifetime = 10;
        public float BaseLifeTime = 10;
        public int NumberOfPlayers = 2;
        public int MimicRespawnTime = 3;
        public int PlayerRespawnTime = 1;
        public bool UseTeamSpawns = true;
        public bool UseLifeTime = false;
    }

    public GameModeBuilderVersusModeSettings VersusModeSettings = new GameModeBuilderVersusModeSettings();

    private GameMode CreateGameMode_Versus()
    {
        GameMode gm = GameModeObject.AddComponent<GM_Versus>();

        gm.NumberOfPlayers = VersusModeSettings.NumberOfPlayers;
        gm.MaxLifeTime = VersusModeSettings.MaxLifetime;
        gm.BaseLifeTime = VersusModeSettings.BaseLifeTime;
        gm.playerRespawnTime = VersusModeSettings.PlayerRespawnTime;
        gm.mimicRespawnTime = VersusModeSettings.MimicRespawnTime;
        gm.UseTeamSpawns = VersusModeSettings.UseTeamSpawns;
        gm.UseLifeTime = VersusModeSettings.UseLifeTime;
        
        return gm;
    }


    private void DestroyAnyExistingGameModeObjects()
    {
        //ensure a GameMode object does not exists
        GameObject[] GameModeObjects = GameObject.FindGameObjectsWithTag("GameMode");
        foreach(GameObject go in GameModeObjects) Destroy(go);
    }
    
}
