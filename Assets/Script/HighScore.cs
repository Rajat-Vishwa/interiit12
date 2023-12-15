using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScore : MonoBehaviour {

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    public static string pname="";
    public static int pscore;

    private void Awake() {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("HighScoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null || (highscores.highscoreEntryList != null && highscores.highscoreEntryList.Count == 0)) {
            // There's no stored table or the list is empty, initialize
            Debug.Log("Initializing table with default values...");
            highscores = new Highscores() {
                highscoreEntryList = new List<HighscoreEntry>()
            };
            AddHighscoreEntry(pscore, pname);
            // Reload
            jsonString = PlayerPrefs.GetString("HighScoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }



        // Sort entry list by Score
        highscores.highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

        highscoreEntryTransformList = new List<Transform>();
        int maxEntriesToShow = Mathf.Min(highscores.highscoreEntryList.Count, 4);  // Show at most 4 entries

        for (int i = 0; i < maxEntriesToShow; i++) {
            CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 40f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * (transformList.Count+1));
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
        default:
            rankString = rank + "TH"; break;

        case 1: rankString = "1ST"; break;
        case 2: rankString = "2ND"; break;
        case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<TMP_Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TMP_Text>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        
        // Highlight First
        if (rank == 1) {
            entryTransform.Find("posText").GetComponent<TMP_Text>().color = Color.red;
            entryTransform.Find("scoreText").GetComponent<TMP_Text>().color = Color.red;
            entryTransform.Find("nameText").GetComponent<TMP_Text>().color = Color.red;
        }

        transformList.Add(entryTransform);
    }

    public static void AddHighscoreEntry(int score, string name) {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };
        
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("HighScoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) {
            // There's no stored table, initialize
            highscores = new Highscores() {
                highscoreEntryList = new List<HighscoreEntry>()  // Initialize the list here
            };
        }


        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("HighScoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable] 
    private class HighscoreEntry {
        public int score;
        public string name;
    }

}