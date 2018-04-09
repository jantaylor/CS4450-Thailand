using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class VocabGameController : MonoBehaviour {

	public GameObject optionPrefab;
	public List<GameObject> options;
	public GameObject optionsContainer;

	private VocabGameUIController gameUIController;
	private Image imagePrompt;
	private WordController wordPrompt;
	private VocabResource vocabResource;
	private GameObject resultAnimation;

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
		imagePrompt = transform.Find("Image Prompt").GetComponent<Image>();
		wordPrompt = transform.Find("Word Prompt").gameObject.GetComponent<WordController>();
		resultAnimation = GameObject.Find("Result Animation");
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
		// numOptions = GameState.Instance.ActiveDifficulty + GameState.Instance.ActiveRound;
		numOptions = 4 + (GameState.Instance.ActiveRound - 1) / 2 * 4; // for rounds 1 and 2: 4 options, for 3 and 4: 8 options.
		numOptions = (numOptions > vocabResource.Length)? vocabResource.Length : numOptions;
		numOptions = (numOptions < 2)? 2 : numOptions;

		for (int i = 0; i < numOptions; i++)
		{
			var option = Instantiate(optionPrefab) as GameObject;
			option.transform.SetParent(optionsContainer.transform);
			options.Add(option);
		}
	}

	/// <summary>
	/// Waits for 'seconds' seconds, then calls Proceed()
	/// </summary>
	private IEnumerator AwaitProceed(int change, int seconds)
	{
		yield return new WaitForSeconds(seconds);
		Proceed(0);
	}

	/// <sumamry>
	/// Moves to the next vocab word
	/// </sumamry>
	public bool Proceed(int change)
	{
		resultAnimation.SetActive(false);
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
				wordPrompt.UpdateWord(vocabResource.GetWord(languageIndex, currentIndex), vocabResource.GetWordAudioPath(languageIndex, currentIndex));
				break;
			case 2:
				wordPrompt.gameObject.SetActive(true);
				wordPrompt.UpdateWord(vocabResource.GetWord(0, currentIndex), vocabResource.GetWordAudioPath(0, currentIndex));
				break;
			case 3:
				imagePrompt.gameObject.SetActive(true);
				imagePrompt.sprite = Resources.Load<Sprite>(vocabResource.GetImagePath(currentIndex));
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
					Debug.Log(e);
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
				case 1: displayType = VocabGameOption.DISPLAY_IMAGE; break;
				case 2: displayType = VocabGameOption.DISPLAY_IMAGE; break;
				case 3: displayType = VocabGameOption.DISPLAY_ENGLISH; break;
				case 4: displayType = VocabGameOption.DISPLAY_ENGLISH_SPEECH; break;
				default: displayType = VocabGameOption.DISPLAY_IMAGE; break;
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
		bool correct = word == vocabResource.GetWord(0, currentIndex);

		// Show the "Correct"/"Wrong" Animation;
		resultAnimation.SetActive(true);
		resultAnimation.GetComponent<AnimationController>().SetSprite(correct? 0 : 1);
		resultAnimation.GetComponent<AnimationController>().Play();

		if (correct)
		{
			gameUIController.Match();
			foreach (var option in options)
			{
				if (option.GetComponent<VocabGameOption>().EnglishWord != word)
				{
					option.GetComponent<VocabGameOption>().Display(VocabGameOption.DISPLAY_HIDDEN);

				}
			}
		}
		else
		{
			StartCoroutine(AwaitProceed(0, 1));
		}
	}


}
