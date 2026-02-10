using UnityEditor;
using UnityEngine;
using SoundManager.VirtualListeners;

namespace SoundManager.VirtualListeners.Editor
{
    [CustomEditor(typeof(AudioSourceVirtual))]
    [CanEditMultipleObjects]
    public class AudioSourceVirtualEditor : UnityEditor.Editor
    {
        SerializedProperty clip;
        SerializedProperty outputAudioMixerGroup;
        SerializedProperty playOnAwake;
        SerializedProperty loop;
        SerializedProperty volume;
        SerializedProperty pitch;
        SerializedProperty spatialBlend;
        SerializedProperty rolloffMode;
        SerializedProperty minDistance;
        SerializedProperty maxDistance;
        SerializedProperty dopplerLevel;
        
        SerializedProperty updateListenerWhilePlaying;
        
        static bool show3DKey = true;

        void OnEnable()
        {
            clip = serializedObject.FindProperty("clip");
            outputAudioMixerGroup = serializedObject.FindProperty("outputAudioMixerGroup");
            playOnAwake = serializedObject.FindProperty("playOnAwake");
            loop = serializedObject.FindProperty("loop");
            volume = serializedObject.FindProperty("volume");
            pitch = serializedObject.FindProperty("pitch");
            spatialBlend = serializedObject.FindProperty("spatialBlend");
            rolloffMode = serializedObject.FindProperty("rolloffMode");
            minDistance = serializedObject.FindProperty("minDistance");
            maxDistance = serializedObject.FindProperty("maxDistance");
            dopplerLevel = serializedObject.FindProperty("dopplerLevel");
            
            updateListenerWhilePlaying = serializedObject.FindProperty("updateListenerWhilePlaying");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(clip, new GUIContent("AudioClip"));
            EditorGUILayout.PropertyField(outputAudioMixerGroup, new GUIContent("Output"));
            
            EditorGUILayout.PropertyField(playOnAwake, new GUIContent("Play On Awake"));
            EditorGUILayout.PropertyField(loop, new GUIContent("Loop"));
            
            EditorGUILayout.PropertyField(volume, new GUIContent("Volume"));
            EditorGUILayout.PropertyField(pitch, new GUIContent("Pitch"));
            EditorGUILayout.PropertyField(spatialBlend, new GUIContent("Spatial Blend"));
            
            show3DKey = EditorGUILayout.Foldout(show3DKey, "3D Sound Settings", true);
            if (show3DKey)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(dopplerLevel, new GUIContent("Doppler Level"));
                EditorGUILayout.PropertyField(rolloffMode, new GUIContent("Volume Rolloff"));
                EditorGUILayout.PropertyField(minDistance, new GUIContent("Min Distance"));
                EditorGUILayout.PropertyField(maxDistance, new GUIContent("Max Distance"));
                EditorGUI.indentLevel--;
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Virtual Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(updateListenerWhilePlaying, new GUIContent("Update Listener While Playing"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}

