using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Renderer))]
public abstract class Buildable : MonoBehaviour
{
    private InputAction build_action;
    protected HexPos hex_pos;

    void Start()
    {
        build_action = InputSystem.actions.FindAction("ToggleBuild");
    }

    // Update is called once per frame
    void Update()
    {
        build_action.performed += (context) =>
        {
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        };
    }

    public virtual void Init(HexPos hex_pos_in) {
        hex_pos = hex_pos_in;
    }
}
