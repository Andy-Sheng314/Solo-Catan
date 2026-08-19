using UnityEngine;

public enum Spin { up, down };
[RequireComponent(typeof(Renderer))]
public class Building : Buildable
{
    [SerializeField] private Spin spin;

    public override void Init(HexPos hex_pos_in)
    {
        base.Init(hex_pos_in);
        spin = (hex_pos.q % 6 == 2) ? Spin.down : Spin.up;
    }
}
