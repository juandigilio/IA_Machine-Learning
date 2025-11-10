using System.Collections.Generic;
using UnityEngine;

public abstract class Voronoi<NodeType> 
{
    protected abstract void AddVoroniPoints(List<NodeType> seccionsToCull);
    protected abstract void OrderByDistance();
}
