using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class VocabGameController : MonoBehaviour {

	public GameObject optionPrefab;
	public List<GameObject> options;
	public GameObject optionsContainer;

	private VocabGameUIController gameUIController;
	private SpriteRenderer imagePrompt;
	private WordController wordPrompt;
	private VocabResource vocabResource;

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
		gameUIController = GameObject.Find("Vocab Game UI").GetComponent<VocabGameUIController>();
		imagePrompt = transform.Find("Image Prompt").gameObject.GetComponent<SpriteRenderer>();
		wordPrompt = transform.Find("Word Prompt").gameObject.GetComponent<WordController>();
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
		numOptions = GameState.Instance.ActiveDifficulty + GameState.Instance.ActiveRound;
		numOptions = (numOptions > vocabResource.Length)? vocabResource.Length : numOptions;
		numOptions = (numOptions < 2)? 2 : numOptions;
		// numOptions += 3;
		// Debug.Log(GameState.Instance.ActiveDifficulty + " + " + GameState.Instance.ActiveRound + " = " + numOptions);

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
		Vector2 newPos = new Vector2(((RectTransform)(optionsContainer.transform.parent)).rect.width / 2 - numCols * 130 / 2, optionsContainer.transform.position.y + 130 / 2 * (numRows - 1));
		Debug.Log(newPos);
		// NOTE: Look at the console log for this line then at the x and y position of the "Game Options Container" and tell me why they don't match...

		optionsContainer.transform.position = newPos;
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
			PopulatePrompt();
			return (currentIndex + change > -1 && currentIndex + change < vocabResource.Length);
		}
		else
		{
			return false;
		}
	}

	/// <sumamry>
	/// Populates the prompt based on ActiveRound. 1: Foreign Text; 2: English Text; 3: Image; 4: Audio Only
	/// </sumamry>
	void PopulatePrompt()
	{
		wordPrompt.gameObject.SetActive(false);
		imagePrompt.gameObject.SetActive(false);

		switch (GameState.Instance.ActiveRound)
		{
			case -1:
			case 1:
				wordPrompt.gameObject.SetActive(true);
				// Debug.Log(vocabResource.GetWord(languageIndex, currentIndex));
				wordPrompt.UpdateWord(vocabResource.GetWord(languageIndex, currentIndex), vocabResource.GetWordAudioPath(languageIndex, currentIndex));
				break;
			case 2:
				wordPrompt.gameObject.SetActive(true);
				wordPrompt.UpdateWord(vocabResource.GetWord(0, currentIndex), vocabResource.GetWordAudioPath(0, currentIndex));
				break;
			case 3:
				imagePrompt.gameObject.SetActive(true);
				imagePrompt.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(vocabResource.GetImagePath(currentIndex));
				break;
			case 4:
				wordPrompt.gameObject.SetActive(true);
				wordPrompt.UpdateWord("", vocabResource.GetWordAudioPath(0, currentIndex));
				break;
		}
	}

	/// <sumamry>
	/// Populates and randomizes the options so the right answer isn't always first and so there are no repeats.
	/// </sumamry>
	void PopulateOptions()
	{
		var random = new System.Random();
		int[] wordIndices = new int[options.Count];
		wordIndices[0] = currentIndex;

		// Get indices for random words as options
		for (int i = 1; i < options.Count; i++)
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

		// randomize the order of the words;
		for (int i = 0; i < options.Count; i++)
		{
			int j = random.Next(options.Count);
			int k = wordIndices[j];
			wordIndices[j] = wordIndices[options.Count - 1];
			wordIndices[options.Count - 1] = k;
		}

		// Setup the options.
		int displayType;
		for (int i = 0; i < options.Count; i++)
		{
			options[i].SetActive(true);
			options[i].GetComponent<VocabGameOption>().EnglishWord = vocabResource.GetWord(0, wordIndices[i]);
			options[i].GetComponent<VocabGameOption>().ForeignWord = vocabResource.GetWord(languageIndex, wordIndices[i]); // Change this to languageIndex
			options[i].GetComponent<VocabGameOption>().ImagePath = vocabResource.GetImagePath(wordIndices[i]);
			options[i].GetComponent<VocabGameOption>().EnglishAudioPath = vocabResource.GetWordAudioPath(0, wordIndices[i]);
			options[i].GetComponent<VocabGameOption>().ForeignAudioPath = vocabResource.GetWordAudioPath(languageIndex, wordIndices[i]);

			// Choose the type of options: See the VocabGameOption class Display() method for details.
			switch (GameState.Instance.ActiveRound)
			{
				case 1: displayType = 1; break;
				case 2: displayType = 1; break;
				case 3: displayType = 3; break;
				case 4: displayType = 4; break;
				default: displayType = 1; break;
			}
			options[i].GetComponent<VocabGameOption>().Display(displayType);
		}
	}

	/// <sumamry>
	/// Checks a word to see if it is the correct one.
	/// If it is, remove all of the other ones and call the UI Controller to enable the "next" button.
	/// If it is not, remove that option.
	/// </sumamry>
	public void CheckMatch(string word)
	{
		if (word == vocabResource.GetWord(0, currentIndex))
		{
			gameUIController.Match();
			foreach (var option in options)
			{
				if (option.GetComponent<VocabGameOption>().EnglishWord != word)
				{
					// TODO: Set the image to "_transparent"
					option.SetActive(false);
				}
			}
		}
		else
		{
			foreach (var option in options)
			{
				if (option.GetComponent<VocabGameOption>().EnglishWord == word)
				{
					// TODO: Set the image to "_transparent"
					option.SetActive(false);
				}
			}
		}
	}


}
