using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Water, Wheat, Wood, Ore, Sheep, Brick };
public class HexPos : IEquatable<HexPos> { 
    public readonly int q, r; 
    public HexPos(int q_in, int r_in)
    {
        q = q_in;
        r = r_in;
    }

    public bool Equals(HexPos other)
    {
        return q == other.q && r == other.r;
    }

    public static HexPos operator +(HexPos a, HexPos b)
    {
        return new HexPos(a.q + b.q, a.r + b.r);
    }
};

[RequireComponent(typeof(Transform))]
public class Board : MonoBehaviour
{
    private readonly HexPos[][] ROAD_ADJS =
    {
        /* q */ new HexPos[] { new(-2, +1), new(+2, -1), 
                               new(-3, +3), new(-3,  0), 
                               new(+3, -3), new(+3,  0) },
        /* r */ new HexPos[] { new(+1, -2), new(-1, +2), 
                               new( 0, +3), new(+3, -3), 
                               new(-3, +3), new( 0, -3) },
        /* s */ new HexPos[] { new(+1, +1), new(-1, -1), 
                               new(-3,  0), new( 0, +3), 
                               new(+3,  0), new( 0, -3) }
    };

    private readonly Dictionary<TileType, int> inventory = new();
    private readonly Dictionary<HexPos, Hex> board = new();

    private readonly Dictionary<HexPos, Buildable> pieces = new();

    [SerializeField] private GameObject hex_obj;
    [SerializeField] private GameObject road_obj;
    [SerializeField] private GameObject building_obj;

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
        
        // initialize hexes
        InstantiateHexes();

        // build first building
        // Build(new HexPos(2, -4));
        // Build(new HexPos(-2, 4));
        Build(new HexPos(3, 3));
        Build(new HexPos(0, 3));
        Build(new HexPos(-3, -3));
        // Adj(new HexPos(6, 0));
        // Adj(new HexPos(0, -6));
        // Adj(new HexPos(-6, 0));
        // Build(new HexPos(3, -3));
        // Build(new HexPos(-3, 0));
        
        // InstantiateRoads();
        // InstantiateBuildings();
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
                    
                    GameObject instance = Instantiate(hex_obj, 
                                          pixel_pos, 
                                          Quaternion.identity,
                                          GetComponent<Transform>());
                    Hex hex = instance.GetComponent<Hex>();
                    board.Add(hex_pos, hex);
                    
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

    void Adj(HexPos hex_pos)
    {
        Buildable new_piece;
        Vector3 pixel_pos = CalcPixelPos(hex_pos);

        // determine object type
        if (hex_pos.q % 3 == 0)
        {
            // road
            new_piece = Instantiate(road_obj, 
                                    pixel_pos, 
                                    Quaternion.identity,
                                    GetComponent<Transform>())
                                    .GetComponent<Road>();
        } 
        else
        {
            // building
            new_piece = Instantiate(building_obj, 
                                    pixel_pos, 
                                    Quaternion.identity,
                                    GetComponent<Transform>())
                                    .GetComponent<Building>();
        }
        pieces.Add(hex_pos, new_piece);
        new_piece.Init(hex_pos);
    }

    // Requires: valid hex position without existing piece
    // Modifies: built, adjs
    // Effects: adds piece to built, removes from adjs, 
    //          updates adjs with new targets
    void Build(HexPos hex_pos)
    {
        // if piece does not current exist, add it
        if (!pieces.ContainsKey(hex_pos)) Adj(hex_pos);

        Buildable piece = pieces[hex_pos];
        if (!piece.IsBuilt())
        {
            // toggle state
            piece.Build();

            // update adjacent
            if (piece is Road road)
            {
                foreach (HexPos delta in ROAD_ADJS[(int) road.getAxis()])
                {
                    HexPos new_hex_pos = hex_pos + delta;
                    if (!pieces.ContainsKey(new_hex_pos)) Adj(new_hex_pos);
                }
            } else
            {
                Debug.Log("Implementation for Buildings in progress");
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

                    Instantiate(road_obj, pixel_pos, Quaternion.identity, 
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

                    Instantiate(building_obj, pixel_pos, Quaternion.identity, 
                                GetComponent<Transform>());
                }
            }
        }
    }

    Vector3 CalcPixelPos(HexPos hex_pos)
    {
        float x = (float)(Math.Sqrt(3) * hex_pos.q  +  
                          Math.Sqrt(3) / 2 * hex_pos.r);
        float y = 3f/2 * hex_pos.r;
        x *= scale/8f;
        y *= scale/8f;

        return new Vector3(x, -y, 0);
    }

}