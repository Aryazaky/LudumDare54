using System.Collections.Generic;
using UnityEngine;

namespace Gespell.Gestures
{
    [RequireComponent(typeof(LineRenderer))]
    public class GestureRecognizer : MonoBehaviour
    {
        private List<Vector3> currentPath;
        private bool drawing = false;
        private List<Gesture> gestureTemplates;
        private List<Gesture> inputGestures;

        private void Start()
        {
            currentPath = new List<Vector3>();
            gestureTemplates = new List<Gesture>();
            inputGestures = new List<Gesture>();

            // Example: Initialize gesture templates with predefined paths
            gestureTemplates.Add(new Gesture(new List<Vector3> { /* Add Vector3 points for the first template gesture */ }));
            gestureTemplates.Add(new Gesture(new List<Vector3> { /* Add Vector3 points for the second template gesture */ }));
            // Add more template gestures as needed
        }

        private void Update()
        {
            // Handle mouse input for drawing gestures
            if (Input.GetMouseButtonDown(0))
            {
                drawing = true;
                currentPath.Clear();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                drawing = false;
                StoreGesture();
            }

            if (drawing)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                currentPath.Add(mousePosition);
            }
        }

        private Gesture GetBestMatch(Gesture inputGesture)
        {
            Gesture bestMatch = null;
            float bestScore = float.MaxValue;

            foreach (Gesture templateGesture in gestureTemplates)
            {
                float score = inputGesture.CompareWith(templateGesture);

                // Find the template gesture with the lowest similarity score
                if (score < bestScore)
                {
                    bestScore = score;
                    bestMatch = templateGesture;
                }
            }

            return bestMatch;
        }

        private void StoreGesture()
        {
            // Perform resampling, rotation normalization, and scaling before storing the gesture
            List<Vector3> resampledPath = Resample(currentPath, 5);
            List<Vector3> normalizedPath = RotateAndScale(resampledPath);
            Gesture newGesture = new Gesture(normalizedPath);

            // Store the gesture for recognition
            inputGestures.Add(newGesture);

            // Optional: visualize the stored gesture path
            DrawGesturePath(normalizedPath);
        }

        private List<Vector3> Resample(List<Vector3> path, int targetPointCount)
        {
            float pathLength = CalculatePathLength(path);
            float spacing = pathLength / (targetPointCount - 1);
            float accumulatedDistance = 0f;
            List<Vector3> resampledPath = new List<Vector3> { path[0] };

            for (int i = 1; i < path.Count; i++)
            {
                float segmentLength = Vector3.Distance(path[i - 1], path[i]);
                if (accumulatedDistance + segmentLength >= spacing)
                {
                    float ratio = (spacing - accumulatedDistance) / segmentLength;
                    Vector3 interpolatedPoint = Vector3.Lerp(path[i - 1], path[i], ratio);
                    resampledPath.Add(interpolatedPoint);
                    path.Insert(i, interpolatedPoint);
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
                resampledPath.Add(path[^1]);
            }

            return resampledPath;
        }

        private float CalculatePathLength(IReadOnlyList<Vector3> path)
        {
            float length = 0f;
            for (int i = 1; i < path.Count; i++)
            {
                length += Vector3.Distance(path[i - 1], path[i]);
            }
            return length;
        }

        private List<Vector3> RotateAndScale(List<Vector3> path)
        {
            // ...
            return path;
        }

        private List<Vector3> RotateToOrigin(List<Vector3> path)
        {
            Vector3 centroid = CalculateCentroid(path);
            Quaternion rotation = Quaternion.FromToRotation(centroid, Vector3.forward);
            List<Vector3> rotatedPath = new List<Vector3>();

            foreach (Vector3 point in path)
            {
                Vector3 rotatedPoint = rotation * (point - centroid);
                rotatedPath.Add(rotatedPoint);
            }

            return rotatedPath;
        }

        private Vector3 CalculateCentroid(List<Vector3> points)
        {
            Vector3 centroid = Vector3.zero;
            foreach (Vector3 point in points)
            {
                centroid += point;
            }
            return centroid / points.Count;
        }

        private List<Vector3> ScaleToFit(List<Vector3> path, float targetSize)
        {
            Bounds boundingBox = CalculateBoundingBox(path);
            float scaleFactor = targetSize / Mathf.Max(boundingBox.size.x, boundingBox.size.y);
            List<Vector3> scaledPath = new List<Vector3>();

            foreach (Vector3 point in path)
            {
                Vector3 scaledPoint = point - boundingBox.center;
                scaledPoint *= scaleFactor;
                scaledPath.Add(scaledPoint);
            }

            return scaledPath;
        }

        private Bounds CalculateBoundingBox(List<Vector3> points)
        {
            Vector3 min = points[0];
            Vector3 max = points[0];

            foreach (Vector3 point in points)
            {
                min = Vector3.Min(min, point);
                max = Vector3.Max(max, point);
            }

            return new Bounds((max + min) / 2f, max - min);
        }

        private void DrawGesturePath(List<Vector3> path)
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();

            // Set the number of points in the LineRenderer
            lineRenderer.positionCount = path.Count;

            // Set the positions of the LineRenderer based on the points in the path
            for (int i = 0; i < path.Count; i++)
            {
                lineRenderer.SetPosition(i, path[i]);
            }
        }
    }
}