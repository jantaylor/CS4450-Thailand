using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VocabController : MonoBehaviour {

	// TODO: currentRound will come from the game state
	public int currentRound;
	public string[] englishWords;
	public string[] thaiWords;
	public AudioClip[] englishAudioClips;
	public AudioClip[] thaiAudioClips;

	private WordController englishWord;
	private WordController thaiWord;
	private WordController englishDefinition;
	private WordController thaiDefinition;
	private GameObject image;
	private int currentIndex;
	private string vocabJsonFile;
	private VocabResource vocabJsonObject;
	private int[] randomOrder;

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
		// Initialize at the index
		currentIndex = 0;
		englishWord = GetComponentsInChildren<WordController>()[0];
		thaiWord = GetComponentsInChildren<WordController>()[1];
		englishDefinition = GetComponentsInChildren<WordController>()[2];
		thaiDefinition = GetComponentsInChildren<WordController>()[3];
		LoadJson();
		GenerateRandomOrder();
		Proceed(0);
	}

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

	// Update is called once per frame
	void Update () {

	}

	public void LoadJson()
	{
		vocabJsonFile = Resources.Load<TextAsset>("Round" + currentRound + "Vocab").text;
		vocabJsonObject = JsonUtility.FromJson<VocabResource>(vocabJsonFile);
	}

	public bool Proceed (int i)
	{
		Debug.Log("Proceed " + i + ": " + (currentIndex + i));
		if (currentIndex + i > -1 && currentIndex + i < randomOrder.Length)
		{
			currentIndex += i;
			Debug.Log("Index: " + currentIndex + "(" + randomOrder[currentIndex] + ") + " + i);
			var audioPaths = vocabJsonObject.GetAudioPaths(randomOrder[currentIndex]);
			var definitionPaths = vocabJsonObject.GetDefinitionPaths(randomOrder[currentIndex]);
			var imagePaths = vocabJsonObject.GetImagePaths(randomOrder[currentIndex]);

			englishWord.UpdateWord(vocabJsonObject.vocab[0].words[randomOrder[currentIndex]], audioPaths[0], imagePaths[0]);
			thaiWord.UpdateWord(vocabJsonObject.vocab[1].words[randomOrder[currentIndex]], audioPaths[1], imagePaths[1]);
			englishDefinition.UpdateWord(vocabJsonObject.vocab[0].definitions[randomOrder[currentIndex]], definitionPaths[0], imagePaths[0]);
			thaiDefinition.UpdateWord(vocabJsonObject.vocab[1].definitions[randomOrder[currentIndex]], definitionPaths[1], imagePaths[1]);

			return true;
		}
		return false;
	}
}
