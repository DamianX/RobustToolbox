﻿using Robust.Server.Interfaces.GameObjects;
using Robust.Shared.GameObjects;
using System;
using System.Collections.Generic;

namespace Robust.Server.GameObjects
{
    public class ParticleSystemComponent : Component, IParticleSystemComponent
    {
        public override string Name => "ParticleSystem";
        public override uint? NetID => NetIDs.PARTICLE_SYSTEM;

        private Dictionary<string, Boolean> emitters = new Dictionary<string, bool>();

        public override ComponentState GetComponentState()
        {
            return new ParticleSystemComponentState(emitters);
        }

        public void AddParticleSystem(string name, bool active)
        {
            if (!emitters.ContainsKey(name))
                emitters.Add(name, active);
        }

        public void RemoveParticleSystem(string name)
        {
            if (emitters.ContainsKey(name))
                emitters.Remove(name);
        }

        public void SetParticleSystemActive(string name, bool active)
        {
            if (emitters.ContainsKey(name))
                emitters[name] = active;
        }
    }
}
