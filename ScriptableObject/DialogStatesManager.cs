using Assets.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogStatesManager : Singleton<StoryEventManager>, ISavable
{
    public List<SerializableWrappedList<string>> _states = new List<SerializableWrappedList<string>>();

    public bool HaveSaid(string pnjName, string dialogueCode)
    {
        var pnjNode = _states.Find(x => x.Key == pnjName);

        if (pnjNode == null) return false;

        return pnjNode.List.Contains(dialogueCode);
    }

    public void AddSaid(string pnjName, string dialogueCode)
    {
        /*
        Task.Run(() =>
        {
            
        });
        */

        if (dialogueCode != null && dialogueCode != string.Empty
            && !HaveSaid(pnjName, dialogueCode))
        {
            var tempStates = _states;

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

    public string ToJsonString()
    {
        return JsonUtility.ToJson(Get());
    }

    public void Load(string json)
    {
        Set(JsonUtility.FromJson<DialogStatesSaveModel>(json));
    }

    public DialogStatesSaveModel Get()
    {
        return new DialogStatesSaveModel
        {
            States = _states
        };
    }

    public void Set(DialogStatesSaveModel saveModel)
    {
        _states = saveModel.States;
    }

}

[Serializable]
public class DialogStatesSaveModel
{
    public List<SerializableWrappedList<string>> States = new List<SerializableWrappedList<string>>();
}