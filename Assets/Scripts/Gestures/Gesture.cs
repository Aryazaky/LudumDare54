using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gespell.Gestures
{
    [Serializable]
    public class Gesture
    {
        [SerializeField] private Vector2[] path;
        private string name;

        public Gesture(IEnumerable<Vector2> path, string name = "")
        {
            this.path = path.ToArray();
            this.name = name;
        }

        public IEnumerable<Vector2> Path => path;

        public int PointCount => path.Length;
        
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
        
        public IEnumerable<Vector2> GetResampledPath(int targetPointCount) => GetResampledPath(path, targetPointCount);

        public static IEnumerable<Vector2> GetResampledPath(IEnumerable<Vector2> path, int targetPointCount)
        {
            List<Vector2> pathList = new(path);
            int initialPathCount = pathList.Count;
            float spacing = GetPathLength(pathList) / (targetPointCount - 1);
            float accumulatedDistance = 0f;
            List<Vector2> resampledPath = new List<Vector2> { pathList[0] };

            for (int i = 1; i < pathList.Count; i++)
            {
                float segmentLength = Vector2.Distance(pathList[i - 1], pathList[i]);
                if (accumulatedDistance + segmentLength >= spacing)
                {
                    float ratio = (spacing - accumulatedDistance) / segmentLength;
                    Vector2 interpolatedPoint = Vector2.Lerp(pathList[i - 1], pathList[i], ratio);
                    resampledPath.Add(interpolatedPoint);
                    pathList.Insert(i, interpolatedPoint);
                    accumulatedDistance = 0f;
                }
                else
                {
                    accumulatedDistance += segmentLength;
                }
            }

            // Make sure the resampled path contains the target number of points
            while (resampledPath.Count < targetPointCount)
            {
                resampledPath.Add(pathList[^1]);
            }

            //Debug.LogWarning($"Resampled from {initialPathCount} to {resampledPath.Count} (target: {targetPointCount})");

            return resampledPath;
        }

        public float PathLength => GetPathLength();

        private float GetPathLength()
        {
            return GetPathLength(path);
        }

        public static float GetPathLength(IList<Vector2> pathList)
        {
            float length = 0f;
            for (int i = 1; i < pathList.Count; i++)
            {
                length += Vector2.Distance(pathList[i - 1], pathList[i]);
            }
            return length;
        }
        
        public IEnumerable<Vector2> ScaleToMatch(Gesture targetGesture)
        {
            // Calculate the bounding boxes of both paths
            Rect boundingBoxToScale = GetBoundingBox();
            Rect targetBoundingBox = targetGesture.GetBoundingBox();
            
            if (boundingBoxToScale.width == 0f || boundingBoxToScale.height == 0f ||
                targetBoundingBox.width == 0f || targetBoundingBox.height == 0f)
            {
                // Handle special case: either path has zero width or height
                Debug.LogWarning("One of the paths has zero width or height. Cannot scale.");
                return path; // Return the original path without scaling
            }

            // Calculate the scaling factors for both X and Y dimensions
            float scaleX = targetBoundingBox.width / boundingBoxToScale.width;
            float scaleY = targetBoundingBox.height / boundingBoxToScale.height;

            // Scale the points of the pathToScale
            List<Vector2> scaledPath = new List<Vector2>();
            foreach (Vector2 point in path)
            {
                float scaledX = (point.x - boundingBoxToScale.x) * scaleX + targetBoundingBox.x;
                float scaledY = (point.y - boundingBoxToScale.y) * scaleY + targetBoundingBox.y;
                scaledPath.Add(new Vector2(scaledX, scaledY));
            }

            return scaledPath;
        }

        public Rect BoundingBox => GetBoundingBox();
        
        private Rect GetBoundingBox()
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (Vector2 point in path)
            {
                minX = Mathf.Min(minX, point.x);
                minY = Mathf.Min(minY, point.y);
                maxX = Mathf.Max(maxX, point.x);
                maxY = Mathf.Max(maxY, point.y);
            }

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }
        
        public override string ToString()
        {
            return $"{name} (Gesture)";
        }
    }
}