using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using Bytes.Language;

namespace Bytes.BytesEditor.Language
{
    [CustomEditor(typeof(LangManager))]
    public class BEditorLangManager : Editor
    {
        private LangManager targetObject;
        private void OnEnable()
        {
            targetObject = (LangManager)target;
            targetObject.Inititialize();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("Here to help ya: " + targetObject.name, MessageType.Info);

            if (GUILayout.Button("Load text files"))
            {
                targetObject.Inititialize();
            }

        }
    }
}