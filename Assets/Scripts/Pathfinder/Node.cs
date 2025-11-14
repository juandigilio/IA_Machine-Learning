[System.Serializable]
public class Node<Coordinate> : INode, INode<Coordinate>
{
    private Coordinate coordinate;
    private Coordinate worldPosition;
    private int terrainCost = 1;
    private bool isBlocked = false;


    public void SetCoordinate(Coordinate coordinate)
    {
        this.coordinate = coordinate;
    }

    public Coordinate GetCoordinate()
    {
        return coordinate;
    }

    public void SetWorldPosition(Coordinate worldPosition)
    {
        this.worldPosition = worldPosition;
    }

    public Coordinate GetWorldPosition()
    {
        return worldPosition;
    }

    public bool IsBlocked()
    {
        return isBlocked;
    }

    public void SetBlocked(bool isBlocked)
    {
        this.isBlocked = isBlocked;
    }

    public int GetTerrainCost()
    {
        return terrainCost;
    }

    public void SetTerrainCost(int terrainCost)
    {
        this.terrainCost = terrainCost;
    }

    public bool Equals(INode<Coordinate> other)
    {
        if (coordinate. == other.GetCoordinate().x)
        {
            if (coordinate == null) return other.coordinate == null;
            return coordinate.Equals(other.coordinate);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return coordinate == null ? 0 : coordinate.GetHashCode();
    }

    public override string ToString() => $"Node({coordinate})";
}