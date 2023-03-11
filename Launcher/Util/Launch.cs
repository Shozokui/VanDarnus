using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meteor.Common;

namespace Launcher.Util
{
    public class Launch
    {
        public string SessionID { get; set; }

        byte[] Key(int tick) => Encoding.ASCII.GetBytes(string.Format("{0:X8}", tick & ~0xFFFF));
        string Args (int tick) => string.Format(" T ={0} /LANG =en-us /REGION =2 /SERVER_UTC =1356916742 /SESSION_ID ={1}", tick, SessionID);


        public Launch(string sessionID)
        {
            SessionID = sessionID;
        }

        public string CommandLineArgs
        {
            get
            {
                var tick = Environment.TickCount;
                var args = Args(tick);
                var blowfish = new Blowfish(Key(tick));

                var len = args.Length + 1;
                var bytes = Encoding.ASCII.GetBytes(args);
                for (var i = 0; i < (len & ~0x7); i+=8)
                {
                    blowfish.Encipher(bytes, i, 4);
                }

                return "";
            }
        }
    }
}
