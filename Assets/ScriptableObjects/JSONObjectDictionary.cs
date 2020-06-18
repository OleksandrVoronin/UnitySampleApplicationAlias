using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// This is a scriptable object for defining id-object links for parsing the JSON file.
/// </summary>
[CreateAssetMenu(fileName = "JSONObjectDictionary", menuName = "ScriptableObjects/JSONObjectDictionary", order = 1)]
public class JSONObjectDictionary : ScriptableObject
{
    #region Editor Drawer Classes

    [Serializable]
    private class PrefabKeyValuePair
    {
        public string id = "";
        public GameObject prefab = null;
    }

    [Serializable]
    private class ScriptKeyValuePair
    {
        public string id = "";
        public TextAsset script = null;
    }

    #endregion


    #region Serialized Fields

    [SerializeField]
    private PrefabKeyValuePair[] m_PrefabList = new PrefabKeyValuePair[0];

    [SerializeField]
    private ScriptKeyValuePair[] m_ScriptsList = new ScriptKeyValuePair[0];

    #endregion


    #region Private Fields

    /*
     * Instead of using input lists for fetching records during JSON parsing stage,
     * I'll auto-convert them into dictionaries. This means O(1) access time. Irrelevant for small 
     * collections, but I'm assuming scalability for the purposes of this assignment.
     * Also lets me run some error-checking for whether or not I have unique IDs/valid links.
     */

    private Dictionary<string, GameObject> m_PrefabDictionary;
     
    private Dictionary<string, Type> m_ScriptTypeDictionary;

    #endregion


    #region Public Methods

    public GameObject GetPrefabById(String id)
    {
        if (m_PrefabDictionary.ContainsKey(id))
        {
            return m_PrefabDictionary[id];
        }

        Debug.LogWarning("Prefab looked up by id " + id + " does not exist. Skipping.");
        return null;
    }

    public Type GetScriptTypeById(String id)
    {
        if (m_ScriptTypeDictionary.ContainsKey(id))
        {
            return m_ScriptTypeDictionary[id];
        }

        Debug.LogWarning("Script looked up by id " + id + " does not exist. Skipping.");
        return null;
    }

    public void ConstructDictionaries()
    {
        // Loop through the prefabs list, construct a dictionary
        m_PrefabDictionary = new Dictionary<string, GameObject>();
        for (int i = 0; i < m_PrefabList.Length; i++)
        {
            PrefabKeyValuePair kvp = m_PrefabList[i];

            if (m_PrefabDictionary.ContainsKey(kvp.id))
            {
                Debug.LogWarning("Id " + kvp.id + " is already used for prefab dictionary, omitting this record.");
                continue;
            }

            m_PrefabDictionary.Add(kvp.id, kvp.prefab);
        }

        // Loop through the scripts list, construct a dictionary
        m_ScriptTypeDictionary = new Dictionary<string, Type>();
        for (int i = 0; i < m_ScriptsList.Length; i++)
        {
            ScriptKeyValuePair kvp = m_ScriptsList[i];

            if (m_ScriptTypeDictionary.ContainsKey(kvp.id))
            {
                Debug.LogWarning("Id " + kvp.id + " is already used for script dictionary, omitting this record.");
                continue;
            }

            // Get Type from the associated asset, a bit tricky
            if (kvp.script != null)
            {
                Assembly asm = Assembly.Load(this.GetType().Assembly.FullName);
                
                if (asm != null) {
                    Type type = asm.GetType(kvp.script.name);
                    
                    if (type != null)
                    {
                        m_ScriptTypeDictionary.Add(kvp.id, type);
                        continue;
                    }
                }
            }

            Debug.LogWarning("Id " + kvp.id + " is mapped to an object that isn't a script, omitting this record.");
        }
    }

    #endregion
}
