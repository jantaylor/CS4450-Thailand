using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class VocabController : MonoBehaviour {

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

	private string vocabJsonFile;
	private VocabResource vocabJsonObject;
	private int[] randomOrder;
	private int currentIndex;
	private int languageIndex;

	[System.Serializable]
	public class VocabResource
	{
		public string[] languages;
		public string audioRoot;
		public string audioFile;
		public string definitionAudioFile;
		public string imageFile;
		public VocabList[] vocab;

		/// <summary>
		/// Returns the name of the audio resource, replacing the placeholders in the template string
		/// </summary>
		/// <param name="index">The array index to retrieve info from.</param>
		/// <returns>Name of the audio resource</returns>
		public List<string> GetAudioPaths(int index)
		{
			List<string> paths = new List<string>();
			string word = null;
			foreach (var v in vocab)
			{
				word = (word == null)? v.words[index] : word;
				string path = audioFile.Replace("{LANG}", v.language).Replace("{WORD}", word);
				paths.Add(path);
			}
			return paths;
		}

		/// <summary>
		/// Returns the name of the audio resource, replacing the placeholders in the template string
		/// </summary>
		/// <param name="index">The array index to retrieve info from.</param>
		/// <returns>Name of the audio resource</returns>
		public List<string> GetDefinitionPaths(int index)
		{
			List<string> paths = new List<string>();
			string word = null;
			foreach (var v in vocab)
			{
				word = (word == null)? v.words[index] : word;
				string path = definitionAudioFile.Replace("{LANG}", v.language).Replace("{WORD}", word);
				paths.Add(path);
			}
			return paths;
		}

		/// <summary>
		/// Returns the name of the image resource, replacing the placeholders in the template string
		/// </summary>
		/// <param name="index">The array index to retrieve info from.</param>
		/// <returns>Name of the image resource</returns>
		public List<string> GetImagePaths(int index)
		{
			List<string> paths = new List<string>();
			string word = null;
			foreach (var v in vocab)
			{
				word = (word == null)? v.words[index] : word;
				string path = imageFile.Replace("{WORD}", word);
				paths.Add(path);
			}
			return paths;
		}

		/// <summary>
		/// For debugging
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "audioRoot: " + audioRoot + "; audioFile: " + audioFile + "; vocab: " + vocab.ToString();
		}
	};

	/// <summary>
	/// A class to contain an array of words of a specified language
	/// </summary>
	[System.Serializable]
	public class VocabList
	{
		public string language;
		public string[] words;
		public string[] definitions;

		/// <summary>
		/// For debugging
		/// <returns></returns>
		/// </summary>
		public override string ToString()
		{
			return words.ToString();
		}
	};


	void Start () {
		// initialize variables from the GameState for shorter referencing.
		activeRound = GameState.Instance.ActiveRound;
		activeStory = GameState.Instance.ActiveStory;
		activeLanguage = GameState.Instance.ActiveLanguage;
		activeDifficulty = GameState.Instance.ActiveDifficulty;

		// Initialize the index and the children element references
		currentIndex = 0;
		englishWord = GetComponentsInChildren<WordController>()[0];
		foreignWord = GetComponentsInChildren<WordController>()[1];
		englishDefinition = GetComponentsInChildren<WordController>()[2];
		foreignDefinition = GetComponentsInChildren<WordController>()[3];
		image = GameObject.Find("Vocab Image").GetComponent<SpriteRenderer>();

		SetupLanguageHelp();

		// Load and randomize the vocab, then start at the first.
		LoadJson();
		languageIndex = Array.FindIndex(vocabJsonObject.languages, language => language.Equals(activeLanguage));
		GenerateRandomOrder();
		Proceed(0);
	}

	/// <summary>
	/// Creates an array of the index numbers of the vocab list in a random order
	/// </summary>
	/// <returns></returns>
	void GenerateRandomOrder()
	{
		int length = vocabJsonObject.vocab[0].words.Length;
		var random = new System.Random();
		randomOrder = new int[length];

		// Fisher-Yates "inside-out" algorithm
		for (int i = 0; i < length; i++)
		{
			int j = random.Next(0, i + 1);
			if (i != j)
			{
				randomOrder[i] = randomOrder[j];
			}
			randomOrder[j] = i;
		}
	}

	/// <summary>
	/// Loads a JSON file and parses it into a VocabResource object
	/// </summary>
	public void LoadJson()
	{
		vocabJsonFile = Resources.Load<TextAsset>("vocab_" + activeStory + "_" + activeDifficulty).text;
		vocabJsonObject = JsonUtility.FromJson<VocabResource>(vocabJsonFile);
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
	public void Proceed (int i)
	{
		SetupLanguageHelp();
		englishWord.gameObject.SetActive((english == 1));
		englishDefinition.gameObject.SetActive((english == 1));
		foreignWord.gameObject.SetActive((foreignHelp == 1));
		foreignDefinition.gameObject.SetActive((foreignHelp == 1));

		if (currentIndex + i > -1 && currentIndex + i < randomOrder.Length)
		{
			// advance the vocab index
			currentIndex += i;

			// Get the paths to the resources
			var audioPaths = vocabJsonObject.GetAudioPaths(randomOrder[currentIndex]);
			var definitionPaths = vocabJsonObject.GetDefinitionPaths(randomOrder[currentIndex]);
			var imagePaths = vocabJsonObject.GetImagePaths(randomOrder[currentIndex]);

			// Update the UI
			image.sprite = Resources.Load<Sprite>(imagePaths[0]);
			englishWord.UpdateWord(vocabJsonObject.vocab[0].words[randomOrder[currentIndex]], audioPaths[0]);
			englishDefinition.UpdateWord(vocabJsonObject.vocab[0].definitions[randomOrder[currentIndex]], definitionPaths[0]);

			foreignWord.UpdateWord(vocabJsonObject.vocab[languageIndex].words[randomOrder[currentIndex]], audioPaths[languageIndex]);
			foreignDefinition.UpdateWord(vocabJsonObject.vocab[languageIndex].definitions[randomOrder[currentIndex]], definitionPaths[languageIndex]);

		}
		else
		{
			GameState.Instance.LoadMenu();
		}
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

}
