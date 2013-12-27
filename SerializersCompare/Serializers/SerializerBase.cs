using System.IO;
using System.Text;

namespace SerializersCompare.Serializers
{
    public abstract class SerializerBase<T> : ITestSerializers<T>
    {
        protected bool SerSaved = false;
        protected bool DeserSaved = false;
        protected bool IsBinarySerializer = true;
        protected string SerName;
        protected string IODir = "IOData";

        public SerializerBase()
        {
            Directory.CreateDirectory(IODir);
        }

        public string Name()
        {
            return SerName;
        }

        public bool IsBinary()
        {
            return IsBinarySerializer;
        }

        protected byte[] SerBytes
        {
            get { return _serBytes; }
            set
            {
                _serBytes = value; 
                SaveSerializedData();
            }
        }

        protected string SerString
        {
            get { return _serString; }
            set
            {
                _serString = value;
                SaveSerializedData();
            }
        }
        protected string DeserString;

        private byte[] _serBytes;
        private string _serString;

        public abstract dynamic Serialize(object thisObj);

        public abstract T Deserialize(dynamic serInput);

        protected void SaveSerializedData()
        {
            if (SerSaved)
                return;

            var ext = IsBinarySerializer ? ".bin" : ".txt";

            var fileName = IODir + "\\" + Name() + ".ser" + ext;
            using (var fs = File.OpenWrite(fileName))
            {
                byte[] bytes;
                if (IsBinarySerializer)
                {
                    bytes = _serBytes;
                }
                else
                {
                    bytes = Encoding.UTF8.GetBytes(_serString);
                }
                fs.Write(bytes, 0, bytes.Length);
            }

            SerSaved = true;
        }

        protected void SaveDeserializedData()
        {
            if (DeserSaved)
                return;

            var ext = IsBinarySerializer ? ".bin" : ".txt";
            var fileName = Name() + ".deser" + ext;
            using (var fs = File.OpenWrite(fileName))
            {
                var bytes = Encoding.UTF8.GetBytes(DeserString);
                fs.Write(bytes, 0, bytes.Length);
            }

            DeserSaved = true;
        }
    }
}
