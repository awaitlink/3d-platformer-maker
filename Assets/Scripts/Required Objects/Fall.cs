using UnityEngine;

public class Fall : MonoBehaviour {

	void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Equals("Player(Clone)") || col.gameObject.name.Equals("Player[Top]") && State.isPlaying)
        {
            State.instance.ChangeGameState();
        }
    }

}
