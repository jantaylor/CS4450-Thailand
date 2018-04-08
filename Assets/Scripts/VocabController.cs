using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class VocabController : SpeechHandler {

	/// <summary>
	/// activeRound is used to determine the availability of foreign language help - from GameState
	/// </summary>
	public int activeRound;

	/// <summary>
	/// activeLanguage is used to determine which language to pull from vocab - from GameState
	/// </summary>
	public string activeLanguage;

	/// <summary>
	/// activeDifficulty is used to determine which vocab list to use - from GameState
	/// </summary>
	public int activeDifficulty;

	/// <summary>
	/// activeStory is used to determine which vocab list to use - from GameState
	/// </summary>
	public string activeStory;

	/// <summary>
	/// Whether to display English text. 0: no; 1: yes
	/// </summary>
	public int english;

	/// <summary>
	/// Whether foreign (thai) help is available: -1: no; 0: yes; 1: yes, visible by default.
	/// </summary>
	public int foreignHelp;

	/// <summary>
	/// Whether speech is enabled/require: -1: no; 0: available (practice); 1: required.
	/// </summary>
	public int speech;

	private SpriteRenderer image;
	private WordController englishWord;
	private WordController foreignWord;
	private WordController englishDefinition;
	private WordController foreignDefinition;
	private HelpController foreignHelpIcon;
	private VocabUIController uiController;

	private string vocabJsonFile;
	private VocabResource vocabJsonObject;
	// private int[] randomOrder;
	private int currentIndex;
	private int languageIndex;


	void Start () {
		// initialize variables from the GameState for shorter referencing.
		activeRound = GameState.Instance.ActiveRound;
		activeStory = GameState.Instance.ActiveStory;
		activeLanguage = GameState.Instance.ActiveLanguage;
		activeDifficulty = GameState.Instance.ActiveDifficulty;

		// Initialize the index and the children element references
		currentIndex = 0;
		englishWord = transform.Find("Word English").GetComponent<WordController>();
		foreignWord = transform.Find("Word Foreign").GetComponent<WordController>();
		englishDefinition = transform.Find("Definition English").GetComponent<WordController>();
		foreignDefinition = transform.Find("Definition Foreign").GetComponent<WordController>();
		image = transform.Find("Vocab Image").GetComponent<SpriteRenderer>();
		foreignHelpIcon = transform.Find("Help Foreign").GetComponent<HelpController>();
		uiController = GameObject.Find("Canvas").GetComponent<VocabUIController>();

		SetupLanguageHelp();

		// Load and randomize the vocab, then start at the first.
		LoadJson();
		languageIndex = Array.FindIndex(vocabJsonObject.languages, language => language.Equals(activeLanguage));
		Proceed(0);
	}

	/// <summary>
	/// Loads a JSON file and parses it into a VocabResource object
	/// </summary>
	public void LoadJson()
	{
		vocabJsonFile = Resources.Load<TextAsset>("vocab_" + activeStory + "_" + activeDifficulty).text;
		vocabJsonObject = JsonUtility.FromJson<VocabResource>(vocabJsonFile);
		vocabJsonObject.GenerateRandomOrder();
	}

	/// <summary>
	/// Sets the settings for the help based on activeRound.
	/// </summary>
	public void SetupLanguageHelp()
	{
		speech = -1;

		if (activeRound == 1)
		{
			foreignHelp = 1;
			english = -1;
		}
		else
		{
			english = 1;

			if (activeRound == 2)
			{
				foreignHelp = 1;
			}
			else if (activeRound == 3)
			{
				foreignHelp = 0;
			}
			else
			{
				foreignHelp = -1;
				speech = 1;
			}
		}
	}

	/// <summary>
	/// Updates the UI with the information from the previous (i=-1), current (i=0), or next(i=1) vocab word
	/// </summary>
	/// <returns>Whether it can go farther in the direction i.</returns>
	public bool Proceed (int i)
	{
		SetupLanguageHelp();
		englishWord.gameObject.SetActive((english == 1));
		englishDefinition.gameObject.SetActive((english == 1));
		foreignWord.gameObject.SetActive((foreignHelp == 1));
		foreignDefinition.gameObject.SetActive((foreignHelp == 1));
		foreignHelpIcon.gameObject.SetActive((foreignHelp == 0));
		GameState.Instance.SpeechEnabled = (speech == 1);
		UnityEngine.Object.FindObjectOfType<VoiceControlManager>().Enabled = (speech == 1);

		if (currentIndex + i > -1 && currentIndex + i < vocabJsonObject.Length)
		{
			// advance the vocab index
			currentIndex += i;

			// Update the UI
			image.sprite = Resources.Load<Sprite>(vocabJsonObject.GetImagePath(currentIndex));

			englishWord.UpdateWord(vocabJsonObject.GetWord(0, currentIndex), vocabJsonObject.GetWordAudioPath(0, currentIndex));
			englishDefinition.UpdateWord(vocabJsonObject.GetDefinition(0, currentIndex), vocabJsonObject.GetDefinitionAudioPath(0, currentIndex));

			foreignWord.UpdateWord(vocabJsonObject.GetWord(languageIndex, currentIndex), vocabJsonObject.GetWordAudioPath(languageIndex, currentIndex));
			foreignDefinition.UpdateWord(vocabJsonObject.GetDefinition(languageIndex, currentIndex), vocabJsonObject.GetDefinitionAudioPath(languageIndex, currentIndex));

			return (currentIndex + i > -1 && currentIndex + i < vocabJsonObject.Length);
		}
		else
		{
			return false;
		}
	}

	public bool IsFirst()
	{
		return currentIndex == 0;
	}

	public bool IsLast()
	{
		return currentIndex + 1 == vocabJsonObject.Length;
	}

	///<summary>
	/// Shows the foreign language content if it is enabled (i.e. if not in round 4)
	///</summary>
	public void ShowLanguageHelp()
	{
		if (foreignHelp == 0)
		{
			foreignWord.gameObject.SetActive(true);
			foreignWord.UpdateSize();
			foreignDefinition.gameObject.SetActive(true);
			foreignDefinition.UpdateSize();
			foreignHelpIcon.gameObject.SetActive(false);
		}
	}


	///<summary>
	/// Plays the sound of the foreign word (round 1) or the sound of the English word.
	///</summary>
	public void PlayPrimarySound()
	{
		if (englishWord.gameObject.activeSelf)
		{
			englishWord.PlaySound();
		}
		else
		{
			foreignWord.PlaySound();
		}
	}

	public void PlaySound(string audioPath)
	{
		AudioPlaybackManager.PlaySound(audioPath);
		if (GameState.Instance.ActiveRound != 4)
		{
			uiController.EnableForward();
		}
	}

	public override void OnSpeechResults(string[] results)
	{
		// TODO
		foreach (string s in results)
		{
			if (s.Contains(englishWord.word))
			{
				uiController.EnableForward();
			}

		}
	}

}
