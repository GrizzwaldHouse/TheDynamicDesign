using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "MagicSpell", menuName = "Magic Spells")]
public class MagicSpell : ScriptableObject
{
    // The prefab of the magic spell projectile
   public GameObject magicSpellPrefab;

    // The speed of the magic spell projectile
    [SerializeField]  public float magicSpellSpeed;

    // The damage dealt by the magic spell
    [SerializeField]  public int magicSpellDamage;

    // The cooldown time between magic spell shots
    [SerializeField] public float magicSpellCooldown;

    [SerializeField] public int lifetime;

    [SerializeField] public float radius;
    [SerializeField] public float manacost;
}

