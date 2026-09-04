using UnityEngine;

public enum Axis { q = 0, r, s };

[RequireComponent(typeof(Renderer))]
public class Road : Buildable
{
    [SerializeField] private Axis axis;

    public override void Init(HexPos hex_pos_in)
    {
        base.Init(hex_pos_in);
        if (hex_pos.q % 6 == 0)
            axis = Axis.q;
        else if (hex_pos.r % 6 == 0)
            axis = Axis.r;
        else
            axis = Axis.s;
        Debug.Log(axis);
    }

    public Axis GetAxis() { return axis; }
}
