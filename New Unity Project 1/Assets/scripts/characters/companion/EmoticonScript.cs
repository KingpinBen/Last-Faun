using UnityEngine;
using System.Collections;

public class EmoticonScript : MonoBehaviour
{
    public float fadeOutTime = 2.0f;
    public float showingTime = 4.5f;

    private Camera _camera;
    private Material _material;
    private Quaternion _forwardUp;
    private Color _color = Color.white;
    private bool _isFading;

    private void Awake()
    {
        _material = renderer.material;
        _forwardUp = Quaternion.FromToRotation(Vector3.up, Vector3.forward);
    }

    private void Start()
    {
        _camera = Camera.main;
        _material.color = _color * 0.0f;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(-_camera.transform.forward, Vector3.up)*_forwardUp;

        if (!_isFading) return;

        if (_color.a > 0.0f)
        {
            _color.a -= Time.deltaTime / fadeOutTime;
            _material.color = _color;
        }
    }

    public void SetTexture(int tileX)
    {
        _material.mainTextureOffset = new Vector2(.25f * tileX, 0);
        _material.color = Color.white;

        StartCoroutine(StartFadeOut());
    }

    private IEnumerator StartFadeOut()
    {
        yield return new WaitForSeconds(showingTime);
        _isFading = true;
    }
}
