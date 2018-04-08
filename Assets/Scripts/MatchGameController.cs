using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class MatchGameController : MonoBehaviour {

	public GameObject optionPrefab;
	public List<GameObject> options;
	public GameObject optionsContainer;

	private MatchGameUIController gameUIController;
	private VocabResource vocabResource;
	private MatchGameOption selected;

	/// <sumamry>
	/// The index of the current vocab word (0, 1, ... - not the actual randomized index)
	/// </sumamry>
	private int currentIndex;

	/// <sumamry>
	/// The index of the language in the vocab list (0: English, 1: Thai) - this is for scalability (i.e. if we add more languages and have a language setting on the GameState)
	/// </sumamry>
	private int languageIndex;

	void Awake()
	{
		gameUIController = GetComponent<MatchGameUIController>();
		optionsContainer = GameObject.Find("Game Options Container");
		currentIndex = 0;
	}

	void Start ()
	{
		LoadJson();
		languageIndex = Array.FindIndex(vocabResource.languages, language => language.Equals(GameState.Instance.ActiveLanguage));
		SetupGame();
		Proceed(0);
	}

	void LoadJson()
	{
		vocabResource = JsonUtility.FromJson<VocabResource>(Resources.Load<TextAsset>("vocab_" + GameState.Instance.ActiveStory + "_" + GameState.Instance.ActiveDifficulty).text);
		vocabResource.GenerateRandomOrder();
	}

	void SetupGame()
	{
		int numOptions;
		switch (GameState.Instance.ActiveRound)
		{
			case 1: numOptions = 8; break;
			case 2:
			case 3: numOptions = 10; break;
			default: numOptions = 12; break;
		}
		// numOptions = (numOptions / 2 > vocabResource.Length)? vocabResource.Length : numOptions / 2;
		Debug.Log("Number of options: " + numOptions);

		for (int i = 0; i < numOptions; i++)
		{
			var option = Instantiate(optionPrefab) as GameObject;
			option.transform.SetParent(optionsContainer.transform);
			options.Add(option);
		}


		// Attempt to center the GridLayoutGroup:

		// With the GridLayoutGroup, it does a weird thing with how many columns it populates based on how many objects there are:
		// 1:1, 2:2, 3:3, 4:4, 5:3, 6:3, 7:4, 8:4, 9:3, 10+:4
		// This line has a formula that determines that without having a nasty if-else chain or switch statement.
		int numCols = numOptions <= 4? numOptions : ((numOptions % 4 == 0 || numOptions >= (1 + (int)(numOptions/4)) * 4 - (int)(numOptions/4))? 4 : 3);
		int numRows = (int)((numOptions - 1) / 4) + 1;

		// Calculates the new position:
		// x = half of the canvas width - half of the width of one column (130 / 2) times the number of columns
		// y = initial y + half of the height of one row (130 / 2) times one less than the number of rows.
		// Vector2 newPos = new Vector2(((RectTransform)(optionsContainer.transform.parent)).rect.width / 2 - numCols * 130 / 2, optionsContainer.transform.position.y + 130 / 2 * (numRows - 1)) * transform.localScale.x;
		// Debug.Log(newPos);
		// NOTE: Look at the console log for this line then at the x and y position of the "Game Options Container" and tell me why they don't match...

		// optionsContainer.transform.position = newPos;
	}

	/// <sumamry>
	/// Moves to the next vocab word
	/// </sumamry>
	public bool Proceed(int change)
	{
		if (currentIndex + change > -1 && currentIndex + change < vocabResource.Length)
		{
			currentIndex += change;
			PopulateOptions();
			// PopulatePrompt();
			return (currentIndex + change > -1 && currentIndex + change < vocabResource.Length);
		}
		else
		{
			return false;
		}
	}

	/// <sumamry>
	/// Populates and randomizes the options so there are no repeats.
	/// </sumamry>
	void PopulateOptions()
	{
		var random = new System.Random();
		int[] wordIndices = new int[options.Count / 2];
		wordIndices[0] = currentIndex;

		// Get indices for random words as options
		for (int i = 1; i < options.Count / 2; i++)
		{
			int j, temp = -1;
			do
			{
				j = random.Next(vocabResource.Length);
				// check against existing array elements:
				try
				{
					temp = Array.FindIndex(wordIndices, value => value == j);
				} catch (ArgumentNullException e)
				{
					temp = -1;
				}
			}	while (temp != -1);

			wordIndices[i] = j;
		}

		// Setup the options.
		int displayType;
		int displayType2;
		for (int i = 0; i < options.Count / 2; i++)
		{
			options[i].SetActive(true);
			options[i].GetComponent<MatchGameOption>().Enabled = true;
			options[i].GetComponent<MatchGameOption>().EnglishWord = vocabResource.GetWord(0, wordIndices[i]);
			options[i].GetComponent<MatchGameOption>().ForeignWord = vocabResource.GetWord(languageIndex, wordIndices[i]); // Change this to languageIndex
			options[i].GetComponent<MatchGameOption>().ImagePath = vocabResource.GetImagePath(wordIndices[i]);
			options[i].GetComponent<MatchGameOption>().EnglishAudioPath = vocabResource.GetWordAudioPath(0, wordIndices[i]);
			options[i].GetComponent<MatchGameOption>().ForeignAudioPath = vocabResource.GetWordAudioPath(languageIndex, wordIndices[i]);

			options[i + options.Count / 2].SetActive(true);
			options[i + options.Count / 2].GetComponent<MatchGameOption>().Enabled = true;
			options[i + options.Count / 2].GetComponent<MatchGameOption>().EnglishWord = vocabResource.GetWord(0, wordIndices[i]);
			options[i + options.Count / 2].GetComponent<MatchGameOption>().ForeignWord = vocabResource.GetWord(languageIndex, wordIndices[i]); // Change this to languageIndex
			options[i + options.Count / 2].GetComponent<MatchGameOption>().ImagePath = vocabResource.GetImagePath(wordIndices[i]);
			options[i + options.Count / 2].GetComponent<MatchGameOption>().EnglishAudioPath = vocabResource.GetWordAudioPath(0, wordIndices[i]);
			options[i + options.Count / 2].GetComponent<MatchGameOption>().ForeignAudioPath = vocabResource.GetWordAudioPath(languageIndex, wordIndices[i]);

			// Choose the type of options: See the MatchGameOption class Display() method for details.
			switch (GameState.Instance.ActiveRound)
			{
				case 1:
					displayType = MatchGameOption.DISPLAY_IMAGE;
					displayType2 = MatchGameOption.DISPLAY_FOREIGN;
					break;
				case 2:
					displayType = MatchGameOption.DISPLAY_IMAGE;
					displayType2 = MatchGameOption.DISPLAY_ENGLISH;
					break;
				case 3:
					displayType = MatchGameOption.DISPLAY_ENGLISH;
					displayType2 = MatchGameOption.DISPLAY_FOREIGN;
					break;
				case 4:
					displayType = MatchGameOption.DISPLAY_IMAGE;
					displayType2 = MatchGameOption.DISPLAY_ENGLISH;
					break;
				default:
					displayType = MatchGameOption.DISPLAY_IMAGE;
					displayType2 = MatchGameOption.DISPLAY_ENGLISH;
					break;
			}
			options[i].GetComponent<MatchGameOption>().DisplayType = displayType;
			options[i + options.Count / 2].GetComponent<MatchGameOption>().DisplayType = displayType2;

			options[i].GetComponent<MatchGameOption>().Display(MatchGameOption.DISPLAY_BLANK);
			options[i + options.Count / 2].GetComponent<MatchGameOption>().Display(MatchGameOption.DISPLAY_BLANK);
		}

		// randomize the order of the options;
		for (int i = 0; i < options.Count; i++)
		{
			int j = random.Next(options.Count);
			GameObject k = options[j];
			options[j] = options[options.Count - 1];
			options[options.Count - 1] = k;
		}

		for (int i = 0; i < options.Count; i++)
		{
			options[i].transform.SetSiblingIndex(i);
		}
	}

	/// <sumamry>
	/// Checks for a match
	/// Removes the match if there is one.
	/// </sumamry>
	/// <returns>Whether to keep the option flipped</returns>
	public IEnumerator CheckMatch(MatchGameOption option)
	{
		// If there is already one selected
		if (selected != null)
		{
			// For the sake of concurrency, set the selected one to a different variable and clear the "selected" variable.
			MatchGameOption option2 = selected;
			selected = null;

			// If there is a match:
			if (option2.EnglishWord == option.EnglishWord)
			{
				// Wait 2 sec.
				yield return new WaitForSeconds(2);

				// hide the match
				option.Display(MatchGameOption.DISPLAY_HIDDEN);
				option2.Display(MatchGameOption.DISPLAY_HIDDEN);
				option2 = null;
			}

			// If there is not a match:
			else
			{
				// Wait 2 sec.
				yield return new WaitForSeconds(2);

				// flip both back over
				option.Display(MatchGameOption.DISPLAY_BLANK);
				option2.Display(MatchGameOption.DISPLAY_BLANK);

				// re-enable both.
				option.Enabled = true;
				option2.Enabled = true;
			}
		}

		// If there is not one selected already, select the clicked one.
		else
		{
			selected = option;
		}
	}


}
