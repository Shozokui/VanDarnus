﻿using FFXIVClassic.Common;
using FFXIVClassic_World_Server.Actor.Group.Work;
using FFXIVClassic_World_Server.Packets.Send.Subpackets.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVClassic_World_Server.DataObjects.Group
{
    class Party : Group
    {
        public PartyWork partyGroupWork = new PartyWork();
        public List<uint> members = new List<uint>();

        public Party(ulong groupId, uint leaderCharaId) : base(groupId)
        {
            partyGroupWork._globalTemp.owner = (ulong)(((ulong)leaderCharaId << 32) | 0xB36F92);
            members.Add(leaderCharaId);
        }

        public void SetLeaderPlayerRequest(Session requestSession, string name)
        {
            if (GetLeader() != requestSession.sessionId)
            {
                requestSession.SendGameMessage(30428, 0x20, Server.GetServer().GetNameForId(requestSession.sessionId));
                return;
            }

            uint newLeader = GetIdForName(name);

            if (newLeader == 0)
            {
                requestSession.SendGameMessage(30575, 0x20);
                return;
            }
            else if (newLeader == GetLeader())
            {
                requestSession.SendGameMessage(30563, 0x20, name);
                return;
            }

            SetLeader(newLeader);
            SendLeaderWorkToAllMembers();

            for (int i = 0; i < members.Count; i++)
            {
                Session session = Server.GetServer().GetSession(members[i]);
                if (session == null)
                    continue;
                session.SendGameMessage(30429, 0x20, Server.GetServer().GetNameForId(members[i]));
            }

            Server.GetServer().GetWorldManager().SendPartySync(this);
        }

        public void KickPlayerRequest(Session requestSession, string name)
        {
            if (GetLeader() != requestSession.sessionId)
            {
                requestSession.SendGameMessage(30428, 0x20, Server.GetServer().GetNameForId(requestSession.sessionId));
                return;
            }

            uint kickedMemberId = GetIdForName(name);

            if (kickedMemberId == 0)
            {
                requestSession.SendGameMessage(30575, 0x20);
                return;
            }           

            for (int i = 0; i < members.Count; i++)
            {
                Session session = Server.GetServer().GetSession(members[i]);
                if (session == null)
                    continue;

                if (members[i] == kickedMemberId)
                    session.SendGameMessage(30410, 0x20);
                else
                    session.SendGameMessage(30428, 0x20, (Object)name);
            }

            //All good, remove
            members.Remove(kickedMemberId);
            SendGroupPacketsAll(members);
            Server.GetServer().GetWorldManager().SendPartySync(this);            
        }
       
        public void SendLeaderWorkToAllMembers()
        {
            for (int i = 0; i < members.Count; i++)
            {
                SynchGroupWorkValuesPacket leaderUpdate = new SynchGroupWorkValuesPacket(groupIndex);
                leaderUpdate.addProperty(this, "partyGroupWork._globalTemp.owner");
                leaderUpdate.setTarget("partyGroupWork/leader");
                Session session = Server.GetServer().GetSession(members[i]);
                if (session == null)
                    continue;
                else                
                    session.clientConnection.QueuePacket(leaderUpdate.buildPacket(session.sessionId, session.sessionId), true, false);                
            }
        }

        public void SetLeader(uint actorId)
        {
            partyGroupWork._globalTemp.owner = (ulong)((actorId << 32) | 0xB36F92);
        }

        public uint GetLeader()
        {
            return (uint)((partyGroupWork._globalTemp.owner >> 32) & 0xFFFFFFFF);
        }
        
        public uint GetIdForName(string name)
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (Server.GetServer().GetNameForId(members[i]).Equals(name))
                {
                    return members[i];
                }
            }
            return 0;
        }

        public bool IsInParty(uint charaId)
        {
            return members.Contains(charaId);
        }        

        public override void SendInitWorkValues(Session session)
        {
            SynchGroupWorkValuesPacket groupWork = new SynchGroupWorkValuesPacket(groupIndex);
            groupWork.addProperty(this, "partyGroupWork._globalTemp.owner");
            groupWork.setTarget("/_init");

            SubPacket test = groupWork.buildPacket(session.sessionId, session.sessionId);
            session.clientConnection.QueuePacket(test, true, false);
            test.DebugPrintSubPacket();
        }        

        public override int GetMemberCount()
        {
            return members.Count;
        }

        public override uint GetTypeId()
        {
            return Group.PlayerPartyGroup;
        }

        public override List<GroupMember> BuildMemberList()
        {
            List<GroupMember> groupMembers = new List<GroupMember>();
            foreach (uint charaId in members)
                groupMembers.Add(new GroupMember(charaId, -1, 0, false, true, Server.GetServer().GetNameForId(charaId)));
            return groupMembers;
        }

        
    }
}
