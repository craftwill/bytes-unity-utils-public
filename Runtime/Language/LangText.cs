using UnityEngine;
using UnityEngine.UI;

namespace Bytes.Language
{
    public abstract class BytesLangText : MonoBehaviour
    {
        public abstract void UpdateText();
    }
    public class LangText : BytesLangText
    {
        public string textId;
        public int speficicFile = -1;
        Text textComponent;
        public override void UpdateText()
        {
            if (!LangManager.GetIsReady()) { return; }

            if (speficicFile == -1) { textComponent.text = LangManager.GetText(textId); }
            else { textComponent.text = LangManager.GetText(textId, speficicFile); }
        }
        private void Awake()
        {
            if (textComponent == null) { textComponent = GetComponent<Text>(); }
        }
        private void OnValidate()
        {
            if (textComponent == null) { textComponent = GetComponent<Text>(); }
            UpdateText();
        }
    }
}
