using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenù : MonoBehaviour {

    public Button Back;
    public AudioSource clip;

    // Use this for initialization
    void Start () {
        Back.onClick.AddListener(() =>
        {
            StartCoroutine(ChangeScene());
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator ChangeScene()
    {
        clip.Play();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(0);
    }
}
