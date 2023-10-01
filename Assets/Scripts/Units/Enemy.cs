using System;
using DG.Tweening;
using Gespell.Enums;
using UnityEngine;

namespace Gespell.Units
{
    public class Enemy : UnitBase
    {
        [SerializeField, Min(0.1f)] private float range = 0.5f;
        [SerializeField, Min(0.1f)] private float knockDuration = 0.5f;
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
            moveTween?.Kill();
            moveTween = transform.DOMove(targetPosition, DistanceTo(targetPosition) / Stat.speed)
                .SetEase(Ease.InSine)
                .OnKill(onComplete);
        }

        private float DistanceTo(Vector3 targetPosition) => Vector3.Distance(transform.position, targetPosition);

        private void OnCollideWithPlayer()
        {
            // Additional check before attack just in case it's called on OnKill when it's not near player yet
            if(DistanceTo(Manager.Player.transform.position) < range) Attack(Manager.Player, Stat.attack);
            
            // Knock self back
            knockTween?.Kill();
            knockTween = transform.DOMove((transform.position - initialPosition).normalized, knockDuration)
                .SetEase(Ease.OutBack)
                .OnKill(StartMoving);
        }
    }
}