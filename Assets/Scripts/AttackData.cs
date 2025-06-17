using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    public Attack[] attacks;
}
[System.Serializable]
public class Attack
{
    public string attackName;
    public float minDamage;
    public float maxDamage;
    public float attackDuration;
    public string animationName;
    public string soundName;
}