using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	enum GameMode {Intro, Main, Scoreboard, Config}

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

	bool inBetweenRounds = false;
	int round = 1;
	const int maxRounds = 8;

	[System.NonSerialized]
	public float timer = 0f;

//	GUIStyle guiStyle;
	public Font guiFont;
	public float guiLeft = 0.2f;
	public float guiTop = 0.8f;
	public Transform gameOverText;

	public UnityEngine.UI.Image[] HpCounters;
	public Color missingHpIconColor;
	
	public List<HealthContainer> healthContainers;

	bool gameOverAnimDone = false;
	Text waveText;
	Text redScoreText;
	Text greenScoreText;
	Text redScoreTextShadow;
	Text greenScoreTextShadow;
	Text highScoreText;
	Text waveAchievedText;
	Text continueTimeText;
	GameObject redCrown;
	GameObject greenCrown;
	GameObject gameOverUI;
	Image redCircle;
	Image greenCircle;

//	private GameObject redScoreBox, yellowScoreBox, blueScoreBox, greenScoreBox;
//	private GUIText redScoreTxt, redPlaceTxt, redAccTxt,
//						yellowScoreTxt, yellowPlaceTxt, yellowAccTxt,
//						blueScoreTxt, bluePlaceTxt, blueAccTxt,
//						greenScoreTxt, greenPlaceTxt, greenAccTxt;

//	private TextMesh redScoreTxt, redAccTxt,
//						yellowScoreTxt, yellowAccTxt,
//						blueScoreTxt, blueAccTxt,
//						greenScoreTxt, greenAccTxt;

	BallManager ballManager;
	PlayerManager playerManager;
	SpawnManager spawnManager;
	ScreenEffectManager screenEffects;

//	IntroGUI introGUI;

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
//		introGUI = GameObject.Find("IntroGUI").GetComponent<IntroGUI>();
		ballManager = GetComponent<BallManager> ();
		playerManager = GetComponent<PlayerManager> ();

//		guiStyle = new GUIStyle();
	}

	void OnLevelWasLoaded(int level) 
	{
		if(level == 0) 
		{
//			introGUI = GameObject.Find("IntroGUI").GetComponent<IntroGUI>();
		} 
		else if(level == 1) 
		{
			isGamePlaying = true;
			gameOverText = GameObject.Find( "GameOverGui" ).transform;
			gameOverText.gameObject.SetActive( true );
			spawnManager = GameObject.FindObjectOfType<SpawnManager>();
			screenEffects = spawnManager.gameObject.GetComponent<ScreenEffectManager>();

			// Get Hp Gui in scene
			HpCounters = GameObject.Find( "HpGui" ).GetComponent<HpIconHolder>().m_hpIcons;


			gameOverUI = GameObject.Find("GameOver_Canvas");
			waveText = GameObject.Find("Wave Text").GetComponent<Text>();
			redScoreText = GameObject.Find("Red Score").GetComponent<Text>();
			greenScoreText = GameObject.Find("Green Score").GetComponent<Text>();
			redScoreTextShadow = GameObject.Find("Red Score Shadow").GetComponent<Text>();
			greenScoreTextShadow = GameObject.Find("Green Score Shadow").GetComponent<Text>();
			redCrown = GameObject.Find("Crown_Red");
			greenCrown = GameObject.Find("Crown_Green");
			waveAchievedText = GameObject.Find("Wave").GetComponent<Text>();
			highScoreText = GameObject.Find("HighScore").GetComponent<Text>();
			continueTimeText = GameObject.Find("ContinueTime").GetComponent<Text>();
			greenCircle = GameObject.Find("Moon Green").GetComponent<Image>();
			redCircle = GameObject.Find("Moon Red").GetComponent<Image>();
			
			for( int i = 1; i < 6; ++i )
			{
				healthContainers.Add( GameObject.Find( "HealthContainer_" + i ).GetComponent<HealthContainer>() );
			}
			
			playerManager.GetText();
			round = 1;
			gameOverUI.SetActive(false);

//			redScoreBox = GameObject.Find( "InGameScoreRed" );
//			yellowScoreBox = GameObject.Find( "InGameScoreYellow" );
//			blueScoreBox = GameObject.Find( "InGameScoreBlue" );
//			greenScoreBox = GameObject.Find( "InGameScoreGreen" );

//			StartCoroutine ("SpawnEnemy");
			StartNextRound();
			//StartCoroutine( "StartEnemyMove" );
			GameObject.Find("GameCamera").GetComponent<Camera>().enabled = true;
//			GameObject.Find("ScoreCamera").camera.enabled = false;
			//GameObject.Find("Timer").SetActive(true);
			//GameObject.Find("Timer").SetActive(false);

//			TextMesh[] texts = redScoreBox.GetComponentsInChildren<TextMesh>();			
//			foreach( TextMesh text in texts ) {
//				if( text.name.Contains( "Score" ) )
//					redScoreTxt = text;
//				if( text.name.Contains( "Accuracy" ) )
//					redAccTxt = text;
//			}			
//			texts = yellowScoreBox.GetComponentsInChildren<TextMesh>();			
//			foreach( TextMesh text in texts ) {
//				if( text.name.Contains( "Score" ) )
//					yellowScoreTxt = text;
//				if( text.name.Contains( "Accuracy" ) )
//					yellowAccTxt = text;
//			}			
//			texts = blueScoreBox.GetComponentsInChildren<TextMesh>();			
//			foreach( TextMesh text in texts ) {
//				if( text.name.Contains( "Score" ) )
//					blueScoreTxt = text;
//				if( text.name.Contains( "Accuracy" ) )
//					blueAccTxt = text;
//			}			
//			texts = greenScoreBox.GetComponentsInChildren<TextMesh>();			
//			foreach( TextMesh text in texts ) {
//				if( text.name.Contains( "Score" ) )
//					greenScoreTxt = text;
//				if( text.name.Contains( "Accuracy" ) )
//					greenAccTxt = text;
//			}

//			redScoreBox.SetActive( false );
//			yellowScoreBox.SetActive( false );
//			blueScoreBox.SetActive( false );
//			greenScoreBox.SetActive( false );

//			scoreText = GameObject.Find("ScoreText").GetComponent<TextMesh>();
		}
		else if( level == 2 ) //config
		{
			OSCSender.SendEmptyMessage("/config/start");
			mode = GameMode.Config;
		}
	}

	void Update() {
		switch(mode)
		{
		case GameMode.Intro:
			if( Input.GetKeyDown(KeyCode.C) )
				ChangeScene("Config");

			if(gameStarted) {
				if(timer > 0f) {
//					introGUI.timerText.text = "Game starts in: " + Mathf.CeilToInt(timer);
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
			if( Input.GetKeyDown(KeyCode.C) )
				ChangeScene("Config");
				
			if( Input.GetKeyDown(KeyCode.BackQuote) )
				ShatterHealthContainer();//temp for debugging health containers

			if( currLives <= 0 ) 
			{
				EndGame();
				//timer = 0f;
			}

			if( inBetweenRounds )
			{
				if( timer <= 0.0f )
					StartNextRound();

				timer -= Time.deltaTime;
			}
			else if( spawnManager.WaveOverCheck() )
			{
				Debug.Log("Wave is over in GM");
				RoundOver();
			}

//			if(timer <= 0 && isGamePlaying == true ) 
//			{
//				//EndGame();
//				foreach( UnityEngine.UI.Image image in HpCounters ) {
//					image.enabled = false;
//				}
//
//				isGamePlaying = false;
//				gameOverText.gameObject.SetActive( true );
//				StartCoroutine( "GameOverGui" );
//				ballManager.StopAllCoroutines();
//				StopAllCoroutines();
//				timer = scoreboardTimer;
//				mode = GameMode.Scoreboard;
//
//				redScoreBox.SetActive( false );
//				yellowScoreBox.SetActive( false );
//				blueScoreBox.SetActive( false );
//				greenScoreBox.SetActive( false );
//
////				GameObject.Find("GameCamera").camera.enabled = false;
////				GameObject.Find("ScoreCamera").camera.enabled = true;
//				GameObject.Find("ScoreGUI").GetComponent<ScoreGUI>().Activate();
//				GameObject.Find("Timer").SetActive(false);
//
//				for(int i = 0; i < playerManager.playerData.Count; i++) {
//					HighScoreManager.AddScore(playerManager.playerData[i].score);
//				}
//
//				//queueManager.Reset();
//
//				Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
//				foreach( Enemy enemy in enemies ) {
//					enemy.StopAllCoroutines();
//					enemy.gameObject.SetActive( false );
//				}
//
//				return;
//			}

			// Update score gui
//			if( redScoreBox.activeSelf == false && playerManager.Added( PlayerColor.Red ))
//				redScoreBox.SetActive( true );
////			if( yellowScoreBox.activeSelf == false && playerManager.Added( PlayerColor.Yellow ))
////				yellowScoreBox.SetActive( true );
////			if( blueScoreBox.activeSelf == false && playerManager.Added( PlayerColor.Blue ))
////				blueScoreBox.SetActive( true );
//			if( greenScoreBox.activeSelf == false && playerManager.Added( PlayerColor.Green ))
//				greenScoreBox.SetActive( true );

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
				
//				switch( playerManager.playerData[i].color ) {
//				case PlayerColor.Red:
//					redScoreTxt.text = "Score: " + tempScoreStr;
//					redAccTxt.text = "Accuracy: " + tempAccStr + "%";
					break;
//				case PlayerColor.Yellow:
//					yellowScoreTxt.text = "Score: " + tempScoreStr;
//					yellowAccTxt.text = "Accuracy: " + tempAccStr + "%";
//					break;
//				case PlayerColor.Green:
//					greenScoreTxt.text = "Score: " + tempScoreStr;
//					greenAccTxt.text = "Accuracy: " + tempAccStr + "%";
//					break;
//				case PlayerColor.Blue:
//					blueScoreTxt.text = "Score: " + tempScoreStr;
//					blueAccTxt.text = "Accuracy: " + tempAccStr + "%";
//					break;
//				}
			}

			//timer -= Time.deltaTime;
			break;
		case GameMode.Scoreboard:

			if( !gameOverAnimDone ) 
			{
				int redScore = playerManager.GetScore(PlayerColor.Red);
				int greenScore = playerManager.GetScore(PlayerColor.Green);
				float combinedScore = (float)( greenScore + redScore );

				if( combinedScore > 0 )
				{
					greenCircle.fillAmount = Mathf.Lerp( 0, greenScore/combinedScore, scoreboardTimer - timer - 2);
					redCircle.fillAmount = Mathf.Lerp( 0, redScore/combinedScore, scoreboardTimer - timer - 2);
				}

				if( scoreboardTimer - timer - 2 >= 1 )
					gameOverAnimDone = true;
			}
			continueTimeText.text = ((int)timer).ToString();
			if(timer <= 0) {
				ChangeScene( "Intro" );
				return;
			}
			timer -= Time.deltaTime;
			break;
		case GameMode.Config:
			if( Input.GetKeyDown(KeyCode.V) )
				ChangeScene("Intro");
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
//		case 1:
//			color = PlayerColor.Yellow;
//			break;
//		case 2:
//			color = PlayerColor.Blue;
//			break;
		default:
			print("Bad Color");
			break;
		}

		if(!playerManager.Added(color)) {
			playerManager.AddPlayer(color);
			if(mode == GameMode.Intro) {
//				introGUI.TurnOnColor(color);
				timer = joinTimer;
//				introGUI.timerText.GetComponent<Animation>().Stop();
//				introGUI.timerText.transform.rotation = Quaternion.identity;
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

	public void RoundOver() //called by spawner manager when wave is over
	{
		round ++;
		if( round > maxRounds )
		{
			EndGame();
			return;
		}
		Debug.Log("GM round over");
		timer = 3f;
		inBetweenRounds = true;
	}

	void StartNextRound()
	{
		Debug.Log("GM staring new round");
		waveText.text = round.ToString();
		inBetweenRounds = false;
		spawnManager.NewSpawnRound( round );
	}

//	IEnumerator SpawnEnemy() {
//		while(true) {
//			//queueManager.SpawnNewEnemy( enemy );
//			//spawnManager.SpawnNewHordeEnemy();
//			yield return new WaitForSeconds( spawnRate );
//		}
//	}

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
			//timer = gameTimer;
			mode = GameMode.Main;
			Application.LoadLevel("ZombieMain");
			break;
		case "Config":
			mode = GameMode.Config;
			Application.LoadLevel("Config");
			break;
		}
	}

	public void OSCMessageReceived(OSC.NET.OSCMessage message)
	{
		Debug.Log("The message I recieved was: " + message.Address);
		if(message.Address == "/shoot"){
//			message.Values[2] = "Red";
			BallHit(message.Values);  
		}

		if(message.Address == "/config/done") 
		{
	//		print ("config done");
			if(mode == GameMode.Config)
				ChangeScene("Intro");//Application.LoadLevel("Intro");
			//mode = GameMode.STANDBY;
		} 
		else if(message.Address == "/config/start") 
		{
			if(mode != GameMode.Config) 
			{
//				StopAllCoroutines();
//				Enemy[] enemies = FindObjectsOfType<Enemy>();
//				foreach(Enemy enemy in enemies) 
//				{
//					enemy.StopAllCoroutines();
//				}
//
//				Spider[] spiders = FindObjectsOfType<Spider>();
//				foreach(Spider spider in spiders) 
//				{
//					spider.StopAllCoroutines();
//				}
//
//				StaticPool.DestroyAllObjects();

				//mode = GameMode.CONFIG;
				ChangeScene("Config");//Application.LoadLevel("Config");
			}
		} 
//		else if(message.Address == "/config/noKinect") 
//		{
//			if(mode == GameMode.CONFIG) 
//			{
//				kinectErrorObj.SetActive(true);
//			}
//		} 
//		else if(message.Address == "/config/kinectFound") 
//		{
//			if(mode == GameMode.CONFIG) 
//			{
//				kinectErrorObj.SetActive(false);
//			}
//		}
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

	public void ReduceLives( int amount, BaseEnemy enemy ) 
	{
		currLives -= amount;
		HpCounters[currLives].color = missingHpIconColor;
		
		ShatterHealthContainer();
		
		screenEffects.damageScreen(enemy);
		
		if( enemy.GetComponent<BasicZombie>() )
		{
			screenEffects.ZombieHitEffect();
		}
		else if( enemy.GetComponent<FlyingZombie>() )
		{
			screenEffects.FlyingZombieHitEffect();
		}
		else if( enemy.GetComponent<Werewolf>() )
		{
			Debug.Log("Werewolf hit");
			screenEffects.WerewolfHitEffect();
		}

	}
	
	void ShatterHealthContainer()
	{
		foreach( HealthContainer hc in healthContainers )
		{
			if( !hc.IsShattered() )
			{
				hc.Shatter();
				return;
			}
		}
	}

	void EndGame() 
	{
		isGamePlaying = false;
		gameOverText.gameObject.SetActive( true );
		//StartCoroutine( "GameOverGui" );
		ballManager.StopAllCoroutines();
		StopAllCoroutines();
		timer = scoreboardTimer;
		mode = GameMode.Scoreboard;

		gameOverUI.SetActive(true);
		gameOverAnimDone = false;

		int rScore = playerManager.GetScore(PlayerColor.Red);
		int gScore = playerManager.GetScore(PlayerColor.Green);

		redCrown.SetActive(false);
		greenCrown.SetActive(false);

		if( rScore >= gScore )
			redCrown.SetActive(true);
		if( gScore >= rScore )
			greenCrown.SetActive(true);

		redScoreText.text = rScore.ToString();
		greenScoreText.text = gScore.ToString();
		redScoreTextShadow.text = rScore.ToString();
		greenScoreTextShadow.text = gScore.ToString();
		waveAchievedText.text = round.ToString();
		continueTimeText.text = timer.ToString();
		highScoreText.text = "99";

		greenCircle.fillAmount = 0;
		redCircle.fillAmount = 0;
		
//		redScoreBox.SetActive( false );
//		yellowScoreBox.SetActive( false );
//		blueScoreBox.SetActive( false );
//		greenScoreBox.SetActive( false );
		
//		GameObject.Find("GameCamera").camera.enabled = false;
//		GameObject.Find("ScoreCamera").camera.enabled = true;
//		GameObject.Find("ScoreGUI").GetComponent<ScoreGUI>().Activate();
//		GameObject.Find("Timer").SetActive(false);
		
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
	
//	IEnumerator GameOverGui() {
//		Vector3 startSize = gameOverText.localScale;
//		Vector3 endSize = startSize* 2f;
//		float timer = 0f;
//		float lerpTime = 0.5f;
//
//		while( timer < 1f ) {
//			gameOverText.transform.localScale = Vector3.Lerp( startSize, endSize, timer );
//
//			timer += Time.deltaTime / lerpTime;
//			yield return null;
//		}
//		yield return new WaitForSeconds( 3 );
//		gameOverText.gameObject.SetActive( false );
//	}
}
