using System;
using UnityEngine;

[Serializable]
public class AudioClipData
{
    public AudioClip clip;
    [Range(0f, 2f)] public float volume = 1f;
}
