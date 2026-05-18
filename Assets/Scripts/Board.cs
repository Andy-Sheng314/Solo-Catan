using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum TileType { Wheat, Stone, Brick };
public class HexPos { 
    public int q, r; 
    public HexPos(int q_in, int r_in)
    {
        q = q_in;
        r = r_in;
    }
};

public class Board : MonoBehaviour
{       
    private Dictionary<TileType, int> inventory = new();
    private Dictionary<HexPos, Hex> board = new();

    [SerializeField]
    private GameObject hex_tile;

    public int R = 2;

    void Awake()
    {   
        // intialize each of the inventory items
        foreach (int type in Enum.GetValues(typeof(TileType))) 
        {
            inventory.Add((TileType) type, 0);
        }
        
        for (int q = 1-R; q < R; ++q)
        {
            for (int r = 1-R; r < R; ++r)
            {
                if (Math.Abs(-q-r) < R)
                {
                    // Debug.Log($"q: {q}, r: {r}");

                    HexPos hex_pos = new(q, r);
                    Vector3 pixel_pos = CalcPixelPos(hex_pos);

                    GameObject instance = Instantiate(hex_tile, 
                                          pixel_pos, 
                                          Quaternion.identity);
                    Hex hex = instance.GetComponent<Hex>();
                    hex.Init(hex_pos, TileType.Wheat, Color.white);
                }
            }
        }
    }

    Vector3 CalcPixelPos(HexPos hex_pos)
    {
        float x = (float)(Math.Sqrt(3)*hex_pos.q + Math.Sqrt(3)/2 * hex_pos.r);
        float y = (float)(1.5*hex_pos.r);
        return new Vector3(x, y, 0);
    }

    void Update()
    {
        
    }
}