using System;
using DG.Tweening;
using Gespell.Enums;
using UnityEngine;

namespace Gespell.Units
{
    public class Enemy : UnitBase
    {
        [SerializeField, Min(0)] private float knockStrength = 0.5f;
        [SerializeField, Min(0.1f)] private float knockDuration = 2f;
        private Vector3 initialPosition;
        private Tweener moveTween;
        private Tweener knockTween;

        public override void Initialize((UnitManager unitManager, UnitStat stat, UnitFaction faction) data)
        {
            base.Initialize((data.unitManager, data.stat, UnitFaction.Enemy));
            initialPosition = transform.position;
            StartMoving();
        }

        private void StartMoving() => StartMoving(Manager.Player.transform.position, OnCollideWithPlayer);

        private void StartMoving(Vector3 targetPosition, TweenCallback onComplete)
        {
            var distanceTo = DistanceTo(targetPosition);
            var duration = distanceTo / Stat.speed;
            Debug.Log($"{this} start moving for {duration}, distance {distanceTo}");
            moveTween = transform.DOMove(GetPositionBetween(targetPosition, transform.position, Stat.range), duration)
                .SetEase(Ease.InSine)
                .OnKill(onComplete);
        }

        private float DistanceTo(Vector3 targetPosition) => Vector3.Distance(transform.position, targetPosition);

        private void OnCollideWithPlayer()
        {
            Debug.Log($"{this} collides with player");
            // Additional check before attack just in case it's called on OnKill when it's not near player yet
            if(DistanceTo(Manager.Player.transform.position) <= Stat.range) Attack(Manager.Player, Stat.attack);
            
            // Knock self back
            knockTween = transform.DOMove(GetPositionBetween(transform.position, initialPosition, knockStrength), knockDuration)
                .SetEase(Ease.OutBack)
                .OnKill(StartMoving);
        }

        private static Vector3 GetPositionBetween(Vector3 A, Vector3 B, float x)
        {
            Vector3 direction = B - A;
            float distance = direction.magnitude;
        
            // Ensure B is not equal to A to avoid division by zero
            if (distance > 0) return A + (x / distance) * direction;

            // A and B are the same point, return A
            return A;
        }
    }
}