using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gespell.Gestures
{
    [Serializable]
    public class Gesture
    {
        [SerializeField] private Vector3[] path;

        public IEnumerable<Vector3> Path => path;

        public Gesture(IEnumerable<Vector3> path)
        {
            this.path = path.ToArray();
        }
        
        public float CompareWith(Gesture other)
        {
            var otherPath = other.Path.ToArray();
            // Compare gestures using Euclidean distance between corresponding points
            int minPoints = Mathf.Min(path.Length, otherPath.Length);
            float distance = 0f;

            for (int i = 0; i < minPoints; i++)
            {
                distance += Vector3.Distance(path[i], otherPath[i]);
            }

            // Normalize the distance based on the number of points
            return distance / minPoints;
        }
    }
}