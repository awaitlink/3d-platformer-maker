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
        newObject.transform.position = Vector3.zero;
        newObject.transform.rotation = Quaternion.identity;
        newObject.GetComponent<Renderer>().material.color = Color.blue;

        GameObjectsManager.lastSelectedObject = newObject;
    }

}
