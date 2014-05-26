using UnityEngine;
using System.Collections;

public class TeamScore : MonoBehaviour {

	private int m_Score;
    public float m_MultiplierDecay;
    private int m_Multiplier;

    public int MaxScoreMultiplier = 5;
    public float ScoreMultiplierDecayTime = 3f;


	public TeamScoreDisplay display;
	//public PlayerInfo playerInfo;

    public int ScoreMultiplier
    {
        get { return m_Multiplier;}
        set
        {
            //dont do anything if same
            if(m_Multiplier == value) return;

            //dont let multiplier fall below 1
            if(value <= 1)
            {
                m_Multiplier = 1;
            } 
            else
            {
                if(value <= MaxScoreMultiplier)
                {
                    m_Multiplier = value;
                }

                m_MultiplierDecay = ScoreMultiplierDecayTime;
            }
            UpdateMultiplierDisplayText();
        }

    }
	
	#region Event declaration
	public delegate void ScoreChangedEventHandler(TeamScore teamScore, int newScore);
	public event ScoreChangedEventHandler ScoreChangedEvent;
	
	void onScoreChanged()
	{
		UpdateScoreDisplayText();

		if(ScoreChangedEvent != null)
			ScoreChangedEvent(this, Score);
	}
	#endregion

    void Update()
    {
        if(m_MultiplierDecay > 0)
        {
            m_MultiplierDecay -= Time.deltaTime;

            if(m_MultiplierDecay <= 0)
            {
                ScoreMultiplier -= 1;
            }
        }
    }

	public int Score
	{
		get{ return m_Score; }
		set
		{
			//score is not negative and is different from before
			if(value >= 0 && m_Score != value)
			{
				m_Score = value;
				onScoreChanged();
			}
		}
	}

	public void Add(int amt)
	{
        Score += amt * ScoreMultiplier++;
	}

	public void ResetScore()
	{
		Score = 0;
        ScoreMultiplier = 1;
	}

	// Use this for initialization
	void Start () 
    {
		display = GetComponent<TeamScoreDisplay>();
	}
	
    void UpdateMultiplierDisplayText()
    {
        display.SetScoreDisplayText(Score, ScoreMultiplier);
    }

	void UpdateScoreDisplayText()
	{
		display.SetScoreDisplayText(Score, ScoreMultiplier);
	}

 
	
}
