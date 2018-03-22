using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocabGameOption : MonoBehaviour {

	private Text text;
	private Image image;
	private VocabGameController gameController;
	private bool touchEnabled;
	private bool speechEnabled;


	public string EnglishWord {get; set;}
	public string ForeignWord {get; set;}
	public string ImagePath {get; set;}
	public string EnglishAudioPath {get; set;}
	public string ForeignAudioPath {get; set;}

	void Awake()
	{
		gameController = GameObject.Find("Vocab Container").GetComponent<VocabGameController>();
		text = gameObject.GetComponentInChildren<Text>();
		image = gameObject.GetComponentInChildren<Image>();
	}

	/// <summary>
	///	Determines what type of options are available: 1: image; 2: Foreign word; 3: English word; 4: English word (speech activated)
	/// </summary>
	public void Display(int displayType)
	{
		text.text = "";
		image.sprite = null;

		switch (displayType)
		{
			case 1:
				image.sprite = Resources.Load<Sprite>(ImagePath);
				image.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
				break;
			case 2:
				text.text = ForeignWord;
				image.sprite = Resources.Load<Sprite>("_transparent");
				break;
			case 3:
				text.text = EnglishWord;
				image.sprite = Resources.Load<Sprite>("_transparent");
				break;
			case 4:
				text.text = EnglishWord;
				image.sprite = Resources.Load<Sprite>("_transparent");
				touchEnabled = false;
				speechEnabled = true;
				break;
		}
	}

	public void CheckMatch()
	{
		gameController.CheckMatch(EnglishWord);
	}
}
