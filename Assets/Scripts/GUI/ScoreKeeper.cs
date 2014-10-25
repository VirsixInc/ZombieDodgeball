using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

    /// <summary>
    /// attached to manager script
    /// </summary>

    int score = 0;
    // art
    public GUIStyle scoreGUIStyle;

    void OnGUI()
    {
        // top left of screen
        GUILayout.BeginArea(new Rect(32, 32, 200, 200));
        GUILayout.Label("Score: " + score, scoreGUIStyle);
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width - 256, Screen.height - 128, 256, 256));
        GUILayout.EndArea();
    }

    public void AddGameStats(int s)
    {
        score += s;
    }
}
