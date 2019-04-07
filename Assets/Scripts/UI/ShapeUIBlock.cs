using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ShapeUIBlock : MonoBehaviour
{
    [SerializeField]
    private Image _shapeImage;

    [SerializeField]
    private Image _borderImage;

    [SerializeField]
    private Text _shapeNameText;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private GameObject _loadingSpinner;

    public Action<string, Sprite> OnClicked;

    public void Initialize(ShapeScriptableObject data)
    {
        _shapeNameText.text = data.name;
        if(data.image_url != "")
        {
            StartCoroutine(LoadImage(data.image_url));
        }

        _button.onClick.AddListener(OnShapeButtonClicked);
    }

    private IEnumerator LoadImage(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var tex = DownloadHandlerTexture.GetContent(www);
                Sprite shapeSprite = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                _shapeImage.sprite = shapeSprite;

                Destroy(_loadingSpinner.gameObject);
            }
        }
    }

    private void OnShapeButtonClicked()
    {
        if(OnClicked != null && _shapeImage.sprite != null)
        {
            OnClicked(_shapeNameText.text, _shapeImage.sprite);
        }
    }

}
