using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class DeckCreatorWindow : EditorWindow
{

    [MenuItem("Window/Pixel Merger/Deck Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<DeckCreatorWindow>("Deck Creator");
    }
    PixelInfo _createPixel(Object iAssetObject)
    {
        PixelInfo pi = ScriptableObject.CreateInstance<PixelInfo>();
        pi.MergerName = A.Pixels.PixelNameFromFileName(iAssetObject.name);
        pi.MergerOrder = A.Pixels.PixelOrderFromFileName(iAssetObject.name);
        pi.MergerSprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(iAssetObject));


        string path = AssetDatabase.GetAssetPath(iAssetObject) + ".asset";
        AssetDatabase.CreateAsset(pi, path);
        AssetDatabase.SaveAssets();
        return pi;

    }
    void _createDeck(Object[] iSelection,string iDeckName)
    {
        string rootFolder = AssetDatabase.GetAssetPath(iSelection[0]);
        rootFolder = rootFolder.Substring(0,rootFolder.LastIndexOf("/")+1 );
        PixelDeckInfo PDI = ScriptableObject.CreateInstance<PixelDeckInfo>();
        PDI.DeckPixels = new List<PixelInfo>();
        PDI.DeckName = iDeckName;
        string path = rootFolder + iDeckName + ".asset";
        AssetDatabase.CreateAsset(PDI, path);
        AssetDatabase.SaveAssets();
        foreach (Object o in iSelection)
        {
            if (o.name.Substring(0, 2).Equals("BG"))
            {
                PDI.DeckBackGround = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(o));

            }
            else if(o.name.Substring(0, 2).Equals("LG"))
            {
                PDI.DeckIcon = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(o));
            }
            else
            {

                PixelInfo pi = ScriptableObject.CreateInstance<PixelInfo>();
                pi.MergerName = A.Pixels.PixelNameFromFileName(o.name);
                pi.MergerOrder = A.Pixels.PixelOrderFromFileName(o.name);
                pi.MergerSprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(o));
                pi.MergerDeckInfo = PDI;

                string pipath = AssetDatabase.GetAssetPath(o) + ".asset";
                AssetDatabase.CreateAsset(pi, pipath);
                AssetDatabase.SaveAssets();
                
                
                PDI.DeckPixels.Add(pi);
            }
        }



        //PDI.DeckName = iDeckName;
        //string path = rootFolder +iDeckName+ ".asset";
        AssetDatabase.CreateAsset(PDI, path);
        AssetDatabase.SaveAssets();
    }
    string deckName = "DeckName";
    void OnGUI()
    {
        GUILayout.Label("Created By BAHMAN Moazzen",EditorStyles.boldLabel);
        
        deckName = EditorGUILayout.TextField("Deck Name:",deckName);
        
        if (GUILayout.Button("Create Deck"))
        {
            _createDeck(Selection.objects,deckName.Trim());
        }
        if (GUILayout.Button("Create Pixel"))
        {
            foreach (var g in Selection.objects)
            {
                Debug.Log(g.name);
                _createPixel(g);

            }
            AssetDatabase.Refresh();
        }
        //if (GUILayout.Button("Create Deck"))
        //{
        //    PixelDeckInfo pixelDeckInfo = ScriptableObject.CreateInstance<PixelDeckInfo>();
        //    string rootPath = Application.dataPath+ AssetDatabase.GetAssetPath(Selection.objects[0]);
        //    string deckName = Selection.objects[0].name;


        //    Sprite[] sprites = Resources.LoadAll<Sprite>(rootPath );
        //    foreach (Sprite s in sprites)
        //    {
        //        if (s.name.Substring(0, 2).Equals("LG"))
        //        {
        //            pixelDeckInfo.DeckName = pixelDeckInfo.mergerName(s.name);
        //            pixelDeckInfo.DeckIcon = s;
        //            //Debug.Log(DeckName);
        //        }
        //        else if (s.name.Substring(0, 2).Equals("BG"))
        //        {
        //            pixelDeckInfo.DeckBackGround = s;
        //        }
        //        else
        //        {
        //            PixelInfo pi = ScriptableObject.CreateInstance<PixelInfo>();
        //            //MergerDeck m = new MergerDeck();

        //            //m.MergerSprite = s;
        //            pi.MergerSprite = s;
        //            //m.MergerName = pixelDeckInfo.mergerName(s.name);
        //            pi.MergerName = pixelDeckInfo.mergerName(s.name);
        //            //m.MergerOrder = pixelDeckInfo.mergerOrder(s.name);
        //            pi.MergerOrder = pixelDeckInfo.mergerOrder(s.name);
        //            //m.MergerDeckInfo = pixelDeckInfo;
        //            pi.MergerDeckInfo = pixelDeckInfo;

        //            AssetDatabase.CreateAsset(pi, rootPath+"/"+pi.MergerName);
        //            AssetDatabase.SaveAssets();
        //            AssetDatabase.Refresh();

        //            pixelDeckInfo.DeckSprites.Add(pi);

        //            //Debug.Log(m.MergerName);
        //        }

        //AssetDatabase.CreateAsset(pixelDeckInfo, rootPath + "/" + pixelDeckInfo.DeckName);
        //    AssetDatabase.SaveAssets();
        //    AssetDatabase.Refresh();
        //    }
        //    

        //    // MyClass is inheritant from ScriptableObject base class

        //    // path has to start at "Assets"
        //    //string path = AssetDatabase.GetAssetPath(Selection.objects[0])+"/pixel.asset";
        //    //AssetDatabase.CreateAsset(pi, path);
        //    //AssetDatabase.SaveAssets();
        //    //AssetDatabase.Refresh();
        //    //EditorUtility.FocusProjectWindow();
        //    //Selection.activeObject = pi;
        //}

    }
}



