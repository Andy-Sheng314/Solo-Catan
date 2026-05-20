using UnityEngine;
using UnityEngine.UIElements;

public class Hex : MonoBehaviour
{
    private HexPos hex_pos;
    private TileType tile_type;

    [SerializeField] private SpriteRenderer sprite_renderer;
    [SerializeField] private Transform hex_transform;

    public void Init(HexPos hex_pos_in, TileType tile_type_in, Color colour, 
                     float scale)
    {
        hex_pos = hex_pos_in;
        tile_type = tile_type_in;
        sprite_renderer.color = colour;
        hex_transform.localScale = new Vector3(scale, scale, 1);
    }
}
