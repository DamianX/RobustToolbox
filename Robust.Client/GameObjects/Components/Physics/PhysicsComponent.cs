﻿using System;
using Robust.Shared.GameObjects;
using Robust.Shared.Log;
using Robust.Shared.Maths;
using Robust.Shared.ViewVariables;

namespace Robust.Client.GameObjects
{
    /// <summary>
    ///     Contains physical properties of the entity. This component registers the entity
    ///     in the physics system as a dynamic ridged body object that has physics. This behavior overrides
    ///     the BoundingBoxComponent behavior of making the entity static.
    /// </summary>
    internal class PhysicsComponent : Component
    {
        /// <inheritdoc />
        public override string Name => "Physics";

        /// <inheritdoc />
        public override uint? NetID => NetIDs.PHYSICS;

        /// <inheritdoc />
        public override Type StateType => typeof(PhysicsComponentState);

        /// <summary>
        ///     Current mass of the entity in kg.
        /// </summary>
        [ViewVariables]
        public float Mass { get; private set; }

        /// <summary>
        ///     Current velocity of the entity.
        /// </summary>
        [ViewVariables]
        public Vector2 Velocity { get; private set; }

        /// <inheritdoc />
        public override void Initialize()
        {
            // This component requires that the entity has an AABB.
            if (!Owner.HasComponent<BoundingBoxComponent>())
                Logger.Error($"[ECS] {Owner.Prototype.Name} - {nameof(PhysicsComponent)} requires {nameof(BoundingBoxComponent)}. ");

            base.Initialize();
        }

        /// <inheritdoc />
        public override void HandleComponentState(ComponentState curState, ComponentState nextState)
        {
            if (curState == null)
                return;

            var newState = (PhysicsComponentState)curState;
            Mass = newState.Mass / 1000f; // gram to kilogram
            Velocity = newState.Velocity;
        }
    }
}
