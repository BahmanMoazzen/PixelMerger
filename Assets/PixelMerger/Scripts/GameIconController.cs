using TMPro;
using UnityEngine;

public class GameIconController : MonoBehaviour
{
    [SerializeField] SpriteRenderer _Image;
    [SerializeField] TextMeshPro _Name;
    public void _UpdateView(PixelInfo iMerger)
    {
        _Image.sprite = iMerger.MergerSprite;
        
        _Name.text = iMerger.MergerOrder.ToString() + "-" + iMerger.MergerName;
    }
}
