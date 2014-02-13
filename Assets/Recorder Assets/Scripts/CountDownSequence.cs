using UnityEngine;
using System.Collections;

namespace Effects
{

	public class CountDownSequence : MonoBehaviour {

		#region event declaration
		public delegate void CountDownSequenceCompletedEventHandler();

		public event CountDownSequenceCompletedEventHandler CountDownSequenceCompletedEvent;

		void onCountDownSequenceCompleted(){ if(CountDownSequenceCompletedEvent != null) CountDownSequenceCompletedEvent();}

		#endregion


		protected float m_Countdown;
		protected bool bRunning;
		protected bool bDisplayEffect;

		void Awake()
		{
			bRunning = false;
			bDisplayEffect = false;
		}
		// Use this for initialization
		void Start () 
		{
		
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(!bRunning)
				return;

			if(m_Countdown < 0)
				CountDownCompleted();

			m_Countdown -= Time.deltaTime;
		}

		void OnGUI()
		{
			if(!bDisplayEffect)
				return;

			var CountDownGuiStyle = GUI.skin.GetStyle("label");
			CountDownGuiStyle.alignment = TextAnchor.UpperCenter;
			GUI.Label (new Rect ((Screen.width/2)-50, (Screen.height/2)-25, 100, 50), m_Countdown.ToString("F0"));
		}
		public void StartCountdown(float startingCountdownValue)
		{
			m_Countdown = startingCountdownValue;
			bRunning = true;
			bDisplayEffect = true;
		}

		public void CancelCountdown()
		{
			Destroy(gameObject);
		}

		public void RushCountdown()
		{
			onCountDownSequenceCompleted();
			Destroy(gameObject);
		}

		protected void CountDownCompleted()
		{
			bRunning = false;

			onCountDownSequenceCompleted();
			Destroy(gameObject);
		}
	}
}
