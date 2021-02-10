using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System;
using System.Linq;

public class crossHair : MonoBehaviour
{
    [SerializeField] private string[] keywords;
    [SerializeField] private float speed = 4.0f;
    
    private KeywordRecognizer kr; // up, down, left, right
    private DictationRecognizer dr; // Longer sentances
    private GrammarRecognizer gr; // Recognizes and return text
    private Rigidbody2D rb;

    // Action in system
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    // Confidence level
    private ConfidenceLevel confidenceLevel = ConfidenceLevel.Low;

    private string spokenWord = "";

    // Start is called before the first frame update
    void Start()
    {
        // String, method
        actions.Add("Up", Up);
        actions.Add("Down", Down);
        actions.Add("Left", Left);
        actions.Add("Right", Right);

        rb = GetComponent<Rigidbody2D>();
        if(keywords != null)
        {
            // kr = new KeywordRecognizer(keywords, confidenceLevel);
            kr = new KeywordRecognizer(actions.Keys.ToArray(), confidenceLevel);
            kr.OnPhraseRecognized += KR_OnPhraseRecognized;
            kr.Start();
        }
    }

    // Update is called once per frame
    private void KR_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        spokenWord = args.text;
        Debug.Log("You said: " + spokenWord);
        actions[spokenWord].Invoke();
    }

    // Action methods
    // Void return type and no param
    private void Up()
    {
        transform.Translate(0, 1, 0);
    }
    private void Down()
    {
        transform.Translate(0, -1, 0);
    }
    private void Left()
    {
        transform.Translate(-1, 0, 0);
    }
    private void Right()
    {
        transform.Translate(1, 0, 0);
    }

    /*void Update()
    {
        // How to apply that command from spoken word
        float hMovement = 0, vMovement = 0;

        switch (spokenWord)
        {
            case "Up":
                vMovement = 1;
                break;
            case "Down":
                vMovement = -1;
                break;
            case "Left":
                hMovement = -1;
                break;
            case "Right":
                hMovement = 1;
                break;
            case "Stop":
                hMovement = vMovement = 0;
                break;
            default:
                break;
        }

        rb.velocity = new Vector2(hMovement * speed, vMovement * speed);
    }*/

    // failsafe stop mechanism
    private void OnApplicationQuit()
    {
        if(kr != null && kr.IsRunning)
        {
            kr.OnPhraseRecognized -= KR_OnPhraseRecognized;
            kr.Stop();
        }
    }
}
