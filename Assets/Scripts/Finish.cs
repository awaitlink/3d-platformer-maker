using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour {

    public Text notificationText;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Equals("Player(Clone)") || col.gameObject.name.Equals("PlayerTop"))
        {
            notificationText.text = "Level Completed!";
            State.instance.ChangeGameState();
        }
    }

}
