using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class Wands : ScriptableObject
{
    public GameObject wandModel;
    public GameObject Spell;
    public float shootRate;
    public Skills skill;
    public AudioClip audio;
}
