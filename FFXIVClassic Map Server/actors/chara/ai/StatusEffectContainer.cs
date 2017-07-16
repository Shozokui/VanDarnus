﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFXIVClassic_Map_Server.Actors;
using FFXIVClassic_Map_Server.lua;
using FFXIVClassic_Map_Server.actors.area;
using FFXIVClassic_Map_Server.packets.send;
using FFXIVClassic_Map_Server.packets.send.actor;
using System.Collections.ObjectModel;

namespace FFXIVClassic_Map_Server.actors.chara.ai
{
    class StatusEffectContainer
    {
        private Character owner;
        private readonly Dictionary<uint, StatusEffect> effects;
        public static readonly int MAX_EFFECTS = 20;

        public StatusEffectContainer(Character owner)
        {
            this.owner = owner;
            this.effects = new Dictionary<uint, StatusEffect>();
        }

        public void Update(DateTime tick)
        {
            // list of effects to remove
            var removeEffects = new List<StatusEffect>();
            foreach (var effect in effects.Values)
            {
                // effect's update function returns true if effect has completed
                if (effect.Update(tick))
                    removeEffects.Add(effect);
            }

            // remove effects from this list
            foreach (var effect in removeEffects)
            {
                RemoveStatusEffect(effect);
            }
        }

        public bool AddStatusEffect(StatusEffect newEffect, bool silent = false)
        {
            // todo: check flags/overwritable and add effect to list
            var effect = GetStatusEffectById(newEffect.GetEffectId());
            bool canOverwrite = false;
            if (effect != null)
            {
                var overwritable = effect.GetOverwritable();
                canOverwrite = (overwritable == (uint)StatusEffectOverwrite.Always) ||
                   (overwritable == (uint)StatusEffectOverwrite.GreaterOnly && (effect.GetDurationMs() < newEffect.GetDurationMs() || effect.GetMagnitude() < newEffect.GetMagnitude())) ||
                   (overwritable == (uint)StatusEffectOverwrite.GreaterOrEqualTo && (effect.GetDurationMs() == newEffect.GetDurationMs() || effect.GetMagnitude() == newEffect.GetMagnitude()));
            }

            if (canOverwrite || effect == null)
            {
                if (!silent || (effect?.GetFlags() & (uint)StatusEffectFlags.Silent) == 0)
                {
                    // todo: send packet to client with effect added message
                }

                if (canOverwrite)
                    effects.Remove(effect.GetEffectId());

                effects.Add(newEffect.GetEffectId(), newEffect);

                // todo: this is retarded..
                {
                    var index = Array.IndexOf(effects.Values.ToArray(), newEffect);
                    owner.charaWork.status[index] = effect.GetEffectIdForCharaWork();
                    owner.charaWork.statusShownTime[index] = effect.GetDurationMs() / 1000;
                    this.owner.zone.BroadcastPacketAroundActor(this.owner, SetActorStatusPacket.BuildPacket(this.owner.actorId, (ushort)index, (ushort)effect.GetEffectId()));
                }
                return true;
            }
            return false;
        }

        public void RemoveStatusEffect(StatusEffect effect, bool silent = false)
        {
            if (effects.ContainsKey(effect.GetEffectId()))
            {
                // send packet to client with effect remove message
                if (!silent || (effect.GetFlags() & (uint)StatusEffectFlags.Silent) == 0)
                {
                    // todo: send packet to client with effect removed message
                }

                // todo: this is retarded..
                {
                    var index = Array.IndexOf(effects.Values.ToArray(), effect);
                    owner.charaWork.status[index] = effect.GetEffectIdForCharaWork();
                    this.owner.zone.BroadcastPacketAroundActor(this.owner, SetActorStatusPacket.BuildPacket(owner.actorId, (ushort)index, (ushort)0));
                }
                // function onLose(actor, effect
                LuaEngine.CallLuaStatusEffectFunction(this.owner, effect, "onLose", this.owner, effect);
                effects.Remove(effect.GetEffectId());
            }
        }

        public void RemoveStatusEffect(uint effectId, bool silent = false)
        {
            foreach (var effect in effects.Values)
            {
                if (effect.GetEffectId() == effectId)
                {
                    RemoveStatusEffect(effect, silent);
                    break;
                }
            }
        }

        public StatusEffect CopyEffect(StatusEffect effect)
        {
            var newEffect = new StatusEffect(this.owner, effect);
            newEffect.SetOwner(this.owner);

            return AddStatusEffect(newEffect) ? newEffect : null;
        }

        public bool RemoveStatusEffectsByFlags(uint flags, bool silent = false)
        {
            // build list of effects to remove
            var removeEffects = new List<StatusEffect>();
            foreach (var effect in effects.Values)
                if ((effect.GetFlags() & flags) != 0)
                    removeEffects.Add(effect);

            // remove effects from main list
            foreach (var effect in removeEffects)
                RemoveStatusEffect(effect, silent);

            // removed an effect with one of these flags
            return removeEffects.Count > 0;
        }

        public StatusEffect GetStatusEffectById(uint id, byte tier = 0xFF)
        {
            StatusEffect effect;

            if (effects.TryGetValue(id, out effect) && effect.GetEffectId() == id && (tier != 0xFF ? effect.GetTier() == tier : true))
                return effect;

            return null;
        }

        public List<StatusEffect> GetStatusEffectsByFlag(uint flag)
        {
            var list = new List<StatusEffect>();
            foreach (var effect in effects.Values)
            {
                if ((effect.GetFlags() & flag) > 0)
                {
                    list.Add(effect);
                }
            }
            return list;
        }

        public bool HasStatusEffectsByFlag(uint flag)
        {
            foreach (var effect in effects.Values)
            {
                if ((effect.GetFlags() & flag) > 0)
                    return true;
            }
            return false;
        }

        public IEnumerable<StatusEffect> GetStatusEffects()
        {
            return effects.Values;
        }

        void SaveStatusEffectsToDatabase(StatusEffectFlags removeEffectFlags = StatusEffectFlags.None)
        {
            if (owner is Player)
            {
                Database.SavePlayerStatusEffects((Player)owner);
            }
        }
    }
}
