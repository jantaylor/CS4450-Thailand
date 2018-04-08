using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class SpeechHandler : MonoBehaviour {

  public abstract void OnSpeechResults(string[] results);
}
