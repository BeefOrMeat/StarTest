using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConstellationData))]
public class ConstellationDataEditor : Editor
{
    private float mRadius = 100.0f;

    private float mLineWidth = 0.25f;
    private float mLinePadding = 2.0f;

    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示
        base.OnInspectorGUI();

        mRadius = EditorGUILayout.FloatField("Star Radius", mRadius);

        ConstellationData data = target as ConstellationData;

        //ボタンを表示
        if (GUILayout.Button("Normalize"))
        {
            data.Normalize(mRadius);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("AddStar"))
        {
            data.AddStar();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("AddLine"))
        {
            data.AddLine(mLineWidth, mLinePadding);
        }
    }
}
