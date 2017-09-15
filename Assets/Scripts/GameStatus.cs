using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class GameStatus : MonoBehaviour
{

    public static List<int> scores;
    public static int Score = 0, newScore = -1;
    public Text _score;

    public Text _time;
    public float timer;
    public float MaxTime;
    public float speed = 1f;

    public GameObject screen;
    public AudioSource clip;

    public static bool CanPlay = true;
    private bool sound;

    private void Start()
    {
        Score = 0;
        timer = 200.000f;
        MaxTime = timer;
        sound = true;
    }

    private void Update()
    {
        _score.text = "SCORE: " + Score;
        var _timer = timer;
        timer -= Time.deltaTime * speed;

        if (timer <= 50.000f) _time.color = Color.red;

        if (timer <= 10.000f) speed = 0.8f;

        if (timer <= 0) timer = 0;

        screen.gameObject.SetActive((timer <= 0) ? true : false);

        if (timer <= 0 && screen.gameObject.activeSelf)
        {
            if (sound)
            {
                clip.Play();
                sound = false;

                newScore = Score;
                List<int> s = new List<int>();
                s.Add(newScore);
                Save(s);
            }

            if (Input.anyKey)
            {
                SceneManager.LoadScene(0);
            }
        }

        _time.text = "TIME: " + timer.ToString("00:00");
    }

    IEnumerator playsound(AudioSource clip)
    {
        clip.Play();
        yield return new WaitForSeconds(0.01f);
    }

    void Save(List<int> s)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Scores.dat", FileMode.OpenOrCreate);
        bf.Serialize(file, s);
        file.Close();
    }
}
