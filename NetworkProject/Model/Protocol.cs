using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NetworkProject.Model
{
    public enum ClientType
    {
        CONNECT, DISCONNECT, CHAT, IMAGE
    }

    [Serializable]
    public class ClientProtocol
    {
        public ClientType Type { get; set; }
        public byte[] Data { get; set; }
    }

    public enum ServerType
    {
        ACCEPT, REJECT, CHAT, IMAGE
    }

    [Serializable]
    public class ServerProtocol
    {
        public ServerType Type { get; set; }
        public byte[] Data { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }

        public override string ToString()
        {
            return Username;
        }
    }

    public class Serde
    {
        public static byte[] Serialize(object obj)
        {
            try
            {
                using (MemoryStream stream = new())
                {
                    BinaryFormatter formatter = new();
                    formatter.Serialize(stream, obj);
                    return stream.ToArray();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return null;
        }

        public static object Deserialize(byte[] buffer)
        {
            try
            {
                using (MemoryStream stream = new(buffer))
                {
                    BinaryFormatter formatter = new();
                    stream.Position = 0;
                    return formatter.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return null;
        }
    }
}
