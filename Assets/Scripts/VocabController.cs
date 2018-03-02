using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabController : MonoBehaviour {

	public int currentRound;
	public string[] englishWords;
	public string[] thaiWords;
	public AudioClip[] englishAudioClips;
	public AudioClip[] thaiAudioClips;

	private WordController englishWord;
	private WordController thaiWord;
	private GameObject image;
	private int currentIndex;
	private string vocabJsonFile;
	private VocabResource vocabJsonObject;

	[System.Serializable]
	public class VocabResource
	{
		public string[] languages;
		public string audioRoot;
		public string audioFile;
		public string imageFile;
		public VocabList[] vocab;

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

		public override string ToString()
		{
			return "audioRoot: " + audioRoot + "; audioFile: " + audioFile + "; vocab: " + vocab.ToString();
		}
	};

	[System.Serializable]
	public class VocabList
	{
		public string language;
		public string[] words;

		public override string ToString()
		{
			return words.ToString();
		}
	};


	// Use this for initialization
	void Start () {
		currentIndex = 0;
		englishWord = GetComponentsInChildren<WordController>()[0];
		thaiWord = GetComponentsInChildren<WordController>()[1];
		LoadJson();
		Proceed(0);
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
		if (currentIndex + i > -1 && currentIndex + i < vocabJsonObject.vocab[0].words.Length)
		{
			currentIndex += i;
			Debug.Log("Index: " + currentIndex + " + " + i);
			var audioPaths = vocabJsonObject.GetAudioPaths(currentIndex);
			var imagePaths = vocabJsonObject.GetImagePaths(currentIndex);
			englishWord.UpdateWord(vocabJsonObject.vocab[0].words[currentIndex], audioPaths[0], imagePaths[0]);
			thaiWord.UpdateWord(vocabJsonObject.vocab[1].words[currentIndex], audioPaths[1], imagePaths[1]);

			return true;
		}
		return false;
	}
}
