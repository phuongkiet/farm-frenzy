using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemContainer))]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        ItemContainer container = target as ItemContainer;
        if(GUILayout.Button("Clear Container"))
        {
            for(int i = 0; i < container.slots.Count; i++)
            {
                container.slots[i].item = null;
                container.slots[i].count = 0;
            }
        }
        DrawDefaultInspector();
    }
}
