
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuSFX : MonoBehaviour
{
    public AudioClip? hoverSound;
    public AudioClip? clickSound;

    private AudioSource? source;

    void Awake()
    {
        source = GetComponent<AudioSource>();

        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null) return;

        Button[] buttons = canvas.GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            MenuHoverSFX sfx = button.GetComponent<MenuHoverSFX>();

            if (sfx == null)
                sfx = button.gameObject.AddComponent<MenuHoverSFX>();

            sfx.Setup(this);
        }
    }

    public void PlayHover()
    {
        if (hoverSound != null && source != null)
            source.PlayOneShot(hoverSound);
    }

    public void PlayClick()
    {
        if (clickSound == null) return;

        // Le son survit au changement de scène.
        GameObject audio = new GameObject("TemporaryClickSFX");
        DontDestroyOnLoad(audio);

        AudioSource temp = audio.AddComponent<AudioSource>();
        temp.spatialBlend = 0f;
        temp.PlayOneShot(clickSound);

        Destroy(audio, clickSound.length + 0.1f);
    }
}

public class MenuHoverSFX : MonoBehaviour,
    IPointerEnterHandler, IPointerDownHandler
{
    private MenuSFX? manager;

    public void Setup(MenuSFX audioManager)
    {
        manager = audioManager;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        manager?.PlayHover();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            manager?.PlayClick();
        }
    }
}
