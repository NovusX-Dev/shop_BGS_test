using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopKeeper : MonoBehaviour
{
    [SerializeField] private GameObject pressEObject;

    private bool _buttonPressed = false;

    public void ActivateButton(bool active)
    {
        pressEObject.SetActive(active);
    }

    public bool ButtonPressed(bool pressed)
    {
       return _buttonPressed = pressed;
    }
}
