using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech; // Grammar Rec
using System.Text; // StringBuilder
using System.IO;

public class GrammarController : MonoBehaviour
{
    // Load grammar
    private GrammarRecognizer gr;

    // Start is called before the first frame update
    private void Start()
    {
        gr = new GrammarRecognizer(Path.Combine(Application.dataPath + "/StreamingAssets", "SimpleGrammar.xml"), ConfidenceLevel.Low);
        Debug.Log("Grammar loaded and recogniser started");
        gr.OnPhraseRecognized += GR_OnPhraseRecognized;
        gr.Start();
    }

    // Update is called once per frame
    private void GR_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder message = new StringBuilder();
        Debug.Log("Rec frame");

        // Read the semantic meanings from the args passed in
        SemanticMeaning[] meanings = args.semanticMeanings;
        
        // Move pawn from C2 to C4 - piece, start, finish
        foreach(SemanticMeaning meaning in meanings)
        {
            string keyString = meaning.key.Trim();
            string valueString = meaning.values[0].Trim();
            message.Append("Key " + keyString + ", Value: " + valueString + " ");
        }
        Debug.Log(message);
    }

    private void OnApplicationQuit()
    {
        if(gr != null && gr.IsRunning)
        {
            gr.OnPhraseRecognized -= GR_OnPhraseRecognized;
            gr.Stop();
        }
    }
}
