using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SpawnPointCollector : MonoBehaviour
    {
        public Transform[] _transformSpawnPoints;

        public Vector3[] GetPositions()
        {
            Vector3[] points = new Vector3[_transformSpawnPoints.Length];
            for (int i = 0; i < _transformSpawnPoints.Length; i++)
            {
                points[i] = _transformSpawnPoints[i].position;
            }

            return points;
        }
    }
}