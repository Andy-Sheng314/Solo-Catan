using UnityEngine;

public class Hex : MonoBehaviour
{
    private HexPos hex_pos;
    private TileType tile_type;

    [SerializeField]
    private SpriteRenderer sprite_renderer;

    public void Init(HexPos hex_pos_in, TileType tile_type_in, Color colour)
    {
        hex_pos = hex_pos_in;
        tile_type = tile_type_in;
        sprite_renderer.color = colour;
    }
}
