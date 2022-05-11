using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes.Language
{

    [System.Serializable]
    public class LangDataInstance
    {
        public List<LangTextInstance> texts;
        // Test data
        static public LangDataInstance testData = new LangDataInstance()
        {
            texts = new List<LangTextInstance> {
                new LangTextInstance(0, "testText1"),
                new LangTextInstance(1, "testText2"),
                new LangTextInstance(2, "testText3"),
            }
        };
    }

    [System.Serializable]
    public class LangTextInstance
    {
        public int id;
        public string textValue;
        public LangTextInstance(int id, string textValue) { this.id = id; this.textValue = textValue; }
    }

}
