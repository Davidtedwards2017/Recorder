using UnityEngine;
using System.Collections;
using StageFramework;

public class TeamInfo : MonoBehaviour {

    public int TeamNumber;

    public static TeamInfo AddTeamInfo(GameObject go, int team)
    {
        DestoryExistingTeamInfo(go);

        TeamInfo teamInfo = go.AddComponent<TeamInfo>();
        teamInfo.TeamNumber = team;
        return teamInfo;
    }

    public static void DestoryExistingTeamInfo(GameObject go)
    {
        TeamInfo teamInfo = go.GetComponent<TeamInfo>();
        
        if(teamInfo != null)
        {
            Destroy(teamInfo);
        }
    }

    public Color TeamColor
    {
        get
        {
            return GameInfo.GetTeamColor(TeamNumber);
        }
    }

}
