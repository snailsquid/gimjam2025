using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
[CustomEditor(typeof(AttachmentParser))]
[CanEditMultipleObjects]
public class AttachmentParserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AttachmentParser parser = (AttachmentParser)target;
        parser.attachment1 = (Attachment)EditorGUILayout.ObjectField("Attachment 1", parser.attachment1, typeof(Attachment), true);
        parser.attachment2 = (Attachment)EditorGUILayout.ObjectField("Attachment 2", parser.attachment2, typeof(Attachment), true);
        parser.areSame = EditorGUILayout.Toggle("Are Same", parser.areSame);
        if (GUILayout.Button("Are these the same?"))
        {
            parser.CheckIfSame();
        }
    }
}