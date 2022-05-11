using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using Bytes.Language;

namespace Bytes.BytesEditor.Language
{
    [CustomEditor(typeof(LangText))]
    public class LangTextEditor : Editor
    {
        private LangText targetObject;
        private void OnEnable()
        {
            targetObject = (LangText)target;
            targetObject.UpdateText();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("Here to help ya: " + targetObject.name, MessageType.Info);

            if (GUILayout.Button("Fetch text"))
            {
                targetObject.UpdateText();
            }

        }
    }
}