using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TabInputField : MonoBehaviour
{

    [SerializeField] private List<TMP_InputField> fields;
    private int InputSelected;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            InputSelected--;
            if (InputSelected < 0)
            {
                InputSelected = fields.Count;
            }
            SelectInputField();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InputSelected++;
            if (InputSelected > fields.Count -1)
            {
                InputSelected = 0;
            }
            SelectInputField();
        }
    }

    public void SelectInputField()
    {
        fields[InputSelected].Select();
    }
}
