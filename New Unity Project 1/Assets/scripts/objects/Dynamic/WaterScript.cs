using UnityEngine;
using System.Collections;

public class WaterScript : MonoBehaviour
{
    public Vector2 movementDirection = Vector2.zero;
    public Vector2 tileScale = Vector2.one;
    public Color color = new Color(0.05f, 0.01f, 0.7f);
    public Color highightColor = Color.white;

    private Material _material;

	void Start ()
	{
	    _material = renderer.material;
	    _material.mainTextureScale = tileScale;
	    _material.color = color;
        _material.SetColor("_Highlight", highightColor);
	}
	
	void Update ()
	{
        _material.mainTextureOffset += movementDirection * 0.001f;
	}
}
