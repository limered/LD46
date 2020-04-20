using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System;

[InitializeOnLoad]
public class SoundPreviewShortcut
{
    [Shortcut("Preview audio", KeyCode.Space)]
    private static void PreviewAudio()
    {
        if (Selection.activeObject is AudioClip)
        {
            PlayClip(Selection.activeObject as AudioClip);
        }
    }

    // Play and Stop is from https://forum.unity.com/threads/way-to-play-audio-in-editor-using-an-editor-script.132042/
    private static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
            null);

        method.Invoke(null, new object[] { clip, startSample, loop });
    }

    public static void StopAllClips()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "StopAllClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] { },
            null);

        method.Invoke(null, new object[] { });
    }
}