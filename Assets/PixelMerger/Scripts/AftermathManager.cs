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
    /// UI Text component that displays the total score.
    /// </summary>
    /// <remarks>Assigned in the Unity Inspector via SerializeField. May be null if not assigned at
    /// runtime.</remarks>
    [SerializeField] Text _totalScore;

    /// <summary>
    /// it is visible only when the player has a new best score, and it is invisible otherwise. it is used to show the player that he has a new best score and to encourage him to try again to beat his best score.
    /// </summary>
    [SerializeField] GameObject _newBestRecordText;
    //[SerializeField] LootLockerRankingManager _rankingManager;
    public void _Back()
    {
        BAHMANBackButtonManager._Instance._ShowMenu();
    }

    IEnumerator Start()
    {
        yield return null;


        //_rankingManager._SetActiveLeaderBoard((int)A.Levels.DifficultyLevel, A.Levels.ThisRoundScore);
        
        

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
        _totalScore.text = A.Tools.ScoreToTitle(A.GameSetting.ScoreTotal._Stock);

        
    }
    //public void _ShowRanks()
    //{
    //    //_rankingManager._LoadRanking((int)A.Levels.DifficultyLevel, _showRankSuccess, _showRankFailed);
    //}
    //public void _SubmitScore()
    //{
        
    //    //_rankingManager._ShowSubmitForm(_submitRankSuccess,_submitRankFailed);
    //}
    //void _showRankFailed()
    //{
    //    BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.LootLocker.ShowRankFailed);
    //}
    //void _showRankSuccess()
    //{
    //    BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.LootLocker.ShowRankSuccess);
    //}
    //void _submitRankSuccess()
    //{
    //    _submitScoreButton.SetActive(false);
    //    BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.LootLocker.SubmitRankSuccess);
    //    _ShowRanks();
    //}
    //void _submitRankFailed()
    //{
    //    BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.LootLocker.SubmitRankFailed);
    //    BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.CheckInternetConnection);
    //}
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
        _claimReward(A.Levels.ThisRoundScore * 2);
    }
    void _claimReward(int iReward)
    {
        A.GameSetting.ScoreTotal._ChangeAmount(iReward);
        _totalScore.text = A.Tools.ScoreToTitle(A.GameSetting.ScoreTotal._Stock);
    }
}
