using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Globalization;

using UnityEngine.EventSystems;

namespace Bytes
{
    public class IntEvent : UnityEvent<int> { }
    public class BoolEvent : UnityEvent<bool> { }
    public class StringEvent : UnityEvent<string> { }
    public class FloatEvent : UnityEvent<float> { }

    [System.Serializable]
    [SerializeField]
    public class TransformEvent : UnityEvent<Transform> { }

    public class ObjectArrEvent : UnityEvent<object[]> { }

    public class Utils
    {
        public enum FilterType { CONTAINS, DOESNT_CONTAIN, EQUALS, IS_NOT }

        static public T[] AddComponentOnAllChilds<T>(Transform pTransform, string pFilter = "__NO_FILTER", FilterType pFilterType = FilterType.CONTAINS) where T : Component
        {
            Transform[] lChilds = GetAllChilds(pTransform, pFilter, pFilterType);
            T[] lReturnArray = new T[lChilds.Length];
            for (int lX = 0; lX < lChilds.Length; lX++)
            {
                T lComponent = lChilds[lX].gameObject.AddComponent<T>();
                lReturnArray[lX] = lComponent;
            }
            return lReturnArray;
        }

        // Add component of type 'T' on array of transforms
        // 'pTransforms' is of type 'R' so that we can also pass any of our custom scripts that are MonoBehaviour
        static public T[] AddComponentOnMonoBehaviourArray<T, R>(R[] pTransforms) where T : Component where R : MonoBehaviour
        {
            T[] lReturnArray = new T[pTransforms.Length];
            for (int lX = 0; lX < pTransforms.Length; lX++)
            {
                T lComponent = pTransforms[lX].gameObject.AddComponent<T>();
                lReturnArray[lX] = lComponent;
            }
            return lReturnArray;
        }

        static public T[] GetComponentsInChilds<T>(Transform pTransform, string pFilter = "__NO_FILTER", FilterType pFilterType = FilterType.CONTAINS)
        {
            Transform[] lChilds = GetAllChilds(pTransform, pFilter, pFilterType);
            List<T> lChildsComponents = new List<T>();
            for (int lX = 0; lX < lChilds.Length; lX++)
            {
                T lComponent = lChilds[lX].GetComponent<T>();
                if (lComponent != null) { lChildsComponents.Add(lComponent); }
            }
            return lChildsComponents.ToArray();
        }

        static public Transform[] GetAllChilds(Transform pTransform, string pFilter = "__NO_FILTER", FilterType pFilterType = FilterType.CONTAINS)
        {
            List<Transform> lChilds = new List<Transform>();
            for (int lX = 0; lX < pTransform.childCount; lX++)
            {
                if (pFilter == "__NO_FILTER")
                {
                    lChilds.Add(pTransform.GetChild(lX));
                }
                else
                {
                    if (StringFilter(pTransform.GetChild(lX).name, pFilter, pFilterType)) { lChilds.Add(pTransform.GetChild(lX)); }
                }
            }
            return lChilds.ToArray();
        }

        static public Canvas CreateCanvas(Transform parent, String canvasName = "UtilsCreatedCanvas")
        {
            var g = new GameObject(canvasName);
            Canvas createdCanvas = g.AddComponent<Canvas>();
            createdCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var cavansScaler = g.AddComponent<CanvasScaler>();
            cavansScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cavansScaler.referenceResolution = new Vector2(1920, 1080);

            if (GameObject.FindObjectOfType<EventSystem>() == null) { g.AddComponent<EventSystem>(); }
            if (GameObject.FindObjectOfType<StandaloneInputModule>() == null) { g.AddComponent<StandaloneInputModule>(); }

            g.transform.SetParent(parent);

            return createdCanvas;
        }

        static public GameObject Clone(GameObject pOriginal, Vector3 pPosition) { return Clone(pOriginal, pPosition, pOriginal.transform.localScale, Quaternion.identity.eulerAngles); }
        static public GameObject Clone(GameObject pOriginal, Vector3 pPosition, Vector3 pScale, Vector3 pRotation)
        {
            var lNewGameObject = GameObject.Instantiate(pOriginal, pPosition, Quaternion.Euler(pRotation));
            lNewGameObject.transform.SetParent(pOriginal.transform.parent);
            lNewGameObject.transform.localScale = pScale;
            lNewGameObject.name = pOriginal.name + " - clone";
            return lNewGameObject;
        }

        static public GameObject CreatePrefabFromResources(string path, Vector3 pPosition)
        {
            return CreatePrefabFromResources(path, pPosition, null, Vector3.one, Vector3.zero);
        }
        static public GameObject CreatePrefabFromResources(string path, Vector3 pPosition, Transform pParent)
        {
            return CreatePrefabFromResources(path, pPosition, pParent, Vector3.one, Vector3.zero);
        }
        static public GameObject CreatePrefabFromResources(string path, Vector3 pPosition, Transform pParent, Vector3 pScale, Vector3 pRotation)
        {
            var lNewGameObject = GameObject.Instantiate(Resources.Load<GameObject>(path), pPosition, Quaternion.Euler(pRotation));
            if (pParent != null) { lNewGameObject.transform.SetParent(pParent); }
            lNewGameObject.transform.localScale = pScale;
            return lNewGameObject;
        }

        static public bool StringFilter(string pString, string pFilter = "__NO_FILTER", FilterType pFilterType = FilterType.CONTAINS)
        {
            if (pFilter == "__NO_FILTER") { return true; }
            bool lPassTroughtFilter = false;
            switch (pFilterType)
            {
                case FilterType.CONTAINS:
                    lPassTroughtFilter = pString.Contains(pFilter); break;
                case FilterType.DOESNT_CONTAIN:
                    lPassTroughtFilter = !pString.Contains(pFilter); break;
                case FilterType.EQUALS:
                    lPassTroughtFilter = pString.Equals(pFilter); break;
                case FilterType.IS_NOT:
                    lPassTroughtFilter = !pString.Equals(pFilter); break;
            }
            return lPassTroughtFilter;
        }

        // Convert X value to a mapped value between C coords and R coords
        // Ex : From -c_1 to c_2  =>  X : To -r_1 to r_2
        //    : Returns X with equivalent percentage with the second pair of limits
        static public float Map(float pX, float pC_1, float pC_2, float pR_1, float pR_2)
        {
            float lPercentage = (((pX - pC_1)) / (pC_2 - pC_1)) * 100;
            return ((lPercentage) * (pR_2 / 50f)) + pR_1 - (Math.Abs((Math.Abs(pR_2) - Math.Abs(pR_1))) * (lPercentage / 100));
        }

        // Same as above, but always between 0f and 1f
        static public float Map01(float pX, float pC_1, float pC_2)
        {
            float lPercentage = (((pX - pC_1)) / (pC_2 - pC_1)) * 100;
            return ((lPercentage) * (1f / 50f)) + 0 - (Math.Abs((Math.Abs(1f) - Math.Abs(0f))) * (lPercentage / 100));
        }

        static public bool IsBetween(float pX, float pMin, float pMax)
        {
            return (pX >= pMin && pX <= pMax);
        }

        // Invert two variables of type 'T'
        static public void InvertValues<T>(ref T val1, ref T val2)
        {
            T lTempBuffer = val1;
            val1 = val2;
            val2 = lTempBuffer;
        }

        // Return pX in negative or positive form with  'pC'% chances of being positive
        static public float PositiveOrNegative(float pX, float pC = 50)
        {
            return (UnityEngine.Random.Range(0f, 1f) > pC / 100f) ? pX * -1 : pX;
        }

        // Unity donne des angles négatifs difficiles d'utilisation, donc ce code retourne l'angle entre 0 et 360 degreer
        public static float AngleConversion(float angle)
        {
            return (angle > 180) ? angle - 360 : angle;
        }

        // Used to be able to set a callback at the end of animation played by target animator
        static public Animate PlayAnimatorClip(Animator animator, string clipName, System.Action callback)
        {
            //animator.updateMode = AnimatorUpdateMode.Normal;
            //if (pRealTimeScale) { animator.updateMode = AnimatorUpdateMode.UnscaledTime; }
            float lDuration = GetAnimatorLayerDuration(animator, clipName);
            animator.Play(clipName, -1, 0);
            return Animate.Delay(lDuration, callback);
        }

        // Return animator clip (also known as a layer) duration by name (if it exists) 
        static public float GetAnimatorLayerDuration(Animator pAnimator, string pClipName)
        {
            AnimationClip[] clips = pAnimator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (pClipName == clip.name) { return clip.length; }
            }
            return -1;
        }

        static public float GetOpacity<T>(Transform pComponent) where T : Component
        {
            var lImage = pComponent.transform.GetComponent<Image>();
            var lSpriteRenderer = pComponent.transform.GetComponent<SpriteRenderer>();
            if (lImage != null) { return GetOpacity(lImage); }
            else if (lSpriteRenderer != null) { return GetOpacity(lSpriteRenderer); }
            else { Debug.LogError("There is no component that can contain an opacity on this gameObject: " + pComponent.name); return -1f; }
        }
        static public float GetOpacity(SpriteRenderer pSpRenderer)
        {
            return pSpRenderer.color.a;
        }
        static public float GetOpacity(Image pImage)
        {
            return pImage.color.a;
        }
        static public float GetOpacity(Text pText)
        {
            return pText.color.a;
        }

        // Don't call this version in update, because it will call GetComponent each frames
        static public void SetOpacity<T>(T pComponent, float pAlpha) where T : Component
        {
            CanvasGroup lCanvasGroup = pComponent.transform.GetComponent<CanvasGroup>();
            var lImage = pComponent.transform.GetComponent<Image>();
            var lSpriteRenderer = pComponent.transform.GetComponent<SpriteRenderer>();
            var lText = pComponent.transform.GetComponent<Text>();

            if (lCanvasGroup != null) { SetOpacity(lCanvasGroup, pAlpha); }
            else if (lImage != null) { SetOpacity(lImage, pAlpha); }
            else if (lSpriteRenderer != null) { SetOpacity(lSpriteRenderer, pAlpha); }
            else if (lText != null) { SetOpacity(lText, pAlpha); }
            else { Debug.LogWarning("There is no Image or SpriteRenderer componenent attached to this GameObject! But you are still trying to access those to change their opacity."); }
        }
        static public void SetOpacity(CanvasGroup pCanvasGroup, float pAlpha)
        {
            pCanvasGroup.alpha = pAlpha;
        }
        static public void SetOpacity(SpriteRenderer pSpRenderer, float pAlpha)
        {
            var lColor = pSpRenderer.color; lColor.a = pAlpha; pSpRenderer.color = lColor;
        }
        static public void SetOpacity(Image pImage, float pAlpha)
        {
            var lColor = pImage.color; lColor.a = pAlpha; pImage.color = lColor;
        }
        static public void SetOpacity(Text pText, float pAlpha)
        {
            var lColor = pText.color; lColor.a = pAlpha; pText.color = lColor;
        }

        // Removes spaces and make name be camelCased
        static public string CamelCase(string s)
        {
            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
            return txtInfo.ToTitleCase(s).Replace(' ', ' ').Replace(" ", System.String.Empty);
        }

        static public Quaternion GetRotationTowardDirection(Vector2 dir)
        {
            return 
                Quaternion.AngleAxis(
                  // Angle processing
                  Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg
                , Vector3.forward);
        }

        static public Vector3 GetMousePos()
        {
            if (Camera.main?.enabled == false || Camera.main == null) { return Vector3.one; }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            return mousePos;
        }

        static public Vector3 GetDirFromMouse(Transform origin)
        {
            Vector3 dir = GetMousePos() - origin.transform.position;
            dir.Normalize();
            return dir;
        }

    }
}