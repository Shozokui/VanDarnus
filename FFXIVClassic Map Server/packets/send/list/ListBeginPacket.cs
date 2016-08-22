﻿using System;
using System.IO;

using FFXIVClassic.Common;

namespace FFXIVClassic_Map_Server.packets.send.list
{
    class ListBeginPacket
    {
        public const ushort OPCODE = 0x017D;
        public const uint PACKET_SIZE = 0x40;

        public static SubPacket BuildPacket(uint playerActorID, uint locationCode, ulong sequenceId, ulong listId, int numEntries)
        {
            byte[] data = new byte[PACKET_SIZE - 0x20];

            using (MemoryStream mem = new MemoryStream(data))
            {
                using (BinaryWriter binWriter = new BinaryWriter(mem))
                {
                    //Write List Header
                    binWriter.Write((UInt64)locationCode);
                    binWriter.Write((UInt64)sequenceId);
                    //Write List Info
                    binWriter.Write((UInt64)listId);
                    binWriter.Write((UInt32)numEntries);
                }
            }

            return new SubPacket(OPCODE, playerActorID, playerActorID, data);
        }
    }
}
