using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public Dictionary<Actions, KeyCode> controlScheme = new Dictionary<Actions, KeyCode>();
    public Dictionary<Actions, KeyCode> controlScheme2 = new Dictionary<Actions, KeyCode>();

    private void Awake()
    {
        if (instance == null)
        {
            INIT();
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void INIT()
    {
        controlScheme.Add(Actions.Up, KeyCode.UpArrow);
        controlScheme2.Add(Actions.Up, KeyCode.W);
        controlScheme.Add(Actions.Down, KeyCode.DownArrow);
        controlScheme2.Add(Actions.Down, KeyCode.S);
        controlScheme.Add(Actions.Left, KeyCode.LeftArrow);
        controlScheme2.Add(Actions.Left, KeyCode.A);
        controlScheme.Add(Actions.Right, KeyCode.RightArrow);
        controlScheme2.Add(Actions.Right, KeyCode.D);
        controlScheme.Add(Actions.Jump, KeyCode.Z);
        controlScheme2.Add(Actions.Jump, KeyCode.Space);
        controlScheme.Add(Actions.Back, KeyCode.X);
        controlScheme2.Add(Actions.Back, KeyCode.LeftShift);
        controlScheme.Add(Actions.Pause, KeyCode.Escape);
        controlScheme2.Add(Actions.Pause, KeyCode.P);
        controlScheme.Add(Actions.Inventory, KeyCode.Return);
        controlScheme2.Add(Actions.Inventory, KeyCode.E);
    }

    void Start()
    {

    }

    public bool GetButtonDown(Actions action)
    {
        return Input.GetKeyDown(controlScheme[action]) || Input.GetKeyDown(controlScheme2[action]);
    }
    public bool GetButton(Actions action)
    {
        return Input.GetKey(controlScheme[action]) || Input.GetKey(controlScheme2[action]);
    }
    public bool GetButtonUp(Actions action)
    {
        return Input.GetKeyUp(controlScheme[action]) || Input.GetKeyUp(controlScheme2[action]);
    }

}
public enum Actions
{
    Up,
    Down,
    Left,
    Right,
    Jump,
    Back,
    Pause,
    Inventory,
}
