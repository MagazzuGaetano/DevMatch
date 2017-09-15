using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BestScore : MonoBehaviour
{

    public Text _bestScore;

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/Scores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Scores.dat", FileMode.Open);
            List<int> scores = (List<int>)bf.Deserialize(file);

            if (GameStatus.scores == null)
            {
                GameStatus.scores = new List<int>();
                GameStatus.scores.AddRange(scores);
            }
            else
            {
                if (GameStatus.newScore != -1) GameStatus.scores.Add(GameStatus.newScore);
            }
            
            _bestScore.text = GameStatus.scores.Max().ToString();
        }
    }
}
