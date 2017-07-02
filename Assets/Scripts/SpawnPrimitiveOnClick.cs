using UnityEngine.UI;
using UnityEngine;

public class SpawnPrimitiveOnClick : MonoBehaviour {

    public Text objectNameText;
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
            newObject.transform.position = last.transform.position + new Vector3(1, 0, 0);
            newObject.transform.rotation = last.transform.rotation;
            newObject.transform.localScale = last.transform.localScale;
            newObject.GetComponent<Renderer>().material.color = last.GetComponent<Renderer>().material.color;
        }

        GameObjectsManager.lastSelectedObject = newObject;
        objectNameText.text = newObject.name;
    }

}
