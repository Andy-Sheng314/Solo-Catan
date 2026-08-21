using UnityEngine;

[RequireComponent(typeof(Renderer))]
public abstract class Buildable : MonoBehaviour
{
    [SerializeField] protected HexPos hex_pos;
    protected bool built;

    public virtual void Init(HexPos hex_pos_in) {
        hex_pos = hex_pos_in;
        built = false;
    }

    public void Build() { built = true; }
    public bool IsBuilt() { return built; }
}
