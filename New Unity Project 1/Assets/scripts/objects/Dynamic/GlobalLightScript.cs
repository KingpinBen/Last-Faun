using UnityEngine;
using System.Collections;

public class GlobalLightScript : MonoBehaviour
{

    public Material skyBoxMaterial;
    public Color skyBoxColorTint = Color.cyan;

    private Material _skyMaterial;

	void Start ()
	{
        if (skyBoxMaterial)
        {
            _skyMaterial = new Material(skyBoxMaterial);
            _skyMaterial.SetColor("_Tint", skyBoxColorTint);

            RenderSettings.skybox = _skyMaterial;
        }
	}
}
