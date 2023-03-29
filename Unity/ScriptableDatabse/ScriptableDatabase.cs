
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public abstract class BaseScriptableDatabase : ScriptableObject
    {
        public abstract void AssignUniqueIDs();
    }

    public abstract class ScriptableDatabase<T> : BaseScriptableDatabase where T : ScriptableDatabaseRecord
    {
        [SerializeField] private T[] records;

        [SerializeField, HideInInspector] private SerializableDictionary<int, T> registeredIds;

        public T Get(int id)
        {
            return registeredIds[id];
        }

        [ContextMenu("Clear registered ids")]
        public void ClearRegistered()
        {
            registeredIds = new SerializableDictionary<int, T>();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public override void AssignUniqueIDs()
        {
            List<int> visitedIds = new();
            
            List<T> repeatedIds = new();

            List<int> registeredToRemove = new();

            List<T> entriesConsidered = new();
            
            foreach (var item in registeredIds)
            {
                if (item.Value == null)
                {
                    registeredToRemove.Add(item.Key);
                }
            }

            for (int i = 0; i < registeredToRemove.Count; i++)
            {
                registeredIds.Remove(registeredToRemove[i]);
            }

            for (int i = 0; i < records.Length; i++)
            {
                if (entriesConsidered.Contains(records[i]))
                {
                    Debug.LogError("Duplicate of record on index " + i + " with record " + entriesConsidered.IndexOf(records[i]));
                    return;
                }

                if (visitedIds.Contains(records[i].ID)) //id is repeated
                {
                    repeatedIds.Add(records[i]);
                }
                else //id is not repeated yet
                {
                    if (registeredIds.TryGetValue(records[i].ID, out T ch)) //id was registered
                    {
                        if (records[i] == ch)
                        {
                            visitedIds.Add(records[i].ID);
                        }
                        else
                        {
                            repeatedIds.Add(records[i]);
                        }
                        
                    }
                    else //id is unique
                    {
                        visitedIds.Add(records[i].ID);
                        registeredIds[records[i].ID] = records[i];
                    }
                }
                
                entriesConsidered.Add(records[i]);
            }

            for (int i = 0; i < repeatedIds.Count; i++)
            {
                var idGenerated = GetUniqueIDExludingList(visitedIds);
                
                repeatedIds[i].AssignId(idGenerated);
                registeredIds[idGenerated] = repeatedIds[i];
                
                visitedIds.Add(idGenerated);
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
        }

        public int GetUniqueIDExludingList(List<int> visitedIds)
        {
            int testedId = 0;
            while (true)
            {
                if (visitedIds.Contains(testedId))
                {
                    testedId++;
                }
                else
                {
                    return testedId;
                }
            }
        }
    }
    
#if UNITY_EDITOR   
    [CustomEditor(typeof(BaseScriptableDatabase),true)]
    public class ScriptableDatabaseEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Assign unique IDs and Register"))
            {
                ((BaseScriptableDatabase)target).AssignUniqueIDs();
            }
        }
    }
#endif

}
