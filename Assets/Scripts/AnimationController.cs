using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour {

  [SerializeField]
  public List<Sprite> Sprites;

  private Animator animator;
  private Animation animation;
  private Image image;

  void Awake()
  {
    animator = GetComponent<Animator>();
    animation = GetComponent<Animation>();
    image = GetComponent<Image>();
  }

  public void SetSprite(int index)
  {
    image.sprite = Sprites[index];
  }

  public void Play()
  {
    StartCoroutine(PlayAnimation());
  }

  public IEnumerator PlayAnimation()
  {
    animator.SetTrigger("Answer");
    yield return new WaitForSeconds(animation.clip.length);
    gameObject.SetActive(false);
  }

  // How to use:
  // private GameObject resultAnimation;
  // Start():
  // resultAnimation = GameObject.Find("Result Animation");

  // Proceed():
  // resultAnimation.SetActive(false);

  // User Action:
  // resultAnimation.SetActive(true);
  // resultAnimation.GetComponent<AnimationController>().SetSprite(correct? 0 : 1);
  // resultAnimation.GetComponent<AnimationController>().Play();


}
