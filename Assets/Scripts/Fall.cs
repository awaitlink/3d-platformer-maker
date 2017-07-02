using UnityEngine;

public class Fall : MonoBehaviour {

	void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Equals("Player(Clone)") || col.gameObject.name.Equals("PlayerTop"))
        {
            State.instance.ChangeGameState();
        }
    }

}
