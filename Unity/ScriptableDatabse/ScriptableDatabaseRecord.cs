using UnityEditor;
using UnityEngine;

namespace Game
{
    public abstract class ScriptableDatabaseRecord : ScriptableObject
    {
        [SerializeField] private int id;
        public int ID => id;


#if UNITY_EDITOR
        public void AssignId(int id)
        {
            this.id = id;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}