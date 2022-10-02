using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class UnityFileUtils : MonoBehaviour
{
    static string[] m_FilePaths;
    [MenuItem("Assets/Copy File",false,10)]
    static void Copy()
    {
        m_FilePaths = new string[Selection.assetGUIDs.Length];
        for (int i = 0; i < Selection.assetGUIDs.Length; i++)
        {
            m_FilePaths[i] = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i]);
            Debug.Log(m_FilePaths[i]);
        }
    }

    // static void Cut()
    // {

    // }
    [MenuItem("Assets/Paste File",false,10)]
    static void Paste()
    {
        foreach (string srcPath in m_FilePaths)
        {
            foreach (string dstGUID in Selection.assetGUIDs)
            {
                string dstPath = AssetDatabase.GUIDToAssetPath(dstGUID);
                dstPath = AssetDatabase.GenerateUniqueAssetPath(dstPath+'/'+srcPath.Substring(srcPath.LastIndexOf('/')));
                if(!AssetDatabase.CopyAsset(srcPath,dstPath))
                {
                    Debug.LogWarning($"Failed to copy {srcPath}");
                }
            }
        }
        //m_FilePaths = null;
    }
    
    [MenuItem("Assets/Paste File",true,10)]
    static bool ValidatePaste()
    {
        if(m_FilePaths!=null)
        {
            return true;
        }
        return false;
    }
}
