using UnityEngine.UI;
using UnityEngine;
using System;

public class SpawnPrimitiveOnClick : MonoBehaviour {

    [Header("UI Offset Inputs")]
    public InputField X;
    public InputField Y;
    public InputField Z;

    [Header("Other")]
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
            float x = 1, y = 0, z = 0;

            try { x = float.Parse(X.text); } catch (Exception e) { }
            try { y = float.Parse(Y.text); } catch (Exception e) { }
            try { z = float.Parse(Z.text); } catch (Exception e) { }

            newObject.transform.position = last.transform.position + new Vector3(x, y, z);
            newObject.transform.rotation = last.transform.rotation;
            newObject.transform.localScale = last.transform.localScale;
            newObject.GetComponent<Renderer>().material.color = last.GetComponent<Renderer>().material.color;

            newObject.AddComponent<Spawner>();
            newObject.GetComponent<Spawner>().type = spawnedPrimitiveType;
        }

        GameObjectsManager.lastSelectedObject = newObject;
        objectNameText.text = newObject.name;
    }

}
