using UnityEngine;

public class Finish : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Equals("Player(Clone)") || col.gameObject.name.Equals("Player[Top]") && State.isPlaying)
        {
            NotificationSystem.instance.ShowNotification("Level Completed!");
            State.instance.ChangeGameState();
        }
    }

}
