using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class EndGameBuilder
{
    private static readonly Color Gold = Html("#F0D39B");
    private static readonly Color Burgundy = Html("#862B3C");
    private static readonly Color Secondary = Html("#292025");

    [MenuItem("Tools/Puppet Hero/Generate End Screen")]
    private static void Generate()
    {
        GameObject canvas = GameObject.Find("EndGameCanvas");
        if (canvas == null) { Debug.LogError("EndGameCanvas introuvable dans la scène ouverte."); return; }
        Transform panel = canvas.transform.Find("EndGamePanel");
        if (panel == null) { Debug.LogError("EndGamePanel doit être enfant de EndGameCanvas."); return; }

        Transform previous = panel.Find("GeneratedEndScreen");
        if (previous != null) Undo.DestroyObjectImmediate(previous.gameObject);

        GameObject root = UI("GeneratedEndScreen", panel);
        Stretch((RectTransform)root.transform);
        TMP_FontAsset font = FindTitleFont();

        Text("Heading", root.transform, "AU BOUT DU FIL", font, 90, Gold, 0, 220, 1400, 140);
        Text("Message", root.transform, "Les quatre fils ont cédé. Le rideau tombe.",
            TMP_Settings.defaultFontAsset != null ? TMP_Settings.defaultFontAsset : font,
            38, Gold, 0, 92, 1300, 100);

        Button replay = MakeButton("ReplayButton", root.transform, "REJOUER", Burgundy, font, -55);
        Button menu = MakeButton("MainMenuButton", root.transform, "MENU PRINCIPAL", Secondary, font, -165);

        EndGameController controller = canvas.GetComponent<EndGameController>();
        if (controller == null) controller = Undo.AddComponent<EndGameController>(canvas);
        Undo.RecordObject(controller, "Configure EndGameController");
        controller.EditorAssign(panel.gameObject,
            root.transform.Find("Heading").GetComponent<TMP_Text>(),
            root.transform.Find("Message").GetComponent<TMP_Text>(), replay, menu);
        EditorUtility.SetDirty(controller);

        // Keep the new design visible in Scene/Game views while editing.
        // EndGameController.Awake hides the panel when Play starts.
        panel.gameObject.SetActive(true);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(canvas.scene);
        Selection.activeGameObject = canvas;
        Debug.Log("Puppet Hero: End screen generated. Save MainLevel (Ctrl+S). During Play, use EndGameController's context menu to test Victory/Defeat.");
    }

    private static TMP_FontAsset FindTitleFont()
    {
        string[] ids = AssetDatabase.FindAssets("CinzelDecorative t:TMP_FontAsset");
        if (ids.Length > 0)
        {
            var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(AssetDatabase.GUIDToAssetPath(ids[0]));
            if (font != null) return font;
        }
        return TMP_Settings.defaultFontAsset;
    }

    private static GameObject UI(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(go, "Generate End Screen");
        go.transform.SetParent(parent, false);
        return go;
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private static RectTransform Rect(GameObject go, float x, float y, float w, float h)
    {
        var rt = (RectTransform)go.transform;
        rt.anchorMin = rt.anchorMax = new Vector2(.5f, .5f);
        rt.pivot = new Vector2(.5f, .5f);
        rt.anchoredPosition = new Vector2(x, y);
        rt.sizeDelta = new Vector2(w, h);
        return rt;
    }

    private static TMP_Text Text(string name, Transform parent, string content, TMP_FontAsset font,
        float fontSize, Color color, float x, float y, float w, float h)
    {
        var go = UI(name, parent);
        Rect(go, x, y, w, h);
        var txt = Undo.AddComponent<TextMeshProUGUI>(go);
        txt.text = content;
        if (font != null) txt.font = font;
        txt.fontSize = fontSize;
        txt.color = color;
        txt.alignment = TextAlignmentOptions.Center;
        txt.textWrappingMode = TextWrappingModes.NoWrap;
        //txt.enableWordWrapping = false;
        txt.raycastTarget = false;
        return txt;
    }

    private static Button MakeButton(string name, Transform parent, string label, Color baseColor,
        TMP_FontAsset font, float y)
    {
        var go = UI(name, parent);
        Rect(go, 0, y, 520, 86);
        var image = Undo.AddComponent<Image>(go);
        image.color = baseColor;
        var outline = Undo.AddComponent<Outline>(go);
        outline.effectColor = Html("#D9A65C");
        outline.effectDistance = new Vector2(2, -2);
        var btn = Undo.AddComponent<Button>(go);
        btn.targetGraphic = image;
        btn.transition = Selectable.Transition.ColorTint;
        var cb = btn.colors;
        cb.normalColor = Color.white;
        cb.highlightedColor = Html("#D9A65C");
        cb.pressedColor = Html("#B88A48");
        cb.selectedColor = Color.white;
        cb.fadeDuration = .12f;
        btn.colors = cb;
        Text("Label", go.transform, label, font, 35, Gold, 0, 0, 500, 75);
        return btn;
    }

    private static Color Html(string value)
    {
        ColorUtility.TryParseHtmlString(value, out Color color);
        return color;
    }
}
