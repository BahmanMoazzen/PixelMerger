
using UnityEngine;
[CreateAssetMenu(fileName ="NewPixel",menuName ="Pixel Merger/New Pixel",order = 0)]
public class PixelInfo : ScriptableObject
{
    public string MergerName;
    public Sprite MergerSprite;
    public int MergerOrder;
    public PixelDeckInfo MergerDeckInfo;
}
