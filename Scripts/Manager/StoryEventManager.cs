using System;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventManager : Singleton<StoryEventManager>, ISavable
{
    public static event Action<string> OnEvent;

    public List<string> _shop = new List<string>();
    public List<string> _scenario = new List<string>();
    public List<string> _mapDiscovery = new List<string>();
    public List<string> _opennedChests = new List<string>();
    //public List<string> Tutorial = new List<string>();

    public void TriggerEvent(string eventName)
    {
        OnEvent?.Invoke(eventName );
    }

    public void AddShopEvent(string eventName)
    {
        if (!_shop.Exists(x => x == eventName))
        {
            _shop.Add(eventName);
            OnEvent?.Invoke(eventName);
        }
    }

    public void AddScenarioEvent(string eventName)
    {
        if (!_scenario.Exists(x => x == eventName))
        {
            _scenario.Add(eventName);
            OnEvent?.Invoke(eventName);
        }
    }

    public void AddMapDiscoveryEvent(string eventName)
    {
        if (!_mapDiscovery.Exists(x => x == eventName))
        {
            _mapDiscovery.Add(eventName);
            OnEvent?.Invoke(eventName);
        }
    }

    public void AddOpennedChestsEvent(string eventName)
    {
        if (!_opennedChests.Exists(x => x == eventName))
        {
            _opennedChests.Add(eventName);
            OnEvent?.Invoke(eventName);
        }
    }

    /*
    public static void AddTutorialEvent(string eventName)
    {
        if (!Tutorial.Exists(x => x == eventName))
        {
            Tutorial.Add(eventName);
        }
    }
    */

    public StoryEventSaveModel Get()
    {
        return new StoryEventSaveModel
        {
            Shop = _shop,
            Scenario = _scenario,
            MapDiscovery = _mapDiscovery,
            OpennedChests = _opennedChests
        };
    }

    public void Set(StoryEventSaveModel saveModel)
    {
        _shop = saveModel.Shop;
        _scenario = saveModel.Scenario;
        _mapDiscovery = saveModel.MapDiscovery;
        _opennedChests = saveModel.OpennedChests;
    }

    public string ToJsonString()
    {
        return JsonUtility.ToJson(Get());
    }

    public void Load(string json)
    {
        Set(JsonUtility.FromJson<StoryEventSaveModel>(json));
    }

}

[Serializable]
public class StoryEventSaveModel
{
    public List<string> Shop = new List<string>();
    public List<string> Scenario = new List<string>();
    public List<string> MapDiscovery = new List<string>();
    public List<string> OpennedChests = new List<string>();
}