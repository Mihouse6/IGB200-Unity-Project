using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileGrid TileGrid;
    public int[] GridPosition;
    public bool[] CableSides = new bool[4] { false, false, false, false }; //Filled from prefabs: { Top, Right, Bottom, Left }
    public bool[] ConnectedSides = new bool[4] { false, false, false, false };

    //TEMP VARS
    public Color ConnectedColour = Color.green;
    public Color DisconnectedColour = Color.red;

    private void Update()
    {
        //Don't process if game is paused
        if (GameManager.instance.Paused)
            return;

        CheckConnectedSides();

        //========= TEMPORARY CODE: Placeholder for tile image assets ==========
        if (CableSides[0])
        {
            Vector3 start = transform.position;
            start.y += 0.01f;
            Vector3 dest = transform.position;
            dest.y += 0.01f;
            dest.z += transform.localScale.z * 5f;
            Debug.DrawLine(transform.position, dest, ConnectedSides[0] ? ConnectedColour : DisconnectedColour);
        }
        if (CableSides[1])
        {
            Vector3 start = transform.position;
            start.y += 0.01f;
            Vector3 dest = transform.position;
            dest.y += 0.01f;
            dest.x += transform.localScale.x * 5f;
            Debug.DrawLine(transform.position, dest, ConnectedSides[1] ? ConnectedColour : DisconnectedColour);
        }
        if (CableSides[2])
        {
            Vector3 start = transform.position;
            start.y += 0.01f;
            Vector3 dest = transform.position;
            dest.y += 0.01f;
            dest.z -= transform.localScale.z * 5f;
            Debug.DrawLine(transform.position, dest, ConnectedSides[2] ? ConnectedColour : DisconnectedColour);
        }
        if (CableSides[3])
        {
            Vector3 start = transform.position;
            start.y += 0.01f;
            Vector3 dest = transform.position;
            dest.y += 0.01f;
            dest.x -= transform.localScale.x * 5f;
            Debug.DrawLine(transform.position, dest, ConnectedSides[3] ? ConnectedColour : DisconnectedColour);
        }
        //========= TEMPORARY CODE: Placeholder for tile image assets ==========
    }

    public void ClockwiseRotate()
    {
        //Rotate gameObejct
        transform.Rotate(new Vector3(0f, 90f, 0f));

        //Rightshift CableSides array
        bool[] newCableSides = new bool[] { false, false, false, false };
        newCableSides[0] = CableSides[3];
        newCableSides[1] = CableSides[0];
        newCableSides[2] = CableSides[1];
        newCableSides[3] = CableSides[2];
        CableSides = newCableSides;

        CheckConnectedSides();
    }

    //Check cable sides for connection to adjacent tiles
    public void CheckConnectedSides()
    {
        //Get adjacent tiles (if exists)
        Tile top = Top();
        Tile right = Right();
        Tile bottom = Bottom();
        Tile left = Left();

        //Disconnect all cables initially
        ConnectedSides = new bool[] { false, false, false, false };

        //If there is a top cable and a tile above
        if (CableSides[0] && top)
            if (top.CableSides[2]) //If the tile above has a bottom cable
                ConnectedSides[0] = true; //Connect the top cable

        //If there is a right cable and a tile to the right
        if (CableSides[1] && right)
            if (right.CableSides[3]) //If the tile to the right has a left cable
                ConnectedSides[1] = true; //Connect the right cable

        //If there is a bottom cable and a tile below
        if (CableSides[2] && bottom)
            if (bottom.CableSides[0]) //If the tile below has a top cable
                ConnectedSides[2] = true; //Connect the bottom cable

        //If there is a left cable and a tile to the left
        if (CableSides[3] && left)
            if (left.CableSides[1]) //If the tile to the left has a right cable
                ConnectedSides[3] = true; //Connect the left cable
    }

    #region Adjacent Tile Retrievers

    //Return the tile above this one (if exists)
    private Tile Top()
    {
        int[] adjacentGridPosition = new int[2] { GridPosition[0], GridPosition[1] + 1 };
        if (TileGrid.TileExists(adjacentGridPosition))
            return TileGrid.Board[adjacentGridPosition[0], adjacentGridPosition[1]].GetComponent<Tile>();
        return null;
    }

    //Return the tile to the right of this one (if exists)
    private Tile Right()
    {
        int[] adjacentGridPosition = new int[2] { GridPosition[0] + 1, GridPosition[1] };
        if (TileGrid.TileExists(adjacentGridPosition))
            return TileGrid.Board[adjacentGridPosition[0], adjacentGridPosition[1]].GetComponent<Tile>();
        return null;
    }

    //Return the tile below this one (if exists)
    private Tile Bottom()
    {
        int[] adjacentGridPosition = new int[2] { GridPosition[0], GridPosition[1] - 1 };
        if (TileGrid.TileExists(adjacentGridPosition))
            return TileGrid.Board[adjacentGridPosition[0], adjacentGridPosition[1]].GetComponent<Tile>();
        return null;
    }

    //Return the tile to the left of this one (if exists)
    private Tile Left()
    {
        int[] adjacentGridPosition = new int[2] { GridPosition[0] - 1, GridPosition[1] };
        if (TileGrid.TileExists(adjacentGridPosition))
            return TileGrid.Board[adjacentGridPosition[0], adjacentGridPosition[1]].GetComponent<Tile>();
        return null;
    }

    //Returns the list of adjacent tiles that this tile has connected cables with
    public List<Tile> GetConnectedTiles()
    {
        List<Tile> tiles = new List<Tile>();

        //Test sides for connected cable, add adjacent tile if connected
        if (ConnectedSides[0])
            tiles.Add(Top());
        if (ConnectedSides[1])
            tiles.Add(Right());
        if (ConnectedSides[2])
            tiles.Add(Bottom());
        if (ConnectedSides[3])
            tiles.Add(Left());

        return tiles;
    }

    #endregion
}
