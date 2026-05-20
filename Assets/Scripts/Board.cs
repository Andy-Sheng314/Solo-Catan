using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Water, Wheat, Stone, Brick };
public class HexPos { 
    public int q, r; 
    public HexPos(int q_in, int r_in)
    {
        q = q_in;
        r = r_in;
    }
};

[RequireComponent(typeof(Transform))]
public class Board : MonoBehaviour
{       
    private Dictionary<TileType, int> inventory = new();
    private Dictionary<HexPos, Hex> board = new();

    [SerializeField] private GameObject hex_tile;
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject building;

    public int R = 2;
    public float scale = 1.0f; // size of tiles
    public float padding = 0.1f; // space between tiles

    void Awake()
    {   
        // intialize each of the inventory items
        foreach (int type in Enum.GetValues(typeof(TileType))) 
        {
            inventory.Add((TileType) type, 0);
        }
        
        // initialize game pieces
        InstantiateHexes();
        InstantiateRoads();
        InstantiateBuildings();
    }

    void InstantiateHexes()
    {
        for (int q = -R; q <= R; ++q)
        {
            for (int r = -R; r <= R; ++r)
            {
                int s = Math.Abs(-q-r);
                if (s <= R)
                {
                    HexPos hex_pos = new(6*q, 6*r);
                    Vector3 pixel_pos = CalcPixelPos(hex_pos);

                    GameObject instance = Instantiate(hex_tile, 
                                          pixel_pos, 
                                          Quaternion.identity,
                                          GetComponent<Transform>());
                    Hex hex = instance.GetComponent<Hex>();
                    board.Add(hex_pos, hex);
                    
                    // just for fun
                    // System.Random rand = new System.Random();
                    // Color rand_colour = new Color((float)rand.NextDouble(), 
                    //                              (float)rand.NextDouble(),
                    //                              (float)rand.NextDouble());

                    // check if border
                    if (Math.Abs(q) < R && Math.Abs(r) < R && Math.Abs(s) < R)
                        hex.Init(hex_pos, TileType.Wheat, 
                        Color.white, scale);
                    else
                        hex.Init(hex_pos, TileType.Water, 
                        new Color(0.54f, 0.65f, 0.66f), scale);
                }
            }
        }
    }

    void InstantiateRoads()
    {
        for (int q = 1-2*R; q < 2*R; ++q)
        {
            for (int r = 1-2*R; r < 2*R; ++r)
            {
                int s = -q-r;
                if (Math.Abs(s) < 2*R 
                    && (q % 2 != 0 || r % 2 != 0 || s % 2 != 0))
                {
                    HexPos hex_pos = new(3*q, 3*r);
                    Vector3 pixel_pos = CalcPixelPos(hex_pos);

                    Instantiate(road, pixel_pos, Quaternion.identity, 
                                GetComponent<Transform>());
                }
            }
        }
    }

    void InstantiateBuildings()
    {
        for (int q = 1-3*R; q < 3*R; ++q)
        {
            for (int r = 1-3*R; r < 3*R; ++r)
            {
                int s = -q-r;
                if (Math.Abs(s) < 3*R && q*r*s % 3 != 0)
                {
                    HexPos hex_pos = new(2*q, 2*r);
                    Vector3 pixel_pos = CalcPixelPos(hex_pos);

                    Instantiate(building, pixel_pos, Quaternion.identity, 
                                GetComponent<Transform>());
                }
            }
        }
    }

    Vector3 CalcPixelPos(HexPos hex_pos)
    {
        float x = (scale + padding) * (float)(Math.Sqrt(3) * hex_pos.q 
                    + Math.Sqrt(3) / 2 * hex_pos.r) / 1.5f / 6f;
        float y = (scale + padding) * hex_pos.r / 6f;
        return new Vector3(x, y, 0);
    }

    void Update()
    {
        
    }
}