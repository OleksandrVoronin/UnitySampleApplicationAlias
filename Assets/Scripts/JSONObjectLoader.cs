using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parses a json file and recreates a described scene.
/// </summary>
public class JSONObjectLoader : MonoBehaviour
{
    #region JSON Data Templates

    [Serializable]
    private class SpawnableObject
    {
        public string type = "";
        public string script = "";
        public Vector3 position = Vector3.zero;
    }

    [Serializable]
    private class SpawnableObjectCollection
    {
        public List<SpawnableObject> game_objects = new List<SpawnableObject>();
    }

    #endregion


    #region Serialized Fields

    [SerializeField]
    private TextAsset m_ToParse = null;

    [SerializeField]
    private JSONObjectDictionary m_ObjectDictionary = null;

    [SerializeField]
    private Transform m_ObjectHost = null;

    #endregion


    #region Public Methods

    public void Init()
    {
        SpawnObjectsFromJSON(m_ToParse.text);
    }

    #endregion


    #region Private Functions

    private void SpawnObjectsFromJSON(string json)
    {
        SpawnableObjectCollection parsedCollection;

        try
        {
            parsedCollection = JsonUtility.FromJson<SpawnableObjectCollection>(json);
        }
        catch (Exception e)
        {
            Debug.LogError("JSON file parsing failed with exception; \n" + e.Message);
            return;
        }

        m_ObjectDictionary.ConstructDictionaries();

        // Go through a list of gameObjects and spawn them at appropriate location with appropriate script.
        for (int i = 0; i < parsedCollection.game_objects.Count; i++)
        {
            SpawnableObject spawnableObject = parsedCollection.game_objects[i];

            GameObject spawnee = Instantiate(m_ObjectDictionary.GetPrefabById(spawnableObject.type));
            spawnee.transform.SetParent(m_ObjectHost);
            spawnee.transform.position = spawnableObject.position;

            if (spawnableObject.script != null)
            {
                spawnee.AddComponent(m_ObjectDictionary.GetScriptTypeById(spawnableObject.script));
            }
        }
    }

    #endregion
}
