using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Qurre.API.Addons;

[PublicAPI]
public class JsonConfig(string name)
{
    private static JObject _shared = new();
    private static string _sharedFilePath = string.Empty;

    public string Name { get; } = name;

    private JToken JsonArray => _shared[Name] ??= new JObject();

    public JToken GetToken(string path, string description = "")
    {
        return JsonArray.GetToken(path, description);
    }

    public void SetToken(string path, JToken value, string description = "")
    {
        JsonArray.SetToken(path, value, description);
    }

    public T GetValue<T>(string path, T defaultValue = default!, string description = "") where T : notnull
    {
        var token = GetToken(path, description);

        if (token.Type != JTokenType.Null)
        {
            try
            {
                var value = token.ToObject<T>();
                if (value is not null)
                    return value;
            }
            catch
            {
                // ignored
            }
        }

        SetValue(path, defaultValue);
        return defaultValue;
    }

    public void SetValue<T>(string path, T value) where T : notnull
    {
        var valueToken = JToken.FromObject(value);
        SetToken(path, valueToken);
    }

    public JToken SafeGetTokenValue(string name, JToken toValue, string desc = "", JToken? source = null)
    {
        try
        {
            var par = source ?? JsonArray;
            var val = par[name];

            if (val is not null)
                return val;

            if (desc.Trim() != string.Empty)
                par[name + "_desc"] = desc.Trim();

            par[name] = toValue;
            return toValue;
        }
        catch (Exception e)
        {
            //string assembly = Assembly.GetCallingAssembly().GetName().Name;
            //string text =
            //    $"[ERROR] [{assembly} => JsonConfig] Occurred error in [SafeGetTokenValue]:\n{e}\n{e.StackTrace}";
            //ServerConsole.AddLog(text, ConsoleColor.Red);
            //Log.LogTxt(text);

            return toValue;
        }
    }

    public T SafeGetValue<T>(string name, T value, string desc = "", JToken? source = null)
    {
        try
        {
            var par = source ?? JsonArray;
            var val = par[name];

            if (val is not null)
            {
                var retVal = val.ToObject<T>();
                if (retVal is not null)
                    return retVal;
            }

            if (desc.Trim() != string.Empty)
                par[name + "_desc"] = desc.Trim();

            par[name] = ConvertObject(value);
            return value;
        }
        catch (Exception e)
        {
            //string assembly = Assembly.GetCallingAssembly().GetName().Name;
            //string text = $"[ERROR] [{assembly} => JsonConfig] Occurred error in [SafeGetValue]:\n{e}\n{e.StackTrace}";
            //ServerConsole.AddLog(text, ConsoleColor.Red);
            //    Log.LogTxt(text);
            return value;
        }
    }

    public static JsonConfig Create(string name)
    {
        return new JsonConfig(name);
    }

    public static void UpdateFile()
    {
        File.WriteAllText(_sharedFilePath, _shared.ToString());
    }

    public static void RefreshConfig()
    {
        Init();
    }

    internal static void Init()
    {
        if (!Directory.Exists(Paths.Configs))
            Directory.CreateDirectory(Paths.Configs);

        _sharedFilePath = Path.Combine(Paths.Configs, $"{Server.Port}.json");
        if (!File.Exists(_sharedFilePath))
        {
            var content = new UTF8Encoding(true).GetBytes("{\n    \n}");
            using var fileStream = File.Create(_sharedFilePath);
            fileStream.Write(content, 0, content.Length);
        }

        var fileContent = File.ReadAllText(_sharedFilePath);

        try
        {
            _shared = JObject.Parse(fileContent);
        }
        catch (Exception ex)
        {
            ServerConsole.AddLog(ex.Message, ConsoleColor.Red);
            File.WriteAllText(_sharedFilePath + ".bak", fileContent);

            File.WriteAllText(_sharedFilePath, "{\n    \n}");
            _shared = JObject.Parse("{\n    \n}");
        }
    }

    private JToken? ConvertObject<T>(T obj)
    {
        switch (obj)
        {
            case string text:
                return text;

            case bool flag:
                return flag;

            case IEnumerable<object> list:
                return new JArray(list.Select(ConvertObject));

            case Vector3 vec:
                return new JObject
                {
                    ["x"] = vec.x,
                    ["y"] = vec.y,
                    ["z"] = vec.z
                };
            case null:
                return null;
        }

        if (TryParseNumber(obj, out var numberJToken))
            return numberJToken;

        return JObject.FromObject(obj);

        bool TryParseNumber(object obj, out JToken? number)
        {
            if (long.TryParse(obj.ToString(), out var longResult))
            {
                number = new JValue(longResult);
                return true;
            }

            if (float.TryParse(obj.ToString(), out var floatResult) && !float.IsNaN(floatResult))
            {
                number = new JValue(floatResult);
                return true;
            }

            number = null;
            return false;
        }

        static void MergeArray(JArray jt, IEnumerable<object> arr)
        {
            var i = 0;
            foreach (var targetItem in arr)
            {
                if (i < jt.Count)
                {
                    var sourceItem = jt[i];

                    if (sourceItem is JContainer existingContainer)
                    {
                        existingContainer.Merge(targetItem);
                    }
                    else if (targetItem is not null)
                    {
                        var contentValue = CreateFromContent(targetItem);
                        if (contentValue.Type is not JTokenType.Null)
                            jt[i] = contentValue;
                    }
                }
                else
                {
                    try
                    {
                        jt.Add(targetItem);
                    }
                    catch
                    {
                        jt.Add(JObject.FromObject(targetItem));
                    }
                }

                i++;
            }
        }

        static JToken CreateFromContent(object content)
        {
            return content switch
            {
                JToken token => token,
                Vector3 vector => new JObject { ["x"] = vector.x, ["y"] = vector.y, ["z"] = vector.z },
                _ => new JValue(content)
            };
        } // end void
    } // end void
}

public static class JsonExtensions
{
    public static JToken GetToken(this JToken token, string path, string description = "")
    {
        if (token is not JObject root)
            throw new InvalidOperationException("Root token must be a JObject.");

        var parts = path.Split('.');
        var current = root;

        for (var i = 0; i < parts.Length - 1; i++)
        {
            var part = parts[i];
            var childToken = current[part];

            if (childToken != null && childToken.Type != JTokenType.Null && childToken.Type != JTokenType.Object)
                throw new InvalidOperationException($"Token at '{string.Join(".", parts.Take(i + 1))}' is not an object.");

            if (childToken is not JObject next)
            {
                next = new JObject();
                current[part] = next;
            }

            current = next;
        }

        var key = parts[^1];
        var existing = current[key];

        if (existing != null)
            return existing;

        if (!string.IsNullOrWhiteSpace(description))
            current[$"{key}_desc"] = description.Trim();

        var placeholder = JValue.CreateNull();
        current[key] = placeholder;

        return placeholder;
    }

    public static void SetToken(this JToken token, string path, JToken value, string description = "")
    {
        if (token is not JObject root)
            throw new InvalidOperationException("Root token must be a JObject.");

        var parts = path.Split('.');
        var current = root;

        for (var i = 0; i < parts.Length - 1; i++)
        {
            var part = parts[i];
            var childToken = current[part];

            if (childToken != null && childToken.Type != JTokenType.Null && childToken.Type != JTokenType.Object)
                throw new InvalidOperationException($"Token at '{string.Join(".", parts.Take(i + 1))}' is not an object.");

            if (childToken is not JObject next)
            {
                next = new JObject();
                current[part] = next;
            }

            current = next;
        }

        var key = parts[^1];

        if (!string.IsNullOrWhiteSpace(description))
            current[$"{key}_desc"] = description.Trim();

        current[key] = value;
    }


    public static T GetValue<T>(this JToken root, string path, T defaultValue = default!, string description = "") where T : notnull
    {
        var token = root.GetToken(path, description);

        if (token.Type != JTokenType.Null)
        {
            try
            {
                var value = token.ToObject<T>();
                if (value is not null)
                    return value;
            }
            catch
            {
                // ignored
            }
        }

        root.SetValue(path, defaultValue);
        return defaultValue;
    }

    public static void SetValue<T>(this JToken root, string path, T value) where T : notnull
    {
        var valueToken = JToken.FromObject(value);
        root.SetToken(path, valueToken);
    }
}