#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class PuppetHeroInstructionsBuilder
{
    [MenuItem("Tools/Puppet Hero/Generate Instructions Panel")]
    public static void Generate()
    {
        GameObject? panel = FindInScene("InstructionsPanel");
        if (panel == null || panel.GetComponent<RectTransform>() == null)
        {
            EditorUtility.DisplayDialog("Puppet Hero", "Create a UI Panel named InstructionsPanel under Canvas first.", "OK");
            return;
        }

        TMP_FontAsset? titleFont = FindInScene("Title")?.GetComponent<TMP_Text>().font;
        if (titleFont == null)
        {
            Debug.LogError("Cannot find Title GameObject in scene");
            return;
        }

        TMP_FontAsset? bodyFont = FindInScene("Subtitle")?.GetComponent<TMP_Text>().font;
        if (bodyFont == null) bodyFont = titleFont;

        Undo.RecordObject(panel, "Prepare instructions panel");
        RectTransform rect = panel.GetComponent<RectTransform>();
        Undo.RecordObject(rect, "Stretch instructions panel");
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        Image backdrop = panel.GetComponent<Image>();
        if (backdrop == null) backdrop = Undo.AddComponent<Image>(panel);
        Undo.RecordObject(backdrop, "Set instructions background");
        backdrop.color = new Color32(25, 19, 23, 242);
        backdrop.raycastTarget = true;
        panel.transform.SetAsLastSibling();

        Transform old = panel.transform.Find("GeneratedInstructions");
        if (old != null) Undo.DestroyObjectImmediate(old.gameObject);
        GameObject root = Create("GeneratedInstructions", panel.transform);
        RectTransform content = root.GetComponent<RectTransform>();
        content.anchorMin = content.anchorMax = new Vector2(.5f, .5f);
        content.pivot = new Vector2(.5f, .5f);
        content.anchoredPosition = Vector2.zero;
        content.sizeDelta = new Vector2(1200, 900);

        Color gold = Hex("#F0D39B");
        Color soft = Hex("#E8D6BC");
        Color mutedGold = Hex("#927147");
        Text("Heading", root.transform, "COMMENT JOUER", 64, gold, titleFont, 0, 320, 1050, 100, FontStyles.Normal);
        Image line = ImageObject("GoldDivider", root.transform, mutedGold, 0, 238, 740, 3);
        line.raycastTarget = false;
        Text("KeysHeading", root.transform, "5 TOUCHES. 5 FILS.", 43, gold, bodyFont, 0, 180, 1000, 65, FontStyles.Bold);

        string[] keys = { "A", "S", "D", "F", "G" };
        float[] xs = { -264, -132, 0, 132, 264 };
        for (int i = 0; i < keys.Length; i++)
        {
            GameObject key = Create("Key_" + keys[i], root.transform);
            Place(key.GetComponent<RectTransform>(), xs[i], 93, 104, 90);
            Image keyImage = key.AddComponent<Image>();
            keyImage.color = Hex("#862B3C");
            Outline outline = key.AddComponent<Outline>();
            outline.effectColor = Hex("#D9A65C");
            outline.effectDistance = new Vector2(2, -2);
            outline.useGraphicAlpha = true;
            Text("Letter", key.transform, keys[i], 47, gold, bodyFont, 0, 0, 104, 90, FontStyles.Bold);
        }

        Text("HowTo", root.transform,
            "Appuie sur A, S, D, F et G au rythme de la musique\npour faire danser ta marionnette.",
            32, soft, bodyFont, 0, -22, 1100, 100, FontStyles.Normal);
        Text("Warning", root.transform,
            "Chaque erreur te rapproche de la rupture d'un fil.\nAprès 4 fils perdus, le personnage finit pendu.",
            30, soft, bodyFont, 0, -143, 1100, 100, FontStyles.Normal);
        Text("Tagline", root.transform, "Garde le rythme. Ne perds pas le fil.",
            30, gold, bodyFont, 0, -241, 1050, 55, FontStyles.Italic);

        GameObject back = Create("BackButton", root.transform);
        Place(back.GetComponent<RectTransform>(), 0, -339, 370, 78);
        Image backImage = back.AddComponent<Image>();
        backImage.color = Hex("#292025");
        Outline backOutline = back.AddComponent<Outline>();
        backOutline.effectColor = Hex("#D9A65C");
        backOutline.effectDistance = new Vector2(2, -2);
        backOutline.useGraphicAlpha = true;
        Button button = back.AddComponent<Button>();
        button.targetGraphic = backImage;
        ColorBlock cb = button.colors;
        cb.normalColor = Color.white;
        cb.highlightedColor = Hex("#D9A65C");
        cb.pressedColor = Hex("#B88A48");
        cb.selectedColor = Hex("#D9A65C");
        cb.fadeDuration = .12f;
        button.colors = cb;
        Text("Label", back.transform, "RETOUR", 34, gold, bodyFont, 0, 0, 370, 78, FontStyles.Bold);
        // The button action is intentionally left empty for the menu-wiring step.
        panel.SetActive(true);
        EditorUtility.SetDirty(panel);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Selection.activeGameObject = panel;
        Debug.Log("Puppet Hero: Instructions panel generated. BackButton On Click is not wired yet.");
    }

    static GameObject? FindInScene(string objectName)
    {
        foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
                if (t.name == objectName) return t.gameObject;
        }
        return null;
    }

    static GameObject Create(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        return go;
    }

    static void Place(RectTransform rect, float x, float y, float w, float h)
    {
        rect.anchorMin = rect.anchorMax = new Vector2(.5f, .5f);
        rect.pivot = new Vector2(.5f, .5f);
        rect.anchoredPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(w, h);
        rect.localScale = Vector3.one;
    }

    static Image ImageObject(string name, Transform parent, Color color, float x, float y, float w, float h)
    {
        GameObject go = Create(name, parent);
        Place(go.GetComponent<RectTransform>(), x, y, w, h);
        Image image = go.AddComponent<Image>();
        image.color = color;
        return image;
    }

    static TMP_Text Text(string name, Transform parent, string value, float size, Color color,
        TMP_FontAsset font, float x, float y, float w, float h, FontStyles style)
    {
        GameObject go = Create(name, parent);
        Place(go.GetComponent<RectTransform>(), x, y, w, h);
        TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
        if (font != null) text.font = font;
        text.text = value;
        text.fontSize = size;
        text.color = color;
        text.fontStyle = style;
        text.alignment = TextAlignmentOptions.Center;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.overflowMode = TextOverflowModes.Overflow;
        text.raycastTarget = false;
        return text;
    }

    static Color Hex(string value)
    {
        ColorUtility.TryParseHtmlString(value, out Color color);
        return color;
    }
}
#endif
