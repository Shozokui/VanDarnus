using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Meteor.Common;
using System.IO;

namespace Launcher.Util
{
    public class Launch
    {
        const uint g_encryptionTimePatchAddress = 0x9A15E3;
        const uint g_lobbyHostNameAddress = 0xB90110;
        byte[] g_encryptionTimePatch = { 0xB8, 0x12, 0xE8, 0xE0, 0x50 };

        public string SessionID { get; set; }

        byte[] Key(int tick) => Encoding.ASCII.GetBytes(string.Format("{0:X8}", tick & ~0xFFFF));
        string Args (int tick) => string.Format(" T ={0} /LANG =en-us /REGION =2 /SERVER_UTC =1356916742 /SESSION_ID ={1}", tick, SessionID);


        public Launch(string sessionID)
        {
            SessionID = sessionID;
        }

        public void GameStart()
        {
            var tick = Environment.TickCount;
            var args = Args(tick);
            var blowfish = new Blowfish(Key(tick));

            var len = args.Length + 1;
            var bytes = Encoding.ASCII.GetBytes(args);
            for (var i = 0; i < (len & ~0x7); i += 8)
            {
                blowfish.Encipher(bytes, i, 8);
            }

            var base64str = Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_');

            var baseGamePath = @"D:\Games\SquareEnix\FINAL FANTASY XIV\ffxivgame.exe";
            var fullProcessPath = $"{baseGamePath} sqex0002${base64str}!////";


            STARTUPINFO si = new STARTUPINFO();
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            bool success = Native.CreateProcess(null, fullProcessPath, IntPtr.Zero, IntPtr.Zero, false, ProcessCreationFlags.CREATE_SUSPENDED, IntPtr.Zero, @"D:\Games\SquareEnix\FINAL FANTASY XIV", ref si, out pi);
            
            if (success)
            {
                ApplyPatches(pi);
            }

        }

        private void Patch(PROCESS_INFORMATION proc, IntPtr address, byte[] patch, uint size)
        {
            uint oldProtect = 0;

            if (!Native.VirtualProtectEx(proc.hProcess, address, size, 0x04, out oldProtect))
            {
                throw new Exception("Failed to change page protection.");
            }

            IntPtr bytesWritten;

            if (!Native.WriteProcessMemory(proc.hProcess, address, patch, size, out bytesWritten))
            {
                throw new Exception("Failed to apply patch");
            }

            if (bytesWritten == IntPtr.Zero || bytesWritten.ToInt32() != size) 
            {
                throw new Exception("Failed to apply patch");
            }

            if (!Native.VirtualProtectEx(proc.hProcess, address, size, oldProtect, out oldProtect))
            {
                throw new Exception("Failed to change page protection.");
            }
        }

        private void ApplyPatches(PROCESS_INFORMATION proc)
        {
            CONTEXT threadContext = new CONTEXT();
            threadContext.ContextFlags = (uint)CONTEXT_FLAGS.CONTEXT_FULL;
            if (!Native.GetThreadContext(proc.hThread, ref threadContext))
            {
                throw new Exception("Failed to get thread context.");
            }

            IntPtr imageBaseAddressPtr = new IntPtr(threadContext.Ebx + 8);
            IntPtr imageBaseAddress = new nint(0);

            IntPtr numRead = new nint(0);
            
            if (!Native.ReadProcessMemory(proc.hProcess, imageBaseAddressPtr, imageBaseAddress, 4, out numRead))
            {
                throw new Exception("Failed to get image base context.");
            }

            Patch(proc, new IntPtr(imageBaseAddress.ToInt32() + g_encryptionTimePatchAddress), g_encryptionTimePatch, sizeof(byte) * (uint)g_encryptionTimePatch.Length);
            Patch(proc, new nint(imageBaseAddress.ToInt32() + g_lobbyHostNameAddress), Encoding.ASCII.GetBytes("localhost"), (uint)("localhost".Length + 1));
        }
    }
}
