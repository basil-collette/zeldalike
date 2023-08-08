using Assets.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DialogueStates")]
public class DialogueStates : ScriptableObject
{
    public List<SerializableWrappedList<string>> States = new List<SerializableWrappedList<string>>();

    public static DialogueStates Current => Resources.Load<DialogueStates>("ScriptableObjects/Dialogues/DialogueStates");

    public static bool HaveSaid(string pnjName, string dialogueCode)
    {
        var pnjNode = Current.States.Find(x => x.Key == pnjName);

        if (pnjNode == null) return false;

        return pnjNode.List.Contains(dialogueCode);
    }

    public static void AddSaid(string pnjName, string dialogueCode)
    {
        /*
        Task.Run(() =>
        {
            
        });
        */

        if (dialogueCode != null && dialogueCode != string.Empty
            && !HaveSaid(pnjName, dialogueCode))
        {
            var tempStates = Current.States;

            var dialoguePnjState = tempStates.Find(x => x.Key == pnjName);
            if (dialoguePnjState == null)
            {
                tempStates.Add(new SerializableWrappedList<string>() { Key = pnjName, List = new List<string>() { dialogueCode } });
            }
            else
            {
                dialoguePnjState.List.Add(dialogueCode);
            }
        }

    }

}