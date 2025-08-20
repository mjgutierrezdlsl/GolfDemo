using UnityEngine;
using UnityEngine.UI;

public class BackgroundScrollingController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 0.5f;
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    private void Update()
    {
        _image.material.SetTextureOffset("_MainTex", new(Time.time*_scrollSpeed,0f));
    }
}
