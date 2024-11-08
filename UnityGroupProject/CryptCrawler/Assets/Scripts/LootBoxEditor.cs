#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LootBox))]
public class LootBoxEditor : Editor
{
    LootBox chest;
    SerializedProperty interactionTextProperty;

    private void OnEnable()
    {
        chest = (LootBox)target;

        interactionTextProperty = serializedObject.FindProperty("interactionText");
    }

    public override void OnInspectorGUI()
    {
        chest.openingMethod =
            (OpeningMethods)EditorGUILayout.EnumPopup("Opening Method", chest.openingMethod);

        if (chest.openingMethod == OpeningMethods.OpenOnCollision
        || chest.openingMethod == OpeningMethods.OpenOnKeyPress)
        {
            chest.playerTag = EditorGUILayout.TextField("Player Tag", chest.playerTag);
            if (chest.openingMethod == OpeningMethods.OpenOnKeyPress)
            {
                chest.keyCode = (KeyCode)EditorGUILayout.EnumPopup("Key Code", chest.keyCode);
            }
        }

        chest.bouncingBox = EditorGUILayout.Toggle("Bouncing Animation", chest.bouncingBox);
        chest.BounceBox(chest.bouncingBox);

        chest.closeOnExit = EditorGUILayout.Toggle("Close On Exit", chest.closeOnExit);

        EditorGUILayout.PropertyField(interactionTextProperty, new GUIContent("Interaction Text"));

        if (EditorApplication.isPlaying)
        {
            if (chest.isOpen)
            {
                if (GUILayout.Button("Close Chest"))
                {
                    chest.Close();
                }
            }
            else
            {
                if (GUILayout.Button("Open Chest"))
                {
                    chest.Open();
                }
            }
        }

        SerializedProperty loots = serializedObject.FindProperty("boxContents");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(loots, true);
        EditorGUI.EndChangeCheck();

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed && !EditorApplication.isPlaying)
        {
            EditorUtility.SetDirty(chest);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(chest.gameObject.scene);
        }
    }
}
#endif