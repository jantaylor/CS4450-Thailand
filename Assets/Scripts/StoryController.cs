using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : MonoBehaviour {

	public int currentRound;
	public string[] englishSentences;
	public string[] thaiSentences;
	public AudioClip[] englishAudioClips;
	public AudioClip[] thaiAudioClips;

	private SentenceController englishSentence;
	private SentenceController thaiSentence;
	private GameObject image;
	private int currentIndex;
	private string sentenceJsonFile;
	private SentenceResource sentenceJsonObject;

	[System.Serializable]
	public class SentenceResource
	{
		public string[] languages;
		public string audioRoot;
		public string audioFile;
		public string imageFile;
		public StoryList[] story;

		public List<string> GetAudioPaths(int index)
		{
			List<string> paths = new List<string>();
			string sentence = null;
			foreach (var s in story)
			{
                sentence = (sentence == null) ? s.sentences[index] : sentence;
				string path = audioFile.Replace("{LANG}", s.language).Replace("{SENTENCE}", sentence);
				paths.Add(path);
			}
			return paths;
		}

		public List<string> GetImagePaths(int index)
		{
			List<string> paths = new List<string>();
			string sentence = null;
			foreach (var s in story)
			{
                sentence = (sentence == null) ? s.sentences[index] : sentence;
				string path = imageFile.Replace("{WORD}", sentence);
				paths.Add(path);
			}
			return paths;
		}

		public override string ToString()
		{
			return "audioRoot: " + audioRoot + "; audioFile: " + audioFile + "; sentence: " + story.ToString();
		}
	};

	[System.Serializable]
	public class StoryList
	{
		public string language;
		public string[] sentences;

		public override string ToString()
		{
			return sentences.ToString();
		}
	};


	// Use this for initialization
	void Start () {
		currentIndex = 0;
        englishSentence = GetComponentsInChildren<SentenceController>()[0];
        thaiSentence = GetComponentsInChildren<SentenceController>()[1];
		LoadJson();
		Proceed(0);
	}

	// Update is called once per frame
	void Update () {

	}

	public void LoadJson()
	{
        sentenceJsonFile = Resources.Load<TextAsset>("Round" + currentRound + "Sentence").text;
        sentenceJsonObject = JsonUtility.FromJson<SentenceResource>(sentenceJsonFile);
	}

	public bool Proceed (int i)
	{
		Debug.Log("Proceed " + i + ": " + (currentIndex + i));
		if (currentIndex + i > -1 && currentIndex + i < sentenceJsonObject.story[0].sentences.Length)
		{
			currentIndex += i;
			Debug.Log("Index: " + currentIndex + " + " + i);
			var audioPaths = sentenceJsonObject.GetAudioPaths(currentIndex);
			var imagePaths = sentenceJsonObject.GetImagePaths(currentIndex);
            englishSentence.UpdateSentence(sentenceJsonObject.story[0].sentences[currentIndex], audioPaths[0], imagePaths[0]);
            thaiSentence.UpdateSentence(sentenceJsonObject.story[1].sentences[currentIndex], audioPaths[1], imagePaths[1]);

			return true;
		}
		return false;
	}
}
