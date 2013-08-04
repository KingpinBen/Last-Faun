using UnityEngine;
using System.Collections;

public class MainMenuGui : MonoBehaviour
{
    public GUISkin skin;
    public Vector2 menuOffset = Vector2.zero;

    private Texture2D _pixel;
    private bool _transitioning;
    private int _levelToLoad = -1;
    private Matrix4x4 _guiMatrix;
    private Rect _guiRect = new Rect(0,0,200, 200);
    public Color _tintColor = Color.black * 0f;

    void Start()
    {
        SaveManager.Load();

        _pixel = new Texture2D(1, 1);
        _pixel.SetPixels(new[] { Color.red});
        _pixel.Apply();
    }

    void Update()
    {
        UpdateGui();
    }

    void OnGUI()
    {
        if (_transitioning)
        {
            _tintColor.a = Mathf.Lerp(_tintColor.a, 1, Time.deltaTime);

            GUI.color = _tintColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _pixel);
        }
        else
        {
            if (skin)
                GUI.skin = skin;

            GUI.matrix = _guiMatrix;

            GUILayout.BeginArea(_guiRect);

            if (SaveManager.loadFound)
            {
                if (GUILayout.Button("Continue"))
                {
                    LoadLevel(SaveManager.currentLevel);
                }
            }

            if (GUILayout.Button("New Game"))
            {
                //  Check if we should override
                LoadLevel(1);
            }

            if (GUILayout.Button("Exit Game"))
            {
                Application.Quit();
            }

            GUILayout.EndArea();
        }
    }

    void LoadLevel(int levelIndex)
    {
        _transitioning = true;
        _levelToLoad = levelIndex;

        StartCoroutine(TransitionOutToLevel());
    }

    void UpdateGui()
    {
        var scale = 
            ((Screen.height < Screen.width) ? Screen.height : Screen.width) * 0.001f;

        _guiMatrix = Matrix4x4.TRS(
            new Vector3(Screen.width - (_guiRect.xMax + menuOffset.x) * scale,
                (menuOffset.y) * scale, 0), 
                Quaternion.identity, new Vector3(scale, scale, 1));
    }

    IEnumerator TransitionOutToLevel()
    {
        yield return new WaitForSeconds(2);

        Application.LoadLevel(_levelToLoad);
    }
}
