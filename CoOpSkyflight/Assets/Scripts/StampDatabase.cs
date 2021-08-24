using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StampDatabase", menuName = "ScriptableObjects/StampDatabase", order = 1)]
public class StampDatabase : ScriptableObject
{
    [System.Serializable]
    public class StampDataContainer
    {
        public string stampName;
        public Texture2D stampTexture;
        public PaintMode paintMode;
        //[HideInInspector]
        public Stamp stamp;
    }

    public List<StampDataContainer> stamps;

    void OnEnable()
    {
        foreach(StampDataContainer stampContainer in stamps)
        {
            stampContainer.stamp = new Stamp(stampContainer.stampTexture);
        }
    }
}
