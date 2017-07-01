using UnityEngine.UI;
using UnityEngine;
using System;

public class ObjectPropertiesEditor : MonoBehaviour {

    [Header("Position")]
    public InputField posX;
    public InputField posY;
    public InputField posZ;

    [Header("Rotation")]
    public InputField rotX;
    public InputField rotY;
    public InputField rotZ;

    [Header("Scale")]
    public InputField scaX;
    public InputField scaY;
    public InputField scaZ;

    private GameObject last;
    private GameObject current;

    void Start()
    {
        posX.onEndEdit.AddListener(delegate { ApplyValues(); });
        posY.onEndEdit.AddListener(delegate { ApplyValues(); });
        posZ.onEndEdit.AddListener(delegate { ApplyValues(); });

        rotX.onEndEdit.AddListener(delegate { ApplyValues(); });
        rotY.onEndEdit.AddListener(delegate { ApplyValues(); });
        rotZ.onEndEdit.AddListener(delegate { ApplyValues(); });

        scaX.onEndEdit.AddListener(delegate { ApplyValues(); });
        scaY.onEndEdit.AddListener(delegate { ApplyValues(); });
        scaZ.onEndEdit.AddListener(delegate { ApplyValues(); });
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

            rotX.text = current.transform.rotation.eulerAngles.x.ToString();
            rotY.text = current.transform.rotation.eulerAngles.y.ToString();
            rotZ.text = current.transform.rotation.eulerAngles.z.ToString();

            scaX.text = current.transform.localScale.x.ToString();
            scaY.text = current.transform.localScale.y.ToString();
            scaZ.text = current.transform.localScale.z.ToString();
        }
    }

    public void ApplyValues()
    {
        if (current != null)
        {
            ApplyPosition();
            ApplyRotation();
            ApplyScale();
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

    private void ApplyRotation()
    {
        float rX, rY, rZ;

        try
        {
            rX = float.Parse(rotX.text);
        }
        catch (Exception e) { rX = current.transform.rotation.eulerAngles.x; }

        try
        {
            rY = float.Parse(rotY.text);
        }
        catch (Exception e) { rY = current.transform.rotation.eulerAngles.y; }

        try
        {
            rZ = float.Parse(rotZ.text);
        }
        catch (Exception e) { rZ = current.transform.rotation.eulerAngles.z; }

        current.transform.rotation = Quaternion.Euler(rX, rY, rZ);
    }

    private void ApplyScale()
    {
        float sX, sY, sZ;

        try
        {
            sX = float.Parse(scaX.text);
        }
        catch (Exception e) { sX = current.transform.localScale.x; }

        try
        {
            sY = float.Parse(scaY.text);
        }
        catch (Exception e) { sY = current.transform.localScale.y; }

        try
        {
            sZ = float.Parse(scaZ.text);
        }
        catch (Exception e) { sZ = current.transform.localScale.z; }

        current.transform.localScale = new Vector3(sX, sY, sZ);
    }

}
