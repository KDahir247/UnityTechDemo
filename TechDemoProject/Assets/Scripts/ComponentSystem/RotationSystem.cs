using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Tech.Component;
using Tech.Data;
using UniRx;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
namespace Tech.Job
{
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    public class RotationSystem : ComponentSystem
    {
        private RotationDirection _direction;

        private int _rotationDirection = 0;

        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _rotationTween;
        
        protected override void OnStartRunning()
        {
            MessageBroker.Default.Receive<RotationDirection>().Subscribe(direction =>
            {
                switch (direction)
                {
                    case RotationDirection.None:
                        _rotationDirection = 0;
                        break;
                    case RotationDirection.Left:
                        _rotationDirection = -1;
                        break;
                    default:
                        _rotationDirection = 1;
                        break;
                    
                }
            });
        }


        protected override void OnUpdate()
        {
            Entities
                .WithAll<Transform>()
                .ForEach((Entity entity, Transform transform, ref RotationComponent rotationComponent) =>
                {
                    _rotationTween = transform.DORotate(
                                rotationComponent.direction * rotationComponent.rotationSpeed * _rotationDirection,
                                rotationComponent.rotationDuration, rotationComponent.rotateMode)
                            .SetEase(rotationComponent.rotateEase)
                            .Play();
                });
        }
    }
}