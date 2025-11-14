using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public interface INode
{
    public bool IsBlocked();
    public void SetBlocked(bool isBloqued);
    public int GetTerrainCost();
    public void SetTerrainCost(int terrainCost);
}

public interface INode<Coordinate> 
{
    public void SetCoordinate(Coordinate coordinateType);
    public Coordinate GetCoordinate();
    public void SetWorldPosition(Coordinate worldPosition);
    public Coordinate GetWorldPosition();
    public bool Equals(INode<Coordinate> other);
}
