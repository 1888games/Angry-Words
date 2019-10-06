using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using System.Threading.Tasks;

public class GridManager : MonoBehaviour
{


	public List<Level> LevelData;

	public TextMeshProUGUI LevelText;

	public string [,] GridContents;

	public Level CurrentLevel;
	public int CurrentLevelID = 0;

	public GameObject CellPrefab;
	public GameObject EmptyPrefab;
	public GameObject WordContainerPanel;
	public GameObject WordContainerPrefab;

	public GridLayoutGroup GridLayout;

	public GameObject [,] UIObjects;

	public Dictionary<Level, List<string>> LevelWords;

	int SelectedWordID = -1;

	public List<GameObject> WordContainers;
	public GameObject SelectedWordContainer;

	public Vector2Int SelectedWordPosition;
	public bool SelectedWordIsDown = false;

	public List<DisplacedLetter> DisplacedLetters;
	public List<GameObject> SelectedWordLetters;
	public List<GameObject> RedLetters;

	public Vector2Int SelectedWordOldPosition;
	public bool SelectedWordOldIsDown;

	public bool placementOk = false;

	public bool gameActive = false;

	public Color PlacedLetterColor;

	public DateTime StartTime;
	public double CompleteTime;

	GlobalstatsIO GlobalStats;

	public GameObject Leaderboard;
	public GameObject LeaderRowPrefab;
	public GameObject GameOverScreen;

	public TextMeshProUGUI NameText;

	GlobalstatsIO_Leaderboard lb;

	public bool allowGameOverQuit = false;

	public GameObject NameEntry;

	// Start is called before the first frame update
	void Start()
    {

	

		GameOverScreen.SetActive (false);
		NameEntry.SetActive (false);
		GlobalstatsIO.api_id = "X0251Fjn0zp0sDsyOWiXZfnUc2QIdocapfgqUzOV";
		GlobalstatsIO.api_secret = "eWBSoaGksDkI1eV44DhtPsS5IjZh8VqnR2ffdVAF";

		GridLayout = GetComponent<GridLayoutGroup> ();

		GlobalStats = new GlobalstatsIO ();
		lb = GlobalStats.getLeaderboard ("leaders", 10);
		

		LevelData = new List<Level> ();
		LevelWords = new Dictionary<Level, List<string>> ();

		LevelData.Add (new Level { id = 1, rows = 7, cols = 8, x = 1, y = 1, isDown = true });

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"TUTORIAL","LEVEL"});

		LevelData.Add (new Level { id = 2, rows = 12, cols = 7, x = 1, y = 1, isDown = true });


		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"THE","THEME","IS","TERRIBLE"});


		LevelData.Add (new Level { id = 3, rows = 12, cols = 7, x = 1, y = 6, isDown = true });

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"DAY","ONE","TOTAL","DISASTER"});



		LevelData.Add (new Level { id = 4, rows = 10, cols = 8, x = 2, y = 7, isDown = false });

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"HAD","ALMOST","GIVEN","UP","COMPLETELY"});

		LevelData.Add (new Level { id = 5, rows = 9, cols = 12, x = 3, y = 2, isDown = true });


		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"WATCHING","BIG","BANG","THEORY","ON","SHOWBOX"});

		LevelData.Add (new Level { id = 6, rows = 9, cols = 10, x = 4, y = 5, isDown = false });

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"WHEN","THIS","IDEA","CAME","TOGETHER"});

		LevelData.Add (new Level { id = 7, rows = 12, cols = 9, x = 3, y = 5, isDown = false });

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"THAT","WASNT","UTTERLY","TERRIBLE"});

		LevelData.Add (new Level { id = 8, rows = 8, cols = 7, x = 1, y = 8, isDown = false});

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
		{"NOW","RACE","AGAINST","TIME"});


		LevelData.Add (new Level { id = 9, rows = 7, cols = 13, x = 6, y = 1, isDown = true });

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"TO","HACK","SOMETHING","TOGETHER"});

		LevelData.Add (new Level { id = 10, rows = 9, cols = 9, x = 1, y = 8, isDown = false });

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"IF","YOURE","PLAYING","THIS"});

		LevelData.Add (new Level { id = 11, rows = 8, cols = 10, x = 4, y = 2, isDown = false });

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"AND","COMPLETE","TWELVE","LEVELS"});

		LevelData.Add (new Level { id = 12, rows = 10, cols = 11, x = 5, y = 4, isDown = true });

		LevelWords.Add (LevelData [LevelData.Count - 1], new List<string>
			{"POST","HORSE","TO","DEMONSTRATE","GENIUS"});

		CurrentLevel = LevelData [CurrentLevelID];

		StartTime = DateTime.Now;
		ClearGrid ();

    }


	public void GotName (string name) {

		NameEntry.SetActive (false);

		Dictionary<string, string> values = new Dictionary<string, string> ();
		values.Add ("leaders", CompleteTime.ToString ());

		NameText.text = NameText.text.Substring (0, NameText.text.Length - 1);

		if (NameText.text.Length < 3) {
			NameText.text = NameText.text + "___";
		}

		if (GlobalStats.share ("", NameText.text, values)) {
			// Success
		} else {
			// An Error occured
		}

		Invoke ("GetLeaderboard", 0.5f);
		Invoke ("ShowLeaderboard", 1.5f);


	}


	// Update is called once per frame
	void Update()
    {

		if (GameOverScreen.activeInHierarchy) {

			if (Input.anyKeyDown && allowGameOverQuit) {

				allowGameOverQuit = false;
				GameOverScreen.SetActive (false);
				CurrentLevelID = 0;

				ClearGrid ();

					
			}
		}



		if (Input.GetKeyDown (KeyCode.L)) {

			Application.Quit ();
		}

		if (gameActive) {

			if (SelectedWordID >= 0) {

				if (Input.GetKeyDown (KeyCode.Space) && placementOk) {

					AddWord (LevelWords [CurrentLevel] [SelectedWordID], SelectedWordPosition.x, SelectedWordPosition.y, SelectedWordIsDown);

				}

				if (Input.GetKeyDown (KeyCode.Escape)) {

					if (SelectedWordOldPosition.x > -1) {
						AddWord (LevelWords [CurrentLevel] [SelectedWordID], SelectedWordOldPosition.x, SelectedWordOldPosition.y, SelectedWordOldIsDown);
					} else {
						ClearSelectedWord ();
					}

					foreach (Transform child in WordContainerPanel.transform) {

						child.GetComponent<Image> ().color = new Color (1, 1, 1, 0f);
					}

					SelectedWordID = -1;
					SelectedWordOldPosition = new Vector2Int (-1, -1);
					SoundController.Instance.PlaySound ("escape");


				}


				if (Input.GetKeyDown (KeyCode.UpArrow)) {

					if (SelectedWordPosition.y > 0) {

						SelectedWordPosition = new Vector2Int (SelectedWordPosition.x, SelectedWordPosition.y - 1);
						//ClearSelectedWord ();
						DrawSelectedWord ();
						SoundController.Instance.PlaySound ("move");
					}

				}


				if (Input.GetKeyDown (KeyCode.R)) {

					if (SelectedWordIsDown == false) {

						if (SelectedWordPosition.y + LevelWords [CurrentLevel] [SelectedWordID].Length > (CurrentLevel.rows)) {
							return;
						}

					} else {


						if (SelectedWordPosition.x + LevelWords [CurrentLevel] [SelectedWordID].Length > (CurrentLevel.cols)) {
							return;
						}


					}




					SelectedWordIsDown = !SelectedWordIsDown;
					DrawSelectedWord ();
					SoundController.Instance.PlaySound ("moveup");
				}

				if (Input.GetKeyDown (KeyCode.DownArrow)) {

					if (SelectedWordPosition.y < CurrentLevel.rows - 1) {

						if (SelectedWordIsDown && SelectedWordPosition.y + LevelWords [CurrentLevel] [SelectedWordID].Length >= CurrentLevel.rows) {
							return;
						}

						SelectedWordPosition = new Vector2Int (SelectedWordPosition.x, SelectedWordPosition.y + 1);
						//ClearSelectedWord ();
						DrawSelectedWord ();
						SoundController.Instance.PlaySound ("move");

					}

				}


				if (Input.GetKeyDown (KeyCode.LeftArrow)) {

					if (SelectedWordPosition.x > 0) {

						SelectedWordPosition = new Vector2Int (SelectedWordPosition.x - 1, SelectedWordPosition.y);
						//ClearSelectedWord ();
						DrawSelectedWord ();
						SoundController.Instance.PlaySound ("move");
					}

				}

				if (Input.GetKeyDown (KeyCode.RightArrow)) {

					if (SelectedWordPosition.x < CurrentLevel.cols - 1) {

						if (SelectedWordIsDown == false && SelectedWordPosition.x + LevelWords [CurrentLevel] [SelectedWordID].Length >= CurrentLevel.cols) {
							return;
						}

						SelectedWordPosition = new Vector2Int (SelectedWordPosition.x + 1, SelectedWordPosition.y);
						//ClearSelectedWord ();
						DrawSelectedWord ();
						SoundController.Instance.PlaySound ("move");
					}

				}

			}

			for (int i = 1; i <= LevelWords [CurrentLevel].Count; ++i) {
				if (Input.GetKeyDown ("" + i)) {

					if (SelectedWordID != i - 1) {

						
						if (SelectedWordID > -1) {

							if (SelectedWordOldPosition.x > -1) {
								AddWord (LevelWords [CurrentLevel] [SelectedWordID], SelectedWordOldPosition.x, SelectedWordOldPosition.y, SelectedWordOldIsDown);
								Debug.Log ("PUT IT BACK LAD");
	   } else {
								ClearSelectedWord ();
							}

						}

						foreach (Transform child in WordContainerPanel.transform) {

							child.GetComponent<Image> ().color = new Color (1, 1, 1, 0f);
						}

						WordContainerPanel.transform.GetChild (i - 1).GetComponent<Image> ().color = new Color (1, 1, 1, 0.5f);



						SelectedWordID = i - 1;

						SelectedWordIsDown = false;
						SelectedWordPosition = new Vector2Int (0, 0);

						SearchForExistingWord (LevelWords [CurrentLevel] [SelectedWordID]);
						DrawSelectedWord ();

						SoundController.Instance.PlaySound ("new");

					}

					Debug.Log (i);
				}
			}

		}



        
    }




	async Task ShowLeaderboard () {


		List<GlobalstatsIO_LeaderboardValue> entries = new List<GlobalstatsIO_LeaderboardValue> ();

		for (int i = 0; i < 10; i++) {

			entries.Add (new GlobalstatsIO_LeaderboardValue { rank = (i + 1).ToString (), name = "LD45", value = "9999999" });

		}

		Debug.Log ("LB DATA" + lb.data.Length);

		for (int i = 0; i < lb.data.Length; i++) {

			GlobalstatsIO_LeaderboardValue data = lb.data [i];

			entries [i] = data;

		}

		for (int i = 0; i < entries.Count; i++) {

			LeaderRow lr = Leaderboard.transform.GetChild (i).GetComponent<LeaderRow> ();

			GlobalstatsIO_LeaderboardValue entry = entries [i];

			lr.Rank.text = entry.rank;
			lr.Name.text = entry.name;

			float secondsFloat = 0;

			float.TryParse (entry.value, out secondsFloat);

			secondsFloat = secondsFloat / 1000f;

			int seconds = Mathf.RoundToInt (secondsFloat);

			int minutes = Mathf.RoundToInt(secondsFloat / 60f);

			int secondsRemaining = seconds % 60;

			lr.Time.text = minutes + " : " + secondsRemaining;

		}

		GameOverScreen.SetActive (true);

		allowGameOverQuit = false;
		Invoke ("AllowGameOver", 3f);


	}

	void AllowGameOver () {


		allowGameOverQuit = true;

	}

	async void CheckLevelComplete () {

		bool levelComplete = true;

		foreach (string word in LevelWords [CurrentLevel]) {

			List<GameObject> wordObjects = GetObjectsForWord (word);

			if (wordObjects.Count == 0) {
				levelComplete = false;
			} else {

				bool wordConnected = false;

				for (int i = 0; i < wordObjects.Count; i++) {

					GameObject go = wordObjects [i];

					if (go.name.Length > word.Length) {
						wordConnected = true;
					}

				}

				if (wordConnected == false) {

					levelComplete = false;
				}
			}
		}


		Debug.Log ("LEVEL COMPLETE: " + levelComplete);

		if (levelComplete) {


			SoundController.Instance.StopSound ("music");
			SoundController.Instance.PlaySound ("complete");


			if (CurrentLevelID < 11) {
				CurrentLevelID++;
				CurrentLevel = LevelData [CurrentLevelID];
				Invoke ("ClearGrid", 1.5f);
			} else {

				TimeSpan ts = (DateTime.Now - StartTime);
				CompleteTime = ts.TotalMilliseconds;

				NameEntry.SetActive (true);
				

			}
		}

	


	}

	public void GetLeaderboard () {

		lb = GlobalStats.getLeaderboard ("leaders", 10);
	}

	public void ClearSelectedWord () {

		if (SelectedWordID >= 0) {

			for (int i = DisplacedLetters.Count - 1; i >= 0; i--) {

				DisplacedLetter dl = DisplacedLetters [i];


				dl.tempGo.transform.SetParent (null);
				Destroy (dl.tempGo);
				dl.go.SetActive (true);
				dl.go.transform.SetParent (this.transform);
				dl.go.transform.SetSiblingIndex (dl.index);


			//	Debug.Log (dl.index + " / " + dl.go.transform.GetSiblingIndex ());

			}

		} else {
			DisplacedLetters = new List<DisplacedLetter> ();
		}


		foreach (GameObject go in RedLetters) {
			go.GetComponent<Image> ().color = PlacedLetterColor;
		}

		//Debug.Log ("DISP: " + DisplacedLetters.Count);

		this.transform.DetachChildren ();

		for (int y = 0; y < CurrentLevel.rows; y++) {

			for (int x = 0; x < CurrentLevel.cols; x++) {

				UIObjects [x, y].transform.SetParent (this.transform);
			}
		}

		SelectedWordLetters = new List<GameObject> ();
		RedLetters = new List<GameObject> ();
		DisplacedLetters = new List<DisplacedLetter> ();


	}


	public List<GameObject> GetObjectsForWord (string word) {

		List<GameObject> wordObjects = new List<GameObject> ();

		for (int i = 0; i < this.transform.childCount; i++) {

			GameObject go = this.transform.GetChild (i).gameObject;

			if (go.name.Contains (word)) {
				wordObjects.Add (go);
			}


		}

		return wordObjects;


	}


	public void SearchForExistingWord (string word) {

		List<GameObject> wordObjects = GetObjectsForWord(word);

		SelectedWordOldPosition = new Vector2Int (-1, -1);
		Debug.Log ("RESET OLD SEARCH");


		if (wordObjects.Count == 0) {

			if (SelectedWordPosition.x + LevelWords [CurrentLevel] [SelectedWordID].Length > (CurrentLevel.cols)) {
				SelectedWordIsDown = true;
			}

		}

		for (int i = 0; i < wordObjects.Count; i++) {

			GameObject go = wordObjects [i];

			int useX = -1;
			int useY = -1;

			if (i == 0) {


				SelectedWordOldIsDown = false;

				int index1 = go.transform.GetSiblingIndex ();
				int index2 = wordObjects[1].transform.GetSiblingIndex ();

				if (index2 - index1 > 1) {
					SelectedWordIsDown = true;
					SelectedWordOldIsDown = true;
				}
		

			}

			for (int x = 0; x < CurrentLevel.cols; x++) {

				for (int y = 0; y < CurrentLevel.rows; y++) {

					if (UIObjects [x, y] == go) {
						//Debug.Log ("FOUND IT KEITH..." + x + " " + y);
						useX = x;
						useY = y;
						if (i == 0) {
							SelectedWordPosition = new Vector2Int (x, y);
							SelectedWordOldPosition = new Vector2Int (x, y);
							Debug.Log ("SET OLD POS" + SelectedWordOldPosition);
						}
						break;
					}

				}

			}

			
			if (go.name.Length > word.Length) {
				go.name = go.name.Replace (word, "");
			} else {

				

				int index = go.transform.GetSiblingIndex ();

				go.transform.SetParent (null);

				Destroy (go);

				GameObject newGo = Instantiate (EmptyPrefab);
				newGo.transform.SetParent (this.transform);

				newGo.transform.SetSiblingIndex (index);

				newGo.name = "EMPTY CELL";
				GridContents [useX, useY] = "";

				UIObjects [useX, useY] = newGo;

				newGo.transform.GetComponent<Transform> ().localScale = Vector3.one;
				


			}
		}



	}


	public void PlaceSelectedWord () {



	}


	public void DrawSelectedWord () {

		ClearSelectedWord ();
		SelectedWordLetters = new List<GameObject> ();
		RedLetters = new List<GameObject> ();
		placementOk = true;

		bool wordConnects = false;

		DisplacedLetters = new List<DisplacedLetter> ();

		int x = SelectedWordPosition.x;
		int y = SelectedWordPosition.y;

		
		char [] array = LevelWords[CurrentLevel][SelectedWordID].ToCharArray ();

		for (int i = 0; i < array.Length; i++) {

			if (x < CurrentLevel.cols && y < CurrentLevel.rows) {

				string currentLetter = GridContents [x, y].ToString ();

				//GridContents [x, y] = array [i].ToString ();

				GameObject oldGo = UIObjects [x, y];

				

				int index = oldGo.transform.GetSiblingIndex ();

				//Debug.Log (x + " " + y + " " + currentLetter + " " + array [i].ToString ());

				if (currentLetter != "" && currentLetter != array [i].ToString ()) {

					PlacedLetterColor = oldGo.GetComponent<Image> ().color;
					oldGo.GetComponent<Image> ().color = new Color (0.75f, 0, 0);
					placementOk = false;
					RedLetters.Add (oldGo);
				
					//Debug.Log ("KEEP" + x + " " + y + " " + index);

				} else {



					if (currentLetter == array [i].ToString ()) {

						wordConnects = true;
					} else {

						

						if (SelectedWordIsDown == false) {

							if (i == 0 && x > 0) {

								string letter = GridContents [x - 1, y];

								if (letter != "") {
									placementOk = false;
								}
							}

							if (i == array.Length -1 && x < CurrentLevel.cols - 1) {

								string letter = GridContents [x + 1, y];

								if (letter != "") {
									placementOk = false;
								}
							}

							if (y > 0) {
								string letter = GridContents [x, y - 1];

								if (letter != "") {
									placementOk = false;
								}
							}

							if (y < CurrentLevel.rows - 1) {

								string letter = GridContents [x, y + 1];

								if (letter != "") {
									placementOk = false;
								}
							}


						} else {

							if (i == 0 && y > 0) {

								string letter = GridContents [x, y - 1];

								if (letter != "") {
									placementOk = false;
								}
							}

							if (i == array.Length - 1 && y < CurrentLevel.rows - 1) {

								string letter = GridContents [x, y + 1];

								if (letter != "") {
									placementOk = false;
								}
							}

							if (x > 0) {
								string letter = GridContents [x - 1, y];

								if (letter != "") {
									placementOk = false;
								}
							}

							if (x < CurrentLevel.cols - 1) {

								string letter = GridContents [x + 1, y];

								if (letter != "") {
									placementOk = false;
								}
							}


						}

					}


					//Debug.Log ("REPLACE" + x + " " + y + " " + index);
					oldGo.transform.SetParent (null);
					oldGo.SetActive (false);

					oldGo.transform.GetSiblingIndex ();

					GameObject newGo = Instantiate (CellPrefab);
					newGo.transform.SetParent (this.transform);

					newGo.transform.SetSiblingIndex (index);

					DisplacedLetters.Add (new DisplacedLetter { go = oldGo, index = index, tempGo = newGo });
				

					newGo.GetComponentInChildren<TextMeshProUGUI> ().text = array [i].ToString ();

					newGo.name = LevelWords [CurrentLevel] [SelectedWordID];

					newGo.GetComponent<Image> ().color = new Color (0, 0.75f, 0);

					

					newGo.transform.GetComponent<Transform> ().localScale = Vector3.one;

					SelectedWordLetters.Add (newGo);

				}

			
				if (SelectedWordIsDown) {
					y = y + 1;
				} else {
					x = x + 1;
				}

			} else {
				placementOk = false;
			}

			Color color = new Color (0, 0.75f, 0);

			if (placementOk == false) {
				color = new Color (0.75f, 0f, 0f);
			} else {

				if (wordConnects == false) {

					color = new Color (0.75f, 0.375f, 0f);

				}

			}


			

			foreach (GameObject go in SelectedWordLetters) {
				go.GetComponent<Image> ().color = color;


			}
		}




	}


	public void ClearGrid () {

		if (CurrentLevelID == 0) {

			StartTime = DateTime.Now ;

		}	

		SoundController.Instance.PlaySound ("music");
		SelectedWordOldPosition = new Vector2Int (-1, -1);
		Debug.Log ("RESET OLD CLEAR");

		LevelText.text = "LEVEL " + CurrentLevel.id;
		foreach (Transform child in this.transform) {

			Destroy (child.gameObject);

		}

		foreach (Transform child in WordContainerPanel.transform) {

			Destroy (child.gameObject);

		}

		GridContents = new string [CurrentLevel.cols, CurrentLevel.rows];
		UIObjects = new GameObject [CurrentLevel.cols, CurrentLevel.rows];

		for (int x = 0; x < CurrentLevel.cols; x++) {

			for (int y = 0; y < CurrentLevel.rows; y++) {
				GridContents [x, y] = "";
			}
		}

		GridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

		GridLayout.constraintCount = CurrentLevel.cols;

		for (int y = 0; y < CurrentLevel.rows; y++) {

			for (int x = 0; x < CurrentLevel.cols; x++) {

				GameObject go = Instantiate (EmptyPrefab);

				go.transform.SetParent (this.transform);
				go.name = "EMPTY CELL";

				go.transform.GetComponent<Transform> ().localScale = Vector3.one;
			
				UIObjects [x, y] = go;

			}


		}

		float xSize = 20f + 45f * CurrentLevel.cols;
		float ySize = 20f + 45f * CurrentLevel.rows;


		GridLayout.GetComponent<RectTransform> ().sizeDelta = new Vector2 (xSize,ySize);

		GridLayout.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xSize / 2, 0f);

		SelectedWordPosition = new Vector2Int (0, 0);


		AddNothing ();
		AddWordContainers ();

		DisplacedLetters = new List<DisplacedLetter> ();
		RedLetters = new List<GameObject> ();

		//DrawSelectedWord ();

		gameActive = true;



	}





	public void AddWord(string word, int x, int y, bool isDown) {

		ClearSelectedWord ();

		DisplacedLetters = new List<DisplacedLetter> ();
		SelectedWordLetters = new List<GameObject> ();
		RedLetters = new List<GameObject> ();
	

		char [] array = word.ToCharArray ();

		for (int i = 0; i < array.Length; i++) {

			GridContents [x, y] = array [i].ToString ();

			GameObject oldGo = UIObjects [x, y];

			int index = oldGo.transform.GetSiblingIndex ();

			oldGo.transform.SetParent (null);

			Destroy (oldGo);

			GameObject newGo = Instantiate (CellPrefab);
			newGo.transform.SetParent (this.transform);

			newGo.transform.SetSiblingIndex (index);

			newGo.GetComponentInChildren<TextMeshProUGUI> ().text = array [i].ToString ();



			newGo.name = word;

			if (oldGo.name != "EMPTY CELL") {

				newGo.name += oldGo.name;

			}

			UIObjects [x, y] = newGo;

			newGo.transform.GetComponent<Transform> ().localScale = Vector3.one;

			if (isDown) {
				y = y + 1;
			} else {
				x = x + 1;
			}
		}

		SelectedWordID = -1;
		SelectedWordOldPosition = new Vector2Int (-1, -1);
		Debug.Log ("RESET OLD ADDWORD");

		CheckLevelComplete ();


		foreach (Transform child in WordContainerPanel.transform) {

			child.GetComponent<Image> ().color = new Color (1, 1, 1, 0f);
		}

		SoundController.Instance.PlaySound ("place");


	}

	public void AddNothing () {

		int x = CurrentLevel.x - 1;
		int y = CurrentLevel.y - 1;

		char [] array = "NOTHING".ToCharArray ();

		for (int i = 0; i < array.Length; i++) {

			GridContents [x, y] = array [i].ToString ();

			GameObject oldGo = UIObjects [x, y];

			int index = oldGo.transform.GetSiblingIndex ();

			oldGo.transform.SetParent (null);

			Destroy (oldGo);

			GameObject newGo = Instantiate (CellPrefab);
			newGo.transform.SetParent (this.transform);

			newGo.transform.SetSiblingIndex (index);

			newGo.GetComponentInChildren<TextMeshProUGUI> ().text = array [i].ToString ();

			newGo.name = "NOTHING";

			UIObjects [x, y] = newGo;

			newGo.transform.GetComponent<Transform> ().localScale = Vector3.one;

			if (CurrentLevel.isDown) {
				y = y + 1;
			} else {
				x = x + 1;
			}
		}


	}


	public void AddWordContainers () {

		for (int i = 0; i < LevelWords [CurrentLevel].Count; i++) {

			GameObject go = Instantiate (WordContainerPrefab);

			go.transform.SetParent (WordContainerPanel.transform);
			go.transform.GetComponent<Transform> ().localScale = Vector3.one;

			go.GetComponent<Image> ().color = new Color (0, 0, 0, 0);

			char [] array = ((i + 1).ToString () + " " + LevelWords [CurrentLevel] [i]).ToCharArray ();

			GridLayoutGroup gridLayout = go.GetComponent<GridLayoutGroup> ();

			gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

			gridLayout.constraintCount = array.Length;


			for (int j = 0; j < array.Length; j++) {


				if (array [j].ToString () != " ") {

					GameObject newGo = Instantiate (CellPrefab);

					newGo.transform.SetParent (go.transform);
					newGo.transform.GetComponent<Transform> ().localScale = Vector3.one;
					newGo.GetComponentInChildren<TextMeshProUGUI> ().text = array [j].ToString ();
				} else {
					GameObject newGo = Instantiate (EmptyPrefab);

					newGo.transform.SetParent (go.transform);
					newGo.transform.GetComponent<Transform> ().localScale = Vector3.one;

				}
				

				

			}

		}


	}
}
