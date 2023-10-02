using System;
using System.Collections.Generic;
using System.Linq;
using Gespell.Utilities;
using UnityEngine;

namespace Gespell.Gestures
{
    [RequireComponent(typeof(LineRenderer))]
    public class GestureRecognizer : MonoBehaviour
    {
        [SerializeField] private int basePointCount = 5;
        [SerializeField] private float lengthIncrement = 0.5f;
        [SerializeField] private float moveThreshold = 0.2f;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private LineRenderer lineRendererPrefab;

        private List<LineRenderer> lineRenderers = new();
        private List<Gesture> gestures = new();
        //[SerializeField] private LineRenderer[] lineRendererDebug;
        private Vector2 lastMousePosition;
        private readonly List<Vector2> currentPath = new();
        private bool drawing;

        public event Action<SpellManager.SpellType> OnSpellDrawn;

        private void Update()
        {
            // Handle mouse input for drawing gestures
            if (Input.GetMouseButtonDown(0))
            {
                drawing = true;
                currentPath.Clear();
                lastMousePosition = GetMouseWorldPosition();
                currentPath.Add(lastMousePosition);
                DrawGesturePath(currentPath);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                drawing = false;
                if(!IsPathTooCloseToForbidden(currentPath, gestures.SelectMany(g => g.Path), 0.1f))
                {
                    CheckGesture();
                }
            }
            else if (drawing)
            {
                Vector2 mousePosition = GetMouseWorldPosition();
                float distance = Vector2.Distance(mousePosition, lastMousePosition);

                // Check if the mouse has moved some distance before adding the new point
                if (distance >= moveThreshold)
                {
                    lastMousePosition = mousePosition;
                    currentPath.Add(mousePosition);
                    DrawGesturePath(currentPath);
                }
            }
        }

        private static Vector2 GetMouseWorldPosition()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return mousePosition;
        }
        
        private bool IsPathTooCloseToForbidden(IEnumerable<Vector2> gesturePath, IEnumerable<Vector2> forbiddenPath, float minimumDistance)
        {
            return gesturePath.Any(gesturePoint =>
                forbiddenPath.Select(forbiddenPoint => Vector2.Distance(gesturePoint, forbiddenPoint))
                    .Any(distance => distance < minimumDistance));
        }

        private void CheckGesture()
        {
            // Calculate the targetPointCount based on the length of the currentPath
            var rawGesture = new Gesture(currentPath);
            float pathLength = rawGesture.PathLength;
            if(rawGesture.PointCount > 1 && pathLength > 0)
            {
                int additionalPoints = Mathf.FloorToInt(pathLength / lengthIncrement);
                int targetPointCount = basePointCount + additionalPoints;

                // Perform resampling, rotation normalization, and scaling before storing the gesture
                var resampledPath = rawGesture.GetResampledPath(targetPointCount).ToList();
                Gesture newGesture = new Gesture(resampledPath);

                // Store the gesture for recognition
                CheckGeometry(newGesture);
                
                DrawGesturePath(resampledPath);
            }
        }

        private void CheckGeometry(Gesture gesture)
        {
            var points = PolylineSimplification.DouglasPeuckerSimplify(gesture.Path.ToList(), 1f);
            
            var isCircle = GestureUtility.IsCircle(points.ToArray(), 1f, 1);

            var points2 = gesture.GetResampledPath(5).ToArray();
            var isStraightLine = GestureUtility.IsALine(points2, 20, 0.2f);
            var isVerticalDirection = GestureUtility.IsVerticalDirection(points2[0] - points[^1]);

            var points3 = gesture.GetResampledPath(3).ToArray();
            var isVShape = GestureUtility.IsVShape(points3, 60);

            //DrawGesturePathDebug(points, points2, points3);
            //Debug.LogWarning($"circle:{isCircle}, line:{isStraightLine} (horizontal? {isVerticalDirection}), v:{isVShape}");

            if (isCircle)
            {
                OnSpellDrawn?.Invoke(SpellManager.SpellType.O);
                StoreGesture(gesture);
            }
            else if(isVShape)
            {
                OnSpellDrawn?.Invoke(SpellManager.SpellType.V);
                StoreGesture(gesture);
            }
            else if (isStraightLine)
            {
                OnSpellDrawn?.Invoke(
                    isVerticalDirection ? SpellManager.SpellType.I : SpellManager.SpellType.HorizontalI);
                StoreGesture(gesture);
            }
        }

        private void StoreGesture(Gesture gesture)
        {
            var line = Instantiate(lineRendererPrefab, transform);
            lineRenderers.Add(line);
            DrawGesturePath(line, gesture.Path.ToArray());
            gestures.Add(gesture);
        }

        private void DrawGesturePath(IReadOnlyList<Vector2> path)
        {
            DrawGesturePath(lineRenderer, path);
        }

        // private void DrawGesturePathDebug(params IReadOnlyList<Vector2>[] paths)
        // {
        //     var a = paths.Zip(lineRendererDebug, (path, renderer) => (path, renderer));
        //     foreach (var b in a)
        //     {
        //         if(b.renderer == null || b.path == null) continue;
        //         DrawGesturePath(b.renderer, b.path);
        //     }
        // }
        
        private static void DrawGesturePath(LineRenderer renderer, IReadOnlyList<Vector2> path)
        {
            // Set the number of points in the LineRenderer
            renderer.positionCount = path.Count;

            // Set the positions of the LineRenderer based on the points in the path
            for (int i = 0; i < path.Count; i++)
            {
                renderer.SetPosition(i, path[i]);
            }
        }
    }
}