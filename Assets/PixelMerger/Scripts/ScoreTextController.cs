using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ScoreTextController : MonoBehaviour
{
    const byte ANIM_DURATION = 1;
    const byte BASE_FONT_SIZE = 4;
    [SerializeField] TextMeshPro _textScore;

    /// <summary>
    /// initial setup for score text animation and text value, also destroy the gameobject after animation duration
    /// </summary>
    /// <param name="iScore">the number to show in the text</param>

    /// <param name="iOrder">the order of the text, used to increase font size for each order</param>
    public void _Setup(int iScore, int iOrder)
    {
        _textScore.text = A.Tags.PLUS_SIGN + iScore.ToString();
        _textScore.fontSize = BASE_FONT_SIZE + iOrder;
        _textScore.gameObject.SetActive(true);
        Destroy(gameObject, ANIM_DURATION);
    }

}
