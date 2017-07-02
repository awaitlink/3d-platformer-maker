using UnityEngine.UI;
using UnityEngine;
using System;

public class ObjectPropertiesEditor : MonoBehaviour {

    public Text notificationText;

    [Header("Position")]
    public InputField[] pos;
    [Header("Rotation")]
    public InputField[] rot;
    [Header("Scale")]
    public InputField[] sca;

    private InputField[][] inputs;

    private GameObject last;
    private GameObject current;

    void Start()
    {
        inputs = new InputField[][] { pos, rot, sca };

        foreach (InputField[] inputArray in inputs)
        {
            foreach (InputField input in inputArray)
            {
                input.onEndEdit.AddListener(delegate { ApplyValues(); });
            }
        }
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

            for (int i = 0; i < inputs.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector3 param = Vector3.zero;
                    switch (i)
                    {
                        case 0: param = current.transform.position; break;
                        case 1: param = current.transform.rotation.eulerAngles; break;
                        case 2: param = current.transform.localScale; break;
                    }
                    inputs[i][j].text = param[j].ToString();
                }
            }
        }
    }

    public void ApplyValues()
    {
        if (current != null)
        {
            notificationText.text = "3D GAME MAKER";
            //TODO: Optimize methods:
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
            pX = float.Parse(inputs[0][0].text);
        }
        catch (Exception e) { pX = current.transform.position.x; }

        try
        {
            pY = float.Parse(inputs[0][1].text);
        }
        catch (Exception e) { pY = current.transform.position.y; }

        try
        {
            pZ = float.Parse(inputs[0][2].text);
        }
        catch (Exception e) { pZ = current.transform.position.z; }

        current.transform.position = new Vector3(pX, pY, pZ);
    }

    private void ApplyRotation()
    {
        float rX, rY, rZ;

        try
        {
            rX = float.Parse(inputs[1][0].text);
        }
        catch (Exception e) { rX = current.transform.rotation.eulerAngles.x; }

        try
        {
            rY = float.Parse(inputs[1][1].text);
        }
        catch (Exception e) { rY = current.transform.rotation.eulerAngles.y; }

        try
        {
            rZ = float.Parse(inputs[1][2].text);
        }
        catch (Exception e) { rZ = current.transform.rotation.eulerAngles.z; }

        current.transform.rotation = Quaternion.Euler(rX, rY, rZ);
    }

    private void ApplyScale()
    {
        float sX, sY, sZ;

        try
        {
            sX = float.Parse(inputs[2][0].text);
        }
        catch (Exception e) { sX = current.transform.localScale.x; }

        try
        {
            sY = float.Parse(inputs[2][1].text);
        }
        catch (Exception e) { sY = current.transform.localScale.y; }

        try
        {
            sZ = float.Parse(inputs[2][2].text);
        }
        catch (Exception e) { sZ = current.transform.localScale.z; }

        current.transform.localScale = new Vector3(sX, sY, sZ);
    }

    public void SetColor(Image color)
    {
        current.GetComponent<Renderer>().material.color = color.color;
    }

}
