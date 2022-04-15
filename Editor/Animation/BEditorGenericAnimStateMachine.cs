using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace Bytes.BytesEditor
{
    [CustomEditor(typeof(GenericAnimationStateMachine))]
    public class BEditorGenericAnimStateMachine : Editor
    {
        private GenericAnimationStateMachine targetObject;
        private void OnEnable()
        {
            targetObject = (GenericAnimationStateMachine)target;
            targetObject.Initialize();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("Here to help ya: " + targetObject.name, MessageType.Info);

            if (GUILayout.Button("Initialize GenericAnimationStateMachine()"))
            {
                targetObject.Initialize();
            }

        }
    }
}