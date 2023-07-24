using System.Collections.Generic;

[System.Serializable]
public class SerializableDic<TKey, TValue>
{
    public List<TKey> keys = new List<TKey>();
    public List<TValue> values = new List<TValue>();

    public void Add(TKey key, TValue value)
    {
        keys.Add(key);
        values.Add(value);
    }

    public void Remove(TKey key)
    {
        int index = keys.IndexOf(key);
        if (index >= 0 && index < values.Count)
        {
            keys.RemoveAt(index);
            values.RemoveAt(index);
        }
    }

    public bool TryGetValue(TKey key)
    {
        int index = keys.IndexOf(key);
        if (index >= 0 && index < values.Count)
        {
            return true;
        }

        return false;
    }

    public TValue GetValueOrDefault(TKey key)
    {
        int index = keys.IndexOf(key);
        if (index >= 0 && index < values.Count)
        {
            return values[index];
        }

        return default(TValue);
    }

}