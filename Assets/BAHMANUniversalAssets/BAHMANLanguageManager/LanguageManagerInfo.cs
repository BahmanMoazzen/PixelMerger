/*
 *  BAHMAN Language Manager V1.0
 * 
 */
using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewLanguageSetting", order = 1, menuName = "BAHMAN Unity Assets/New Language Settings")]
public class LanguageManagerInfo : ScriptableObject
{
    [SerializeField] bool _enableTranslation;
    [Header("Translate from English to language:")]
    [SerializeField] string[] LanguagesName = { "Farsi" };
    [SerializeField] Languages currentLanguage;
    public Words[] AllWords;
    Dictionary<string, string[]> _WordDictionary;
    const string ACTIVELANGUAGETAG = "BAHMANLANGUAGEMANAGER_CURRENTLANGUAGE";
    private void OnEnable()
    {
        _WordDictionary = new Dictionary<string, string[]>();
        foreach (var word in AllWords)
        {
            _WordDictionary.Add(word.WordTag, word.WordTranslations);
        }

    }
    /// <summary>
    /// translates the word into active language
    /// </summary>
    /// <param name="iWord">the word to translate</param>
    /// <returns></returns>
    public string TranslateWord(string iWord)
    {

        if (_WordDictionary.ContainsKey(iWord))
        {
            return _WordDictionary[iWord][(int)currentLanguage];
        }
        else
        {
            return iWord;
        }
    }

    public bool IsActive
    {
        get
        {
            return _enableTranslation;
        }
    }
    /// <summary>
    /// set and get the active language with save on disk.
    /// </summary>
    public Languages ActiveLanguage
    {
        get
        {
            return (Languages)PlayerPrefs.GetInt(ACTIVELANGUAGETAG, (int)currentLanguage);
        }
        set
        {
            PlayerPrefs.SetInt(ACTIVELANGUAGETAG, (int)currentLanguage);
        }
    }
    /// <summary>
    /// get all the languages name
    /// </summary>
    /// <returns>all the string names of languages</returns>
    public string[] GetLanguageNames()
    {
        return LanguagesName;
    }
    /// <summary>
    /// set the current language by language name
    /// </summary>
    /// <param name="iLanguageName">the name of the language which must be in the list "LanguagesName"</param>
    public void SetActiveLanguage(string iLanguageName)
    {
        int activeLanguageIndex = 0;
        foreach (string langName in LanguagesName)
        {
            if (langName == iLanguageName)
            {
                break;
            }
            activeLanguageIndex++;
        }
        ActiveLanguage = (Languages)activeLanguageIndex;
    }
    /// <summary>
    /// set the current language by language index
    /// </summary>
    /// <param name="iLanguageIndex">language index within the array bounds of "LanguagesName"</param>
    public void SetActiveLanguage(int iLanguageIndex)
    {

        ActiveLanguage = (Languages)iLanguageIndex;
    }



}
public enum Languages { Farsi }
[Serializable]
public struct Words
{
    public string WordTag;
    public string[] WordTranslations;
}
