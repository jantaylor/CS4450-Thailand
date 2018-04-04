using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocabGameOption : MonoBehaviour {

	private Text text;
	private Image image;
	private VocabGameController gameController;
	private AudioClip audioClip;
	private AudioSource audioPlayer;
	private bool touchEnabled;
	private bool speechEnabled;


	public string EnglishWord {get; set;}
	public string ForeignWord {get; set;}
	public string ImagePath {get; set;}
	public string EnglishAudioPath {get; set;}
	public string ForeignAudioPath {get; set;}

	public const int DISPLAY_WRONG = -1;
	public const int DISPLAY_HIDDEN = 0;
	public const int DISPLAY_IMAGE = 1;
	public const int DISPLAY_FOREIGN = 2;
	public const int DISPLAY_ENGLISH = 3;
	public const int DISPLAY_ENGLISH_SPEECH = 4;


	void Awake()
	{
		gameController = GameObject.Find("Vocab Container").GetComponent<VocabGameController>();
		audioPlayer = GameObject.Find("Audio Player").GetComponent<AudioSource>();
		text = gameObject.GetComponentInChildren<Text>();
		image = gameObject.GetComponentInChildren<Image>();
	}

	/// <summary>
	///	Determines what type of options are available: -1: wrong answer (x) image; 0: hidden; 1: image; 2: Foreign word; 3: English word; 4: English word (speech activated)
	/// </summary>
	public void Display(int displayType)
	{
		text.text = "";
		image.sprite = null;

		switch (displayType)
		{
			case DISPLAY_WRONG:
				text.text = "";
				image.sprite = Resources.Load<Sprite>("_x");
				break;
			case DISPLAY_HIDDEN:
				text.text = "";
				image.sprite = Resources.Load<Sprite>("_transparent");
				image.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
				break;
			case DISPLAY_IMAGE:
				image.sprite = Resources.Load<Sprite>(ImagePath);
				image.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
				audioClip = Resources.Load<AudioClip>(EnglishAudioPath);
				break;
			case DISPLAY_FOREIGN:
				text.text = ForeignWord;
				image.sprite = Resources.Load<Sprite>("_transparent");
				audioClip = Resources.Load<AudioClip>(ForeignAudioPath);
				break;
			case DISPLAY_ENGLISH:
				text.text = EnglishWord;
				image.sprite = Resources.Load<Sprite>("_transparent");
				audioClip = Resources.Load<AudioClip>(EnglishAudioPath);
				break;
			case DISPLAY_ENGLISH_SPEECH:
				text.text = EnglishWord;
				image.sprite = Resources.Load<Sprite>("_transparent");
				touchEnabled = false;
				speechEnabled = true;
				audioClip = Resources.Load<AudioClip>(EnglishAudioPath);
				break;
		}
	}

	public void CheckMatch()
	{
		AudioPlaybackManager.PlaySound(EnglishAudioPath);
		gameController.CheckMatch(EnglishWord);
	}
}
