﻿using System;
using System.IO;

namespace FFXIVClassic_Map_Server.packets.send
{
    class _0x10Packet
    {
        public const ushort OPCODE = 0x0010;
        public const uint PACKET_SIZE = 0x28;

        public static SubPacket buildPacket(uint playerActorId, int val)
        {
            byte[] data = new byte[PACKET_SIZE - 0x20];

            using (MemoryStream mem = new MemoryStream(data))
            {
                using (BinaryWriter binWriter = new BinaryWriter(mem))
                {
                    binWriter.Write((UInt32)val);
                }
            }

            return new SubPacket(OPCODE, playerActorId, playerActorId, data);
        }
    }
}
