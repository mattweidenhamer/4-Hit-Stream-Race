using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "Racer Data", menuName = "ScriptableObjects/Racer", order = 1)]
public class RacerDataBaseScript : ScriptableObject
{
    public string racerName;
    public AnimatorController animatorController;
    public Sprite previewSprite;

}
