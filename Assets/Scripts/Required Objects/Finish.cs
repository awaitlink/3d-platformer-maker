using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Equals("Player(Clone)") || col.gameObject.name.Equals("Player[Top]"))
        {
            NotificationSystem.instance.ShowNotification("Level Completed!");
            State.instance.ChangeGameState();
        }
    }

}
