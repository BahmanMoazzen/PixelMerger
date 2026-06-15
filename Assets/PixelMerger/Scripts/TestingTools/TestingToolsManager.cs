using UnityEngine;

public class TestingToolsManager : MonoBehaviour
{
    public void UnlockAllDecks()
    {
        foreach (var deck in A.GameSetting.AllDecks)
        {
            deck.UnLock();
        }
    }
    public void LockAllDecks()
    {
        foreach (var deck in A.GameSetting.AllDecks)
        {
            deck.IsLocked = true;
        }
    }
    public void UnlockDeck(int iDeckOrder)
    {
        A.GameSetting.AllDecks[iDeckOrder].UnLock();

    }
    public void LockDeck(int iDeckOrder)
    {
        A.GameSetting.AllDecks[iDeckOrder].IsLocked = true;

    }
    public void AddCoin(int iAmount)
    {
        A.GameSetting.ScoreSavable._ChangeAmount(iAmount);
    }
    public void RemoveCoin(int iAmount)
    {
        A.GameSetting.ScoreSavable._ChangeAmount(-iAmount);

    }
    public void AddHammer(int iAmount)
    {
        A.GameSetting.HammerSavable._ChangeAmount(iAmount);
    }
    public void AddUnicolor(int iAmount)
    {
        A.GameSetting.UnicolorSavable._ChangeAmount(iAmount);

    }
    public void AddTiltLeft(int iAmount)
    {
        A.GameSetting.TiltLeftSavable._ChangeAmount(iAmount);
    }
    public void AddTiltRight(int iAmount)
    {
        A.GameSetting.TiltRightSavable._ChangeAmount(iAmount);
    }

}
