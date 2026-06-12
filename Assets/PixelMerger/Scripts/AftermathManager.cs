using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AftermathManager : MonoBehaviour
{
    /// <summary>
    /// the best ever score of the player, the score of this run, and the total score of the player are shown in the aftermath screen. these scores are used to show the player how well he did in this run and to encourage him to try again to beat his best score and to increase his total score.
    /// </summary>
    [SerializeField] Text _bestScore;
    /// <summary>
    /// the score of the current run
    /// </summary>
    [SerializeField] Text _thisRunScore;


    /// <summary>
    /// it is visible only when the player has a new best score, and it is invisible otherwise. it is used to show the player that he has a new best score and to encourage him to try again to beat his best score.
    /// </summary>
    [SerializeField] GameObject _newBestRecordText;
    //[SerializeField] LootLockerRankingManager _rankingManager;

    [SerializeField] GameObject _claimButton, _claimDoubleButton;
    public void _Back()
    {
        BAHMANBackButtonManager._Instance._ShowMenu();
    }

    IEnumerator Start()
    {
        yield return null;


        if (A.Levels.SetBestScore(A.Levels.ThisRoundScore))
        {
            _newBestRecordText.SetActive(true);
        }
        else
        {
            _newBestRecordText.SetActive(false);
        }

        _bestScore.text = A.Tools.ScoreToTitle(A.Levels.BestScore);
        _thisRunScore.text = A.Tools.ScoreToTitle(A.Levels.ThisRoundScore);
        if (A.Levels.ThisRoundScore <= 0)
        {
            _claimButton.SetActive(false);
            _claimDoubleButton.SetActive(false);
        }

    }

    public void _TryAgain()
    {

        BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.TitleScreenScene);
    }
    public void _ButtonClicked()
    {
        SoundManager._Instance._PlaySound(GameSounds.ButtomClicked);
    }

    public void _ClaimReward()
    {
        _claimReward(A.Levels.ThisRoundScore);
    }
    public void _ClaimDoubleReward()
    {
        BAHMANAdManager.Instance._ShowRewardedAd(_adShowedSuccessful, _adShowedFailure);
    }
    void _adShowedSuccessful()
    {
        _claimReward(A.Levels.ThisRoundScore * 2);
        _claimButton.gameObject.SetActive(false);
        _claimDoubleButton.gameObject.SetActive(false);
    }
    void _adShowedFailure()
    {
        BAHMANMessageBoxManager.Instance._ShowMessage(A.Tags.AdverFailedTag);
        _claimDoubleButton.gameObject.SetActive(false);

    }
    void _claimReward(int iReward)
    {
        A.GameSetting.ScoreSavable._ChangeAmount(iReward);
        //_totalScore.text = A.Tools.ScoreToTitle(A.GameSetting.ScoreSavable._Stock);
    }
}
