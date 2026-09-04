using UnityEngine;

public enum Spin { up = 0, down };
[RequireComponent(typeof(Renderer))]
public class Building : Buildable
{
    [SerializeField] private Spin spin;

    public override void Init(HexPos hex_pos_in)
    {
        base.Init(hex_pos_in);
        spin = ((hex_pos.q - 2) % 6 == 0) ? Spin.up : Spin.down;
    }

    public Spin GetSpin()
    {
        return spin;
    }
}
