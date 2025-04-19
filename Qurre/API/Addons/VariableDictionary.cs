using System.Collections.Generic;
using JetBrains.Annotations;

namespace Qurre.API.Addons;

[PublicAPI]
public class VariableDictionary<TValue> : Dictionary<string, TValue>
{
    public new TValue this[string key]
    {
        get
        {
            try
            {
                return base[key];
            }
            catch
            {
                return default!;
            }
        }
        set => base[key] = value;
    }

    public bool TryGetAndParse<T>(string key, out T value)
    {
        if (TryGetValue(key, out var pre))
            if (pre is T res)
            {
                value = res;
                return true;
            }

        value = default!;
        return false;
    }

    public new bool TryGetValue(string key, out TValue value)
    {
        try
        {
            return base.TryGetValue(key, out value);
        }
        catch
        {
            value = default!;
            return false;
        }
    }

    public new bool ContainsKey(string key)
    {
        try
        {
            return base.ContainsKey(key);
        }
        catch
        {
            return false;
        }
    }

    public new bool Add(string key, TValue value)
    {
        try
        {
            base.Add(key, value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public new bool Remove(string key)
    {
        try
        {
            return base.Remove(key);
        }
        catch
        {
            return false;
        }
    }
}