using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaginationIcon : MonoBehaviour
{
    private Image _image;

    public float inactiveScale;
    public float activeScale;

    public Color inactiveColor;
    public Color activeColor;

    private void Awake()
    {
        _image = gameObject.GetComponent<Image>();
    }

    public void Toggle(bool active)
    {
        if (active)
        {
            transform.localScale = new Vector3(activeScale, activeScale, activeScale);
            _image.color = activeColor;
        }
        else
        {
            transform.localScale = new Vector3(inactiveScale, inactiveScale, inactiveScale);
            _image.color = inactiveColor;
        }
    }
}
