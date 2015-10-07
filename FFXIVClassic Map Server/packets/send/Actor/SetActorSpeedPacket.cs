﻿using FFXIVClassic_Lobby_Server.packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVClassic_Map_Server.packets.send.actor
{
    class SetActorSpeedPacket
    {
        public const ushort OPCODE = 0x00D0;
        public const uint PACKET_SIZE = 0xA8;

        public const ushort DEFAULT_STOP = 0x0000;
        public const ushort DEFAULT_WALK = 0x4000;
        public const ushort DEFAULT_RUN  = 0x40A0;

        public static SubPacket buildPacket(uint playerActorID, uint targetActorID)
        {
            byte[] data = new byte[PACKET_SIZE - 0x20];

            using (MemoryStream mem = new MemoryStream(data))
            {
                using (BinaryWriter binWriter = new BinaryWriter(mem))
                {
                    binWriter.Write((UInt16)00);
                    binWriter.Write((UInt16)DEFAULT_STOP);
                    binWriter.Write((UInt32)0);

                    binWriter.Write((UInt16)00);
                    binWriter.Write((UInt16)DEFAULT_WALK);
                    binWriter.Write((UInt32)1);

                    binWriter.Write((UInt16)00);
                    binWriter.Write((UInt16)DEFAULT_RUN);
                    binWriter.Write((UInt32)2);

                    binWriter.Write((UInt16)00);
                    binWriter.Write((UInt16)DEFAULT_RUN);
                    binWriter.Write((UInt32)3);

                    binWriter.BaseStream.Seek(0x80, SeekOrigin.Begin);

                    binWriter.Write((UInt32)5);
                }
            }

            return new SubPacket(OPCODE, playerActorID, targetActorID, data);
        }

        public static SubPacket buildPacket(uint playerActorID, uint targetActorID, ushort stopSpeed, ushort walkSpeed, ushort runSpeed)
        {               
            byte[] data = new byte[PACKET_SIZE - 0x20];

            using (MemoryStream mem = new MemoryStream(data))
            {
                using (BinaryWriter binWriter = new BinaryWriter(mem))
                {
                    binWriter.Write((UInt16)00);
                    binWriter.Write((UInt16)stopSpeed);
                    binWriter.Write((UInt32)0);

                    binWriter.Write((UInt16)00);
                    binWriter.Write((UInt16)walkSpeed);
                    binWriter.Write((UInt32)1);

                    binWriter.Write((UInt16)00);
                    binWriter.Write((UInt16)runSpeed);
                    binWriter.Write((UInt32)2);

                    binWriter.Write((UInt16)00);
                    binWriter.Write((UInt16)runSpeed);
                    binWriter.Write((UInt32)3);

                    binWriter.BaseStream.Seek(0x90, SeekOrigin.Begin);

                    binWriter.Write((UInt32)5);
                }
            }

            return new SubPacket(OPCODE, playerActorID, targetActorID, data);
        }
    }
}
