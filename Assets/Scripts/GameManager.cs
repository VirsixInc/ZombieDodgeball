using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	enum GameMode {Intro, Main, Scoreboard}

	public int maxLives = 10;
	private int currLives;

	public GameObject enemy;
	public Transform enemySpawnPoint;
	public float spawnRate = 1f;

	public bool randomSpawnTime = false;
	public float randomRange = 1f;

	GameMode mode = GameMode.Intro;
	bool gameStarted = false;
	public bool isGamePlaying = false;

	public float joinTimer = 5f;
	public float gameTimer = 180f;
	public float scoreboardTimer = 15f;

	[System.NonSerialized]
	public float timer = 0f;

//	GUIStyle guiStyle;
	public Font guiFont;
	public float guiLeft = 0.2f;
	public float guiTop = 0.8f;
	public Transform gameOverText;

	public UnityEngine.UI.Image[] HpCounters;
	public Color missingHpIconColor;

	private GameObject redScoreBox, yellowScoreBox, blueScoreBox, greenScoreBox;
//	private GUIText redScoreTxt, redPlaceTxt, redAccTxt,
//						yellowScoreTxt, yellowPlaceTxt, yellowAccTxt,
//						blueScoreTxt, bluePlaceTxt, blueAccTxt,
//						greenScoreTxt, greenPlaceTxt, greenAccTxt;

	private TextMesh redScoreTxt, redAccTxt,
						yellowScoreTxt, yellowAccTxt,
						blueScoreTxt, blueAccTxt,
						greenScoreTxt, greenAccTxt;

	BallManager ballManager;
	PlayerManager playerManager;
	SpawnManager spawnManager;

	IntroGUI introGUI;

	StaticPool staticPool;

	#region Singleton Initialization
	public static GameManager instance {
		get { 
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GameManager>();
			
			return _instance;
		}
	}
	
	void Awake() {
		if(_instance == null) {
			//If I am the fist instance, make me the first Singleton
			_instance = this;
			DontDestroyOnLoad(gameObject);
			staticPool = new StaticPool(); // Fight me
		} else {
			//If a Singleton already exists and you find another reference in scene, destroy it
			if(_instance != this)
				Destroy(gameObject);
		}
	}
	#endregion

	void Start() {
		currLives = maxLives;
		introGUI = GameObject.Find("IntroGUI").GetComponent<IntroGUI>();
		ballManager = GetComponent<BallManager> ();
		playerManager = GetComponent<PlayerManager> ();

//		guiStyle = new GUIStyle();
	}

	void OnLevelWasLoaded(int level) {
		if(level == 0) {
			introGUI = GameObject.Find("IntroGUI").GetComponent<IntroGUI>();
		} else if(level == 1) {
			isGamePlaying = true;
			gameOverText = GameObject.Find( "GameOverGui" ).transform;
			gameOverText.gameObject.SetActive( true );
			spawnManager = GameObject.FindObjectOfType<SpawnManager>();

			// Get Hp Gui in scene
			HpCounters = GameObject.Find( "HpGui" ).GetComponent<HpIconHolder>().m_hpIcons;


			redScoreBox = GameObject.Find( "InGameScoreRed" );
			yellowScoreBox = GameObject.Find( "InGameScoreYellow" );
			blueScoreBox = GameObject.Find( "InGameScoreBlue" );
			greenScoreBox = GameObject.Find( "InGameScoreGreen" );

			StartCoroutine ("SpawnEnemy");
			//StartCoroutine( "StartEnemyMove" );
			GameObject.Find("GameCamera").camera.enabled = true;
//			GameObject.Find("ScoreCamera").camera.enabled = false;
			GameObject.Find("Timer").SetActive(true);

			TextMesh[] texts = redScoreBox.GetComponentsInChildren<TextMesh>();			
			foreach( TextMesh text in texts ) {
				if( text.name.Contains( "Score" ) )
					redScoreTxt = text;
				if( text.name.Contains( "Accuracy" ) )
					redAccTxt = text;
			}			
			texts = yellowScoreBox.GetComponentsInChildren<TextMesh>();			
			foreach( TextMesh text in texts ) {
				if( text.name.Contains( "Score" ) )
					yellowScoreTxt = text;
				if( text.name.Contains( "Accuracy" ) )
					yellowAccTxt = text;
			}			
			texts = blueScoreBox.GetComponentsInChildren<TextMesh>();			
			foreach( TextMesh text in texts ) {
				if( text.name.Contains( "Score" ) )
					blueScoreTxt = text;
				if( text.name.Contains( "Accuracy" ) )
					blueAccTxt = text;
			}			
			texts = greenScoreBox.GetComponentsInChildren<TextMesh>();			
			foreach( TextMesh text in texts ) {
				if( text.name.Contains( "Score" ) )
					greenScoreTxt = text;
				if( text.name.Contains( "Accuracy" ) )
					greenAccTxt = text;
			}

			redScoreBox.SetActive( false );
			yellowScoreBox.SetActive( false );
			blueScoreBox.SetActive( false );
			greenScoreBox.SetActive( false );

//			scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
		}
	}

	void Update() {
		switch(mode)
		{
		case GameMode.Intro:
			if(gameStarted) {
				if(timer > 0f) {
					introGUI.timerText.text = "Game starts in: " + Mathf.CeilToInt(timer);
					timer -= Time.deltaTime;
					if(timer <= 0f) {
						ChangeScene( "Main" );
						return;
					}
				}
			}
			break;
		case GameMode.Main:
			// Once timer goes down to zero
			if( currLives <= 0 ) {
				timer = 0f;
			}

			if(timer <= 0 && isGamePlaying == true ) {
				//EndGame();
				foreach( UnityEngine.UI.Image image in HpCounters ) {
					image.enabled = false;
				}

				isGamePlaying = false;
				gameOverText.gameObject.SetActive( true );
				StartCoroutine( "GameOverGui" );
				ballManager.StopAllCoroutines();
				StopAllCoroutines();
				timer = scoreboardTimer;
				mode = GameMode.Scoreboard;

				redScoreBox.SetActive( false );
				yellowScoreBox.SetActive( false );
				blueScoreBox.SetActive( false );
				greenScoreBox.SetActive( false );

//				GameObject.Find("GameCamera").camera.enabled = false;
//				GameObject.Find("ScoreCamera").camera.enabled = true;
				GameObject.Find("ScoreGUI").GetComponent<ScoreGUI>().Activate();
				GameObject.Find("Timer").SetActive(false);

				for(int i = 0; i < playerManager.playerData.Count; i++) {
					HighScoreManager.AddScore(playerManager.playerData[i].score);
				}

				//queueManager.Reset();

				Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
				foreach( Enemy enemy in enemies ) {
					enemy.StopAllCoroutines();
					enemy.gameObject.SetActive( false );
				}

				return;
			}

			// Update score gui
			if( redScoreBox.activeSelf == false && playerManager.Added( PlayerColor.Red ))
				redScoreBox.SetActive( true );
			if( yellowScoreBox.activeSelf == false && playerManager.Added( PlayerColor.Yellow ))
				yellowScoreBox.SetActive( true );
			if( blueScoreBox.activeSelf == false && playerManager.Added( PlayerColor.Blue ))
				blueScoreBox.SetActive( true );
			if( greenScoreBox.activeSelf == false && playerManager.Added( PlayerColor.Green ))
				greenScoreBox.SetActive( true );

			for(int i = 0; i < playerManager.playerData.Count; i++) {
				string tempScoreStr = playerManager.playerData[i].score.ToString();

				string tempAccStr;
				if( playerManager.playerData[i].totalShots > 0 ) {
					float percent = ((float)playerManager.playerData[i].shotsHit/(float)playerManager.playerData[i].totalShots) * 100f;
					tempAccStr = ((int)percent).ToString();
				} else {
					tempAccStr = "0";
				}
				
				if(tempScoreStr.Length < 2)
					tempScoreStr = " " + tempScoreStr;
				if(tempAccStr.Length < 2)
					tempAccStr = " " + tempAccStr;
				
				switch( playerManager.playerData[i].color ) {
				case PlayerColor.Red:
					redScoreTxt.text = "Score: " + tempScoreStr;
					redAccTxt.text = "Accuracy: " + tempAccStr + "%";
					break;
				case PlayerColor.Yellow:
					yellowScoreTxt.text = "Score: " + tempScoreStr;
					yellowAccTxt.text = "Accuracy: " + tempAccStr + "%";
					break;
				case PlayerColor.Green:
					greenScoreTxt.text = "Score: " + tempScoreStr;
					greenAccTxt.text = "Accuracy: " + tempAccStr + "%";
					break;
				case PlayerColor.Blue:
					blueScoreTxt.text = "Score: " + tempScoreStr;
					blueAccTxt.text = "Accuracy: " + tempAccStr + "%";
					break;
				}
			}

			timer -= Time.deltaTime;
			break;
		case GameMode.Scoreboard:
			if(timer <= 0) {
				ChangeScene( "Intro" );
				return;
			}
			timer -= Time.deltaTime;
			break;
		}
	}

	// Why the shit doesnt this work?
	string Tab(string str, int numSpaces) {
		int size = str.Length;
		if(numSpaces > str.Length)
			for(int i = 0; i < numSpaces - size; i++)
				str += " ";
		return str;
	}

	public void BallHit(ArrayList args) {
		float x = (float)(args[0]);
		x = Mathf.Abs(x - 1);
		float y = (float)(args[1]);
		y = Mathf.Abs(y - 1);
		Vector2 pos = new Vector2(x,y);

		int colorID =  (int)args[2];

		PlayerColor color = PlayerColor.Red;

		switch(colorID) {
		case 0:
			color = PlayerColor.Red;
			break;
		case 3:
			color = PlayerColor.Green;
			break;
		case 1:
			color = PlayerColor.Yellow;
			break;
		case 2:
			color = PlayerColor.Blue;
			break;
		default:
			print("Bad Color");
			break;
		}

		if(!playerManager.Added(color)) {
			playerManager.AddPlayer(color);
			if(mode == GameMode.Intro) {
				introGUI.TurnOnColor(color);
				timer = joinTimer;
				introGUI.timerText.animation.Stop();
				introGUI.timerText.transform.rotation = Quaternion.identity;
			}
		}
		if(!gameStarted) {
			gameStarted = true;
			timer = joinTimer;
		}

		if(mode == GameMode.Main) {
			pos.x *= Screen.width;
			pos.y = 1 - pos.y;
			pos.y *= Screen.height;
			ballManager.Shoot(pos, color);
		}
	}

	IEnumerator SpawnEnemy() {
		while(true) {
			//queueManager.SpawnNewEnemy( enemy );
			spawnManager.SpawnNewHordeEnemy();
			yield return new WaitForSeconds( spawnRate );
		}
	}

	IEnumerator StartEnemyMove() {
		while( true ) {
			//queueManager.StartNextInQueue();
			yield return new WaitForSeconds( 1.5f );
		}
	}

	void ChangeScene( string scene ) {
		switch( scene )
		{
		case "Intro":
			timer = 0f;
			ResetSpawnManager();
			gameStarted = false;
			playerManager.playerData.Clear();
			mode = GameMode.Intro;			
			Application.LoadLevel("newIntro");
			break;
		case "Main":
			currLives = maxLives;
			timer = gameTimer;
			mode = GameMode.Main;
			Application.LoadLevel("ZombieMain");
			break;
		}
	}

	public void OSCMessageReceived(OSC.NET.OSCMessage message){
		if(message.Address == "/shoot"){
//			message.Values[2] = "Red";
			BallHit(message.Values);  
		}
//    	if(message.Address == "/endGame"){
//      		AdjustGameSetting("Quit Game", true);
//      		timer = 0;
//    	} else if(message.Address == "/timeChange"){
//      		ArrayList args = message.Values;
//	      	print(args[0]);
//	     	string recieved = args[0].ToString();
//	     	float time;
//	     	float.TryParse(recieved, out time);
//	      	AdjustGameSetting("Game Time", time);
//    	}
	}

	public void ResetSpawnManager() {
		if( spawnManager != null ) {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
			foreach( GameObject tempEnemy in enemies ) {
				tempEnemy.SendMessage( "Reset", SendMessageOptions.DontRequireReceiver );
			}
		}
	}

	public void AdjustGameSetting(string setting, float value) {
		switch(setting) 
		{
		case "Game Time":
			gameTimer = value;
			break;
		default:
			break;
		}
	}

	public void AdjustGameSetting(string setting, bool value) {
		switch(setting)
		{
		case "Quit Game":
			ChangeScene( "Intro" );
			break;
		default:
			break;
		}
	}

	public void ReduceLives( int amount ) {
		currLives -= amount;

		HpCounters[currLives].color = missingHpIconColor;

	}

	void EndGame() {
		isGamePlaying = false;
		gameOverText.gameObject.SetActive( true );
		StartCoroutine( "GameOverGui" );
		ballManager.StopAllCoroutines();
		StopAllCoroutines();
		timer = scoreboardTimer;
		mode = GameMode.Scoreboard;
		
		redScoreBox.SetActive( false );
		yellowScoreBox.SetActive( false );
		blueScoreBox.SetActive( false );
		greenScoreBox.SetActive( false );
		
//		GameObject.Find("GameCamera").camera.enabled = false;
//		GameObject.Find("ScoreCamera").camera.enabled = true;
		GameObject.Find("ScoreGUI").GetComponent<ScoreGUI>().Activate();
		GameObject.Find("Timer").SetActive(false);
		
		for(int i = 0; i < playerManager.playerData.Count; i++) {
			HighScoreManager.AddScore(playerManager.playerData[i].score);
		}
		
		//queueManager.Reset();
		
		Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
		foreach( Enemy enemy in enemies ) {
			enemy.StopAllCoroutines();
			enemy.gameObject.SetActive( false );
		}
		
		return;
	}
	
	IEnumerator GameOverGui() {
		Vector3 startSize = gameOverText.localScale;
		Vector3 endSize = startSize* 2f;
		float timer = 0f;
		float lerpTime = 0.5f;

		while( timer < 1f ) {
			gameOverText.transform.localScale = Vector3.Lerp( startSize, endSize, timer );

			timer += Time.deltaTime / lerpTime;
			yield return null;
		}
		yield return new WaitForSeconds( 3 );
		gameOverText.gameObject.SetActive( false );
	}
}
