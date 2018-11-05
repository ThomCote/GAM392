using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour {

	public GameObject tutorial1;
	public GameObject tutorial2;

	public Text promptText;

	int numInputs = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Space"))
		{
			// Progress the title screen / tutorial
			if (numInputs == 0)
			{
				// Show tutorial 1
				tutorial1.SetActive(true);
				promptText.text = "Click sticks to continue.";
				++numInputs;
			}
			else if (numInputs == 1)
			{
				// Show tutorial 2
				tutorial1.SetActive(false);
				tutorial2.SetActive(true);
				promptText.text = "Click sticks to play!";
				++numInputs;
			}
			else if (numInputs == 2)
			{
				// Start game
				promptText.text = "Loading...";
				SceneManager.LoadScene("MainLevel");
			}
		}
	}
}
