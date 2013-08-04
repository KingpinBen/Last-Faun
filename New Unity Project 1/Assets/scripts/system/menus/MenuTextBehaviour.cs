using UnityEngine;

public class MenuTextBehaviour : MonoBehaviour
{

    public int openScene;
    public bool isQuit;

    private bool _mouseSelected;

    private void Start()
    {
        renderer.material.color = Color.grey;
    }

    public void SetSelected(bool isSelected)
    {

    }

    private void OnSelect()
    {
        if (!isQuit)
            Application.LoadLevel(openScene);
        else
            Application.Quit();
    }

    private void OnMouseUp()
    {
        if (_mouseSelected)
            OnSelect();
    }

    private void OnMouseEnter()
    {
        _mouseSelected = true;
        renderer.material.color = Color.white;
    }

    private void OnMouseExit()
    {
        _mouseSelected = false;
        renderer.material.color = Color.grey;
    }
}
