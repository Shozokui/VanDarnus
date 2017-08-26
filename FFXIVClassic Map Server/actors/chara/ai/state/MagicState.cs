﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFXIVClassic.Common;
using FFXIVClassic_Map_Server.Actors;
using FFXIVClassic_Map_Server.packets.send.actor;
using FFXIVClassic_Map_Server.packets.send.actor.battle;
using FFXIVClassic_Map_Server.packets.send;

namespace FFXIVClassic_Map_Server.actors.chara.ai.state
{
    class MagicState : State
    {

        private Ability spell;
        private uint cost;
        private Vector3 startPos;

        public MagicState(Character owner, Character target, ushort spellId) :
            base(owner, target)
        {
            this.startTime = DateTime.Now;
            // todo: lookup spell from global table
            this.spell = Server.GetWorldManager().GetAbility(spellId);
            var returnCode = lua.LuaEngine.CallLuaAbilityFunction(owner, spell, "spells", "onSpellPrepare", owner, target, spell);

            // todo: check recast
            if (owner.CanCast(target, spell, ref errorPacket))
            {
                // todo: Azia can fix, check the recast time and send error
                OnStart();
            }
            else if (interrupt || errorPacket != null)
            {
                if (owner is Player && errorPacket != null)
                    ((Player)owner).QueuePacket(errorPacket);

                errorPacket = null;
                interrupt = true;
            }
        }

        public override void OnStart()
        {
            var returnCode = lua.LuaEngine.CallLuaAbilityFunction(owner, spell, "spells", "onSpellStart", owner, target, spell);

            if (returnCode != 0)
            {
                interrupt = true;
                errorPacket = BattleActionX01Packet.BuildPacket(owner.actorId, owner.actorId, owner.actorId, 0, 0, (ushort)(returnCode == -1 ? 32558 : returnCode), spell.id, 0, 1);
            }
            else
            {
                // todo: check within attack range
                startPos = owner.GetPosAsVector3();
                owner.LookAt(target);

                foreach (var player in owner.zone.GetActorsAroundActor<Player>(owner, 50))
                {
                    // todo: this is retarded, prolly doesnt do what i think its gonna do
                    //player.QueuePacket(BattleActionX01Packet.BuildPacket(player.actorId, owner.actorId, target != null ? target.actorId : 0xC0000000, spell.battleAnimation, spell.effectAnimation, 0, spell.id, 0, (byte)spell.castTimeSeconds));
                }
            }
        }

        public override bool Update(DateTime tick)
        {
            if (spell != null)
            {
                TryInterrupt();

                if (interrupt)
                {
                    OnInterrupt();
                    return true;
                }

                // todo: check weapon delay/haste etc and use that
                var actualCastTime = spell.castTimeSeconds;

                if ((tick - startTime).TotalSeconds >= spell.castTimeSeconds)
                {
                    OnComplete();
                    return true;
                }
                return false;
            }
            return true;
        }

        public override void OnInterrupt()
        {
            // todo: send paralyzed/sleep message etc.
            if (errorPacket != null)
            {
                owner.zone.BroadcastPacketAroundActor(owner, errorPacket);
            }
        }

        public override void OnComplete()
        {
            spell.targetFind.FindWithinArea(target, spell.validTarget);
            isCompleted = true;
            
            var targets = spell.targetFind.GetTargets();
            BattleAction[] actions = new BattleAction[targets.Count];
            List<SubPacket> packets = new List<SubPacket>();
            var i = 0;
            foreach (var chara in targets)
            {
                var action = new BattleAction();
                action.effectId = spell.effectAnimation;
                action.param = 1;
                action.unknown = 1;
                action.targetId = chara.actorId;
                action.worldMasterTextId = spell.worldMasterTextId;
                action.amount = (ushort)lua.LuaEngine.CallLuaAbilityFunction(owner, spell, "spells", "onSpellFinish", owner, chara, spell, action);
                actions[i++] = action;

                //packets.Add(BattleActionX01Packet.BuildPacket(chara.actorId, owner.actorId, action.targetId, spell.battleAnimation, action.effectId, action.worldMasterTextId, spell.id, action.amount, action.param));
            }
            packets.Add(BattleActionX10Packet.BuildPacket(owner.target.actorId, owner.actorId, spell.battleAnimation, spell.id, actions));
            owner.zone.BroadcastPacketsAroundActor(owner, packets);
        }

        public override void TryInterrupt()
        {
            if (interrupt)
                return;

            if (owner.statusEffects.HasStatusEffectsByFlag((uint)StatusEffectFlags.PreventAction))
            {
                // todo: sometimes paralyze can let you attack, get random percentage of actually letting you attack
                var list = owner.statusEffects.GetStatusEffectsByFlag((uint)StatusEffectFlags.PreventAction);
                uint effectId = 0;
                if (list.Count > 0)
                {
                    // todo: actually check proc rate/random chance of whatever effect
                    effectId = list[0].GetStatusEffectId();
                }
                // todo: which is actually the swing packet
                //this.errorPacket = BattleActionX01Packet.BuildPacket(target.actorId, owner.actorId, target.actorId, 0, effectId, 0, (ushort)BattleActionX01PacketCommand.Attack, (ushort)damage, 0);
                //owner.zone.BroadcastPacketAroundActor(owner, errorPacket);
                //errorPacket = null;
                interrupt = true;
                return;
            }
            
            interrupt = !CanCast();
        }

        private bool CanCast()
        {
            return owner.CanCast(target, spell, ref errorPacket) && !HasMoved();
        }

        private bool HasMoved()
        {
            return (Utils.DistanceSquared(owner.GetPosAsVector3(), startPos) > 4.0f);
        }
    }
}
