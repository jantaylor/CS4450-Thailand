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
	private int currentIndex;
	private int languageIndex;
	private VocabResource vocabResource;

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
		// Debug.Log(GameState.Instance.ActiveDifficulty + " + " + GameState.Instance.ActiveRound + " = " + numOptions);

		for (int i = 0; i < numOptions; i++)
		{
			var option = Instantiate(optionPrefab) as GameObject;
			option.transform.SetParent(optionsContainer.transform);
			options.Add(option);
		}

		// TODO: Center the options.
	}

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

	void PopulatePrompt()
	{
		wordPrompt.gameObject.SetActive(false);
		imagePrompt.gameObject.SetActive(false);

		switch (GameState.Instance.ActiveRound)
		{
			case -1:
			case 1:
				wordPrompt.gameObject.SetActive(true);
				Debug.Log(vocabResource.GetWord(languageIndex, currentIndex));
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

	public void CheckMatch(string word)
	{
		if (word == vocabResource.GetWord(0, currentIndex))
		{
			gameUIController.Match();
			foreach (var option in options)
			{
				option.SetActive(false);
			}
		}
	}


}
