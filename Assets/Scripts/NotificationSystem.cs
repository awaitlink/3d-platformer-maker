using UnityEngine;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour {

    [HideInInspector]
    public static NotificationSystem instance;

    [Header("UI Elements")]
    public GameObject notificationUI;
    public Text notificationText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        notificationUI.SetActive(false);
    }

    public void CloseNotification()
    {
        notificationUI.SetActive(false);
    }

    public void ShowNotification(string text)
    {
        notificationText.text = text;
        notificationUI.SetActive(true);
    }

    public void ShowAbout()
    {
        ShowNotification("It's 3 PM? No, it's time for 3D Platformer Maker! Made by AV (superuser.itch.io).");
    }

}
