using UnityEngine.UI;
using UnityEngine;

public class SpawnPrimitiveOnClick : MonoBehaviour {

    public PrimitiveType spawnedPrimitiveType;
    public Transform parent;

    void Start()
    {
        Button mButton = GetComponent<Button>();
        mButton.onClick.AddListener(OnClick);
    }

	public void OnClick()
    {
        GameObject newObject = GameObject.CreatePrimitive(spawnedPrimitiveType);

        newObject.transform.SetParent(parent);
        newObject.GetComponent<Renderer>().material.color = Color.blue;

        GameObject last = GameObjectsManager.lastSelectedObject;
        if (last != null)
        {
            newObject.transform.position = last.transform.position + new Vector3(2, 0, 0);
            newObject.transform.rotation = last.transform.rotation;
        }

        GameObjectsManager.lastSelectedObject = newObject;
    }

}
