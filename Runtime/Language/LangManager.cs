using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes.Language
{
    /// <summary>
    /// Folder tree should look like this:
    /// 
    /// *you can rename your files if you set fileName in LoadLang params*
    /// Resources/
    ///     Lang/
    ///         fr/lang.json
    ///         en/lang.json
    /// 
    /// </summary>
    //[ExecuteAlways]
    public class LangManager : MonoBehaviour
    {
        static string TEXT_NOT_FOUND = "TEXT_NOT_FOUND";

        static protected LangManager instance;
        static protected LangManager GetInstance() { return instance; }

        [Header("Params")]
        public string[] languages;
        public bool keepInScene;
        public bool loadLangInAwake;
        public string[] langFilesToLoadInAwake;
        public bool updateAllLangTextInScene;

        [Header("State")]
        public string currentLangage;
        Dictionary<string, string>[] currentLangDataInstances;

        protected virtual void Awake()
        {
            Inititialize();
        }

        public virtual void Inititialize(string[] overrideLangFilesToLoad = null)
        {
            instance = this;

            if (overrideLangFilesToLoad != null) 
            {
                langFilesToLoadInAwake = overrideLangFilesToLoad;
            }

            if (keepInScene) { DontDestroyOnLoad(this.gameObject); }
            if (loadLangInAwake) { LoadLangMultipleFiles(currentLangage, langFilesToLoadInAwake); }

            print("LangManager loaded langs!");
        }

        static public void LoadLang(string lang, string fileName = "-lang")
        {
            GetInstance().currentLangDataInstances = new Dictionary<string, string>[1];
            GetInstance().currentLangage = lang;
            LoadLangDataInstance("Lang/" + lang + "/" + fileName + ".json");
            UpdateAllLangTextInScene();
        }

        static public void LoadLangMultipleFiles(string lang, string[] fileNames)
        {
            GetInstance().currentLangDataInstances = new Dictionary<string, string>[fileNames.Length];
            GetInstance().currentLangage = lang;
            for (int i = 0; i < fileNames.Length; i++)
            {
                LoadLangDataInstance("Lang/" + lang + "/" + fileNames[i] + ".json", true, i);
            }
            UpdateAllLangTextInScene();
        }

        static protected void UpdateAllLangTextInScene()
        {
            if (!GetInstance().updateAllLangTextInScene) { return; }

            foreach (BytesLangText txt in GameObject.FindObjectsOfType<BytesLangText>())
            {
                txt.UpdateText();
            }
        }

        static public string GetText(string id)
        {
            string text = "";
            for (int i = 0; i < GetInstance().currentLangDataInstances.Length; i++)
            {
                text = GetText(id, i);
                if (text != TEXT_NOT_FOUND && text != null) { return text; }
            }
            return text;
        }

        static public string GetText(string id, int specificFileIndex)
        {
            if (GetInstance().currentLangDataInstances.Length <= specificFileIndex) { return TEXT_NOT_FOUND; }
            Dictionary<string, string> langData = GetInstance().currentLangDataInstances[specificFileIndex];
            if (langData == null) { Debug.LogWarning("Lang File index: " + specificFileIndex + " does not exits. There is " + GetInstance().currentLangDataInstances.Length + " files loaded."); return TEXT_NOT_FOUND; }
            return GetText(langData, id);
        }

        static protected string GetText(Dictionary<string, string> langData, string id)
        {
            string text;
            var found = langData.TryGetValue(id, out text);
            if (!found) return TEXT_NOT_FOUND;
            return text;
        }

        static protected void LoadLangDataInstance(string resourcePath, bool additive = false, int index = 0)
        {
            string filePath = resourcePath.Replace(".json", "");
            try
            {
                TextAsset targetFile = Resources.Load<TextAsset>(filePath);

                var langData = SimpleJsonParser.ParseDictionary(targetFile.text);

                if (additive) { GetInstance().currentLangDataInstances[index] = langData; }
                else { GetInstance().currentLangDataInstances[0] = langData; }

                print("Loaded: " + filePath);

            }
            catch (System.Exception exc)
            {
                print("Error! While loading currentLangDataInstance from path: " + filePath);
                print("and error msg is: " + exc);
            }
        }

        static public bool GetIsReady()
        {
            return GetInstance() != null && GetInstance().currentLangDataInstances.Length > 0;
        }

    }
}

public static class SimpleJsonParser
{
    public static Dictionary<string, string> ParseDictionary(string json)
    {
        var result = new Dictionary<string, string>();

        // Remove outer braces
        json = json.Trim().TrimStart('{').TrimEnd('}');

        // Split into key-value pairs
        var pairs = json.Split(',');

        foreach (var pair in pairs)
        {
            var split = pair.Split(new[] { ':' }, 2);
            if (split.Length != 2) continue;

            string key = Unquote(split[0].Trim());
            string value = Unquote(split[1].Trim());

            result[key] = value;
        }

        return result;
    }

    private static string Unquote(string s)
    {
        if (s.StartsWith("\"") && s.EndsWith("\""))
            return s.Substring(1, s.Length - 2).Replace("\\\"", "\"");
        return s;
    }
}