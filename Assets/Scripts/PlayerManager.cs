using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum PlayerColor {
	Green, Blue, Red, Yellow
	//Red, Yellow, Blue, Green
}

public class PlayerManager : MonoBehaviour 
{
	public class PlayerData {
		public PlayerData(PlayerColor p_color) {
			color = p_color;
		}
		public int score = 0;
		public int totalShots = 0;
		public int shotsHit = 0;
		public PlayerColor color = PlayerColor.Red;
	}

	public List<PlayerData> playerData;

	static PlayerManager s_instance;

	Text redScoreText;
	Text greenScoreText;

	void Start () {
		playerData = new List<PlayerData> ();
		s_instance = this;
	}

	void Update () {

	}

	public static void IncreaseShots(PlayerColor color) {
		foreach(PlayerData player in s_instance.playerData){
			if(player.color == color) {
				player.totalShots ++;
				return;
			}
		}
	}

	public static void IncreaseHits(PlayerColor color) {
		foreach(PlayerData player in s_instance.playerData){
			if(player.color == color) {
				player.shotsHit++;
				return;
			}
		}
	}

	public void AddPlayer(PlayerColor color) {
		playerData.Add (new PlayerData (color));
	}

	public bool Added(PlayerColor color) {
		for(int i = 0; i < playerData.Count; i++) {
			if(playerData[i].color == color)
				return true;
		}
		return false;
	}

	public static void ReducePoints(int dmg) {
		foreach(PlayerData player in s_instance.playerData) {
			if(player.score < 0)
				player.score -= dmg;
		}
	}

	public static void AddPoints(PlayerColor color, int pts) 
	{
		foreach(PlayerData player in s_instance.playerData)
		{
			if(player.color == color) 
			{
				player.score += pts;
				s_instance.SetScoreText( player.score, player.color);
				return;
			}
		}
	}

	public void SetScoreText( int score, PlayerColor color )
	{
		if( color == PlayerColor.Red )
		{
			redScoreText.text = score.ToString();
		}
		else if( color == PlayerColor.Green )
		{
			greenScoreText.text = score.ToString();
		}
	}

	public void GetText()
	{
		redScoreText = GameObject.Find("Red Score Text").GetComponent<Text>();
		greenScoreText = GameObject.Find("Green Score Text").GetComponent<Text>();
		redScoreText.text = "0";
		greenScoreText.text = "0";
	}
}