using LootLocker.Requests;
using UnityEngine;
using UnityEngine.UI;

public class LootLockerRankItemController : MonoBehaviour
{
    [SerializeField] Text _rankText, _playerNameText, _playerScoreText;
    [SerializeField] Color _normalTextColor, _ownerTextColor;
    public void _LoadRank(LootLockerLeaderboardMember iRank,int iPlayerID)
    {
        _rankText.text = iRank.rank.ToString();
        _playerNameText.text = iRank.player.name;
        _playerScoreText.text = iRank.score.ToString();
        if (iRank.player.id.Equals(iPlayerID))
        {
            _rankText.color = _playerNameText.color = _playerScoreText.color = _ownerTextColor;
        }
        else
        {
            _rankText.color = _playerNameText.color = _playerScoreText.color = _normalTextColor;
        }
        
    }
}
