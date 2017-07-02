using System.Collections;
using UnityEngine;

public class State : MonoBehaviour {

    [Header("Player")]
    public GameObject player;

    [Header("UI Stuff")]
    public GameObject[] uiStuffToHideWhenPlaying;

    private GameObject playerClone = null;

    [HideInInspector]
    public static bool isPlaying = false;

    void Start()
    {
        GameObjectsManager.lastSelectedObject = player;
    }

    public void ChangeGameState()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            player.SetActive(false);

            playerClone = Instantiate(player, player.transform.position, player.transform.rotation, player.transform.parent);
            playerClone.transform.localScale = player.transform.localScale;
            Rigidbody playerCloneRB = playerClone.GetComponent<Rigidbody>();
            playerCloneRB.isKinematic = false;
            playerCloneRB.useGravity = true;


            foreach (GameObject toHide in uiStuffToHideWhenPlaying)
            {
                StartCoroutine(MoveGO(toHide.transform, new Vector2(-2, -2), 40));
            }

            playerClone.SetActive(true);

            CameraController.target = playerClone.transform;
        }
        else
        {
            Destroy(playerClone);
            player.SetActive(true);

            GameObjectsManager.lastSelectedObject = player;

            foreach (GameObject toHide in uiStuffToHideWhenPlaying)
            {
                StartCoroutine(MoveGO(toHide.transform, new Vector2(2, 2), 40));
            }
        }
    }

    private IEnumerator MoveGO(Transform toHide, Vector2 direction, int times)
    {
        for (int i = 0; i <= times; i++)
        {
            toHide.Translate(direction);
            yield return new WaitForSeconds(.01f);
        }
    }

}
