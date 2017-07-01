using UnityEngine.UI;
using UnityEngine;
using System;

public class ObjectPropertiesEditor : MonoBehaviour {

    [Header("Position")]
    public InputField posX;
    public InputField posY;
    public InputField posZ;

    private GameObject last;
    private GameObject current;

    void Start()
    {
        posX.onEndEdit.AddListener(delegate { ApplyValues(); });
        posY.onEndEdit.AddListener(delegate { ApplyValues(); });
        posZ.onEndEdit.AddListener(delegate { ApplyValues(); });
    }

    void Update()
    {
        last = current;
        current = GameObjectsManager.lastSelectedObject;

        if (last != current)
        {
            GetNewObjectValues();
        }
    }

    private void GetNewObjectValues()
    {
        if (current != null)
        {
            posX.text = current.transform.position.x.ToString();
            posY.text = current.transform.position.y.ToString();
            posZ.text = current.transform.position.z.ToString();
        }
    }

    public void ApplyValues()
    {
        if (current != null)
        {
            ApplyPosition();
        }
    }

    private void ApplyPosition()
    {
        float pX, pY, pZ;

        try
        {
            pX = float.Parse(posX.text);
        }
        catch (Exception e) { pX = current.transform.position.x; }

        try
        {
            pY = float.Parse(posY.text);
        }
        catch (Exception e) { pY = current.transform.position.y; }

        try
        {
            pZ = float.Parse(posZ.text);
        }
        catch (Exception e) { pZ = current.transform.position.z; }

        current.transform.position = new Vector3(pX, pY, pZ);
    }

}
