//using System.Collections.Generic;
//using UnityEngine;

//public static class MinersManager
//{
//    [SerializeField] private static List<Miner> miners = new List<Miner>();

//    public static void AddMiner(Miner miner)
//    {
//        if (!miners.Contains(miner))
//        {
//            miners.Add(miner);
//        }
//        else 
//        {       
//            Debug.LogWarning("Miner already exists in the manager.");
//        }
//    }

//    public static List<Miner> GetAllMiners()
//    {
//        return miners;
//    }

//    public static void RemoveMiner(Miner miner)
//    {
//        if (miners.Contains(miner))
//        {
//            miners.Remove(miner);
//        }
//        else 
//        {       
//            Debug.LogWarning("Miner does not exist in the manager.");
//        }
//    }
//}
