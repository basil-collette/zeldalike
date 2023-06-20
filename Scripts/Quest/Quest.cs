using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest")]
public class Quest : ScriptableObject
{
    [SerializeReference] public List<Goal> Goals;
}
