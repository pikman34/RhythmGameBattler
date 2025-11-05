using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;

    public bool startPlaying;

    public GameObject notesHolder1, notesHolder2, notesHolder3;
    public GameObject notesInstance;
    public BeatScroller theBS;
    public CharacterBattle activeCharacterBattle;
    public static GameManager instance;

    public int currentscore;
    public int scorePerNote = 1;
    public int scorePerGoodNote = 3;
    public int scorePerPerfectNote = 5;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public Text scoreText;
    public Text multiText;
    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missHits;

    void Awake()
    {
        totalNotes = 0;
    }

    void Start()
    {
        instance = this;

        scoreText.text = "Attack Power: 0";
        currentMultiplier = 1;
        
    }

    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown && totalNotes > 0)
            {
                startPlaying = true;
                theBS.hasStarted = true;
            }
        }
    }

    public void NoteHit()
    {
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        multiText.text = "Multiplier: x" + currentMultiplier;

        if (activeCharacterBattle != null)
        {
            activeCharacterBattle.alivenotes--;
        }

        //currentscore += scorePerNote * currentMultiplier;
        scoreText.text = "Attack Power: " + currentscore;
    }

    public void NormalHit()
    {
        currentscore += scorePerNote * currentMultiplier;
        NoteHit();

        normalHits++;
    }

    public void GoodHit()
    {
        currentscore += scorePerGoodNote * currentMultiplier;
        NoteHit();

        goodHits++;
    }

    public void PerfectHit()
    {
        currentscore += scorePerPerfectNote * currentMultiplier;
        NoteHit();

        perfectHits++;
    }

    public void NoteMissed()
    {

        if (activeCharacterBattle != null)
        {
            activeCharacterBattle.alivenotes--;
        }

        currentMultiplier = 1;
        multiplierTracker = 0;

        multiText.text = "Multiplier: x" + currentMultiplier;

        missHits++;
    }

    public void NewNotes()
    {
        int randomchart = 0;
        randomchart = UnityEngine.Random.Range(1, 4);

        switch (randomchart)
        {
            case 1:
                notesInstance = Instantiate(notesHolder1);
                theBS = notesInstance.GetComponent<BeatScroller>();
                totalNotes = FindObjectsByType<NoteObject>(FindObjectsSortMode.None).Length;
                break;

            case 2:
                notesInstance = Instantiate(notesHolder2);
                theBS = notesInstance.GetComponent<BeatScroller>();
                totalNotes = FindObjectsByType<NoteObject>(FindObjectsSortMode.None).Length;
                break;
            case 3:
                notesInstance = Instantiate(notesHolder3);
                theBS = notesInstance.GetComponent<BeatScroller>();
                totalNotes = FindObjectsByType<NoteObject>(FindObjectsSortMode.None).Length;
                break;
        }
    }
}
