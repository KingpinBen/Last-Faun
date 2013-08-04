using UnityEngine;
using System.Collections;

public class InGamePause : MonoBehaviour
{
    public static InGamePause instance;

    public GUISkin guiSkin;

    private Matrix4x4 _guiMatrix;
    private Rect _guiRect = new Rect(0, 0, 250, 200);
    private bool _paused;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateGui();
    }

    private void Update()
    {
        UpdateGui();
        if (Input.GetKeyDown(KeyCode.Escape))
            UpdateGameState();
    }

    private void UpdateGameState()
    {
        _paused = !_paused;

        Time.timeScale = _paused ? 0 : 1;
        Screen.showCursor = _paused;
    }

    private void UpdateGui()
    {
        var scale = Screen.height*0.001f;
        var offset = new Vector3((Screen.width*.5f) - ((_guiRect.xMax*.5f)*scale),
                                 (Screen.height*.5f) - ((_guiRect.yMax*.5f)*scale), 0);
        _guiMatrix =
            Matrix4x4.TRS(offset, Quaternion.identity, new Vector3(scale, scale, 1));
    }

    private void OnGUI()
    {
        if (!_paused) return;
        if (guiSkin) GUI.skin = guiSkin;
        GUI.matrix = _guiMatrix;

        GUILayout.BeginArea(_guiRect, guiSkin.customStyles[0]);
        GUILayout.Label("Paused");
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Resume"))
            UpdateGameState();

        if (GUILayout.Button("Back to Menu"))
        {
            UpdateGameState();
            Application.LoadLevel(0);
        }

        GUILayout.EndArea();
    }

    public bool GetIsPaused()
    {
        return _paused;
    }
}
