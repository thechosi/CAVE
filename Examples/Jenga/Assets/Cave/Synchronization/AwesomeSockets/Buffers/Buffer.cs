using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AwesomeSockets.Buffers
{
    public class Buffer
    {
        private const int DefaultBufferSize = 1024;
        private int _bufferSize;
        private byte[] _bytes;
        private MemoryStream _memoryStream;
        private BinaryWriter _binaryWriter;
        private BinaryReader _binaryReader;
        private bool _finalized;

        private Buffer(int bufferSize)
        {
            _bufferSize = bufferSize;
            _bytes = new byte[bufferSize];
            _memoryStream = new MemoryStream(_bytes);
            _binaryWriter = new BinaryWriter(_memoryStream);
            _binaryReader = new BinaryReader(_memoryStream);
            _finalized = false;
        }

        public static int GetSize(Buffer buffer)
        {
            return buffer._bufferSize;
        }

        public static long GetPosition(Buffer buffer)
        {
            return buffer._memoryStream.Position;
        }

        public static void Forward(Buffer buffer, long newPosition)
        {
            buffer._memoryStream.Position += newPosition;
        }

        #region public operation methods
        public static Buffer New()
        {
            return New(DefaultBufferSize);
        }

        public static Buffer New(int bufferSize)
        {
            return new Buffer(bufferSize);
        }

        public static void Resize(Buffer buffer, int size)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            buffer.Resize(size);
        }

        public static void ClearBuffer(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            buffer.ClearBuffer();
        }

        public static void FinalizeBuffer(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            buffer.FinalizeBuffer();
        }

        public static long GetWrittenBytes(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return buffer._memoryStream.Position;
        }

        public static void Add(Buffer buffer, Buffer bufferToWrite)
        {
            if (buffer == null || bufferToWrite == null) throw new ArgumentNullException("buffer");
            buffer.ClearBuffer();
            buffer = New();
            buffer.Add(bufferToWrite._bytes);
        }

        public static void Add(Buffer buffer, byte[] byteArray)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (byteArray.Length == 0) throw new InvalidOperationException("Cannot provide a zero-length array");
            buffer.ClearBuffer();
            buffer.Add(byteArray);
            buffer.FinalizeBuffer();
        }

        public static void Add(Buffer buffer, object value)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (buffer._finalized) throw new InvalidOperationException("Buffer provided is in 'finalized' state. You must call 'ClearBuffer()' to reset it.");
            buffer.Add(value);
        }

        public static T Get<T>(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (typeof(T) == typeof(bool)) return (T)(object)buffer.GetBoolean();
            if (typeof(T) == typeof(byte)) return (T)(object)buffer.GetByte();
            if (typeof(T) == typeof(sbyte)) return (T)(object)buffer.GetSByte();
            if (typeof(T) == typeof(char)) return (T)(object)buffer.GetChar();
            if (typeof(T) == typeof(double)) return (T)(object)buffer.GetDouble();
            if (typeof(T) == typeof(float)) return (T)(object)buffer.GetFloat();
            if (typeof(T) == typeof(int)) return (T)(object)buffer.GetInt();
            if (typeof(T) == typeof(uint)) return (T)(object)buffer.GetUInt();
            if (typeof(T) == typeof(long)) return (T)(object)buffer.GetLong();
            if (typeof(T) == typeof(ulong)) return (T)(object)buffer.GetULong();
            if (typeof(T) == typeof(short)) return (T)(object)buffer.GetShort();
            if (typeof(T) == typeof(ushort)) return (T)(object)buffer.GetUShort();
            throw new InvalidOperationException(string.Format("Provided type ({0}) cannot be deserialized from buffer. You must provide a (except struct and enum) or a string", typeof(T)));
        }

        public static byte[] GetBuffer(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (!buffer._finalized) throw new InvalidOperationException("Buffer provided is not in 'finalized' state. You must call 'FinalizeBuffer()' in order to get the full buffer");
            return buffer.GetBuffer();
        }

        public static byte[] GetBufferRef(Buffer buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return buffer.GetBuffer();
        }

        //public static void EncryptBuffer(Buffer buffer, string encryptionKey, string initVector, int keySize = (int)KeySizes.TwoFiftySix, int ivSize = (int)KeySizes.OneTwenyEight)
        //{
        //    var plaintextBytes = buffer.bytes;
        //    var rijndaelEncryptor = buffer.CreateRijndael(encryptionKey, initVector, keySize, ivSize).CreateEncryptor();

        //    MemoryStream mStream = new MemoryStream();
        //    CryptoStream cStream = new CryptoStream(mStream, rijndaelEncryptor, CryptoStreamMode.Write);

        //    cStream.Write(plaintextBytes, 0, plaintextBytes.Length);
        //    cStream.FlushFinalBlock();

        //    //buffer.SaveNullPosition();
        //    Add(buffer, mStream.ToArray());

        //    mStream.Close();
        //    cStream.Close();
        //}

        //public static void DecryptBuffer(Buffer buffer, string decryptionKey, string initVector, int keySize = (int)KeySizes.TwoFiftySix, int ivSize = (int)KeySizes.OneTwenyEight)
        //{
        //    var cipherBytes = buffer.bytes;
        //    var rijndaelDecryptor = buffer.CreateRijndael(decryptionKey, initVector, keySize, ivSize).CreateDecryptor();

        //    MemoryStream mStream = new MemoryStream(cipherBytes);
        //    CryptoStream cStream = new CryptoStream(mStream, rijndaelDecryptor, CryptoStreamMode.Read);

        //    var plaintextBytes = new byte[cipherBytes.Length];

        //    cStream.Read(plaintextBytes, 0, cipherBytes.Length);

        //    Add(buffer, plaintextBytes);
        //    //buffer.ApplyNullPosition();

        //    mStream.Close();
        //    cStream.Close();
        //}


        //Here for converting doubles and floats...
        public static void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count)
        {
            System.Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
        }

        public override bool Equals(object obj)
        {
            Buffer other = (Buffer)obj;
            return ((_bufferSize == other._bufferSize) &&
                    (_bytes.Equals(other._bytes)) &&
                    (_binaryWriter.BaseStream.Position == other._binaryWriter.BaseStream.Position) &&
                    (_finalized == other._finalized));
        }

        public override int GetHashCode()
        {
            return _bytes.Aggregate((x, y) => Convert.ToByte(x ^ y ^ _bufferSize ^ _binaryWriter.BaseStream.Position ^ (_finalized ? 1 : 0)));
        }
        #endregion

        #region private operation methods
        private byte[] GetBuffer()
        {
            return _bytes;
        }

        private void Resize(int newSize)
        {
            if (newSize > _bytes.Length)
            {
                Array.Resize(ref _bytes, newSize);
                _memoryStream = new MemoryStream(_bytes);
                _binaryWriter = new BinaryWriter(_memoryStream);
                _binaryReader = new BinaryReader(_memoryStream);
            }
            _bufferSize = newSize;
            ClearBuffer();
        }

        private void ClearBuffer()
        {
            _memoryStream.Position = 0;
            _finalized = false;
        }

        private void FinalizeBuffer()
        {
            _finalized = true;
        }

        private void Add(byte[] byteArray)
        {
            _binaryWriter.Write(byteArray);
        }

        private void Add(object primitive)
        {
            if (primitive == null) throw new ArgumentNullException("primitive");
            ConvertToByteArray(primitive);
        }

        private void ConvertToByteArray(object primitive)
        {
            if (primitive is bool) _binaryWriter.Write((bool)primitive);
            else if (primitive is byte) _binaryWriter.Write((byte)primitive);
            else if (primitive is sbyte) _binaryWriter.Write((sbyte)primitive);
            else if (primitive is char) _binaryWriter.Write((char)primitive);
            else if (primitive is double) _binaryWriter.Write((double)primitive);
            else if (primitive is float) _binaryWriter.Write((float)primitive);
            else if (primitive is int) _binaryWriter.Write((int)primitive);
            else if (primitive is uint) _binaryWriter.Write((uint)primitive);
            else if (primitive is long) _binaryWriter.Write((long)primitive);
            else if (primitive is ulong) _binaryWriter.Write((ulong)primitive);
            else if (primitive is short) _binaryWriter.Write((short)primitive);
            else if (primitive is ushort) _binaryWriter.Write((ushort)primitive);
            //if (primitive is string)
            //{
            //    var str = primitive as string;
            //    if (str.Contains("\0")) throw new InvalidOperationException("String cannot contain null character '\\0'");
            //    var termStr = string.Format("{0}{1}", (string) primitive, "\0");   //The '\0' is to null-terminate the string so we can deserialize it on the other end as strings aren't fixed in size
            //    byte[] bytes = new byte[termStr.Length * sizeof(char)];
            //    System.Buffer.BlockCopy(termStr.ToCharArray(), 0, bytes, 0, bytes.Length);
            //    return bytes;
            //}
            else
                throw new InvalidOperationException("Provided type cannot be serialized for transmission. You must provide a value type (except enum and struct) or a string");
        }
        #endregion

        #region private getters
        private bool GetBoolean()
        {
            if (!CheckBufferBoundaries(sizeof(bool))) throw new InvalidOperationException("Failed to get bool, reached end of buffer.");
            return _binaryReader.ReadBoolean();
        }

        private byte GetByte()
        {
            if (!CheckBufferBoundaries(sizeof(byte))) throw new InvalidOperationException("Failed to get byte, reached end of buffer.");
            return _binaryReader.ReadByte();
        }

        private sbyte GetSByte()
        {
            if (!CheckBufferBoundaries(sizeof(sbyte))) throw new InvalidOperationException("Failed to get sbyte, reached end of buffer.");
            return (sbyte)_binaryReader.ReadByte();
        }

        private char GetChar()
        {
            if (!CheckBufferBoundaries(sizeof(char))) throw new InvalidOperationException("Failed to get char, reached end of buffer.");
            return _binaryReader.ReadChar();
        }

        private double GetDouble()
        {
            if (!CheckBufferBoundaries(sizeof(double))) throw new InvalidOperationException("Failed to get double, reached end of buffer.");
            return _binaryReader.ReadDouble();
        }

        private float GetFloat()
        {
            if (!CheckBufferBoundaries(sizeof(float)))
                throw new InvalidOperationException("Failed to get float, reached end of buffer.");
            return _binaryReader.ReadSingle();
        }

        private int GetInt()
        {
            if (!CheckBufferBoundaries(sizeof(int)))
                throw new InvalidOperationException("Failed to get int, reached end of buffer.");
            return _binaryReader.ReadInt32();
        }

        private uint GetUInt()
        {
            if (!CheckBufferBoundaries(sizeof(uint)))
                throw new InvalidOperationException("Failed to get uint, reached end of buffer.");
            return _binaryReader.ReadUInt32();
        }

        private long GetLong()
        {
            if (!CheckBufferBoundaries(sizeof(long))) throw new InvalidOperationException("Failed to get long, reached end of buffer.");
            return _binaryReader.ReadInt64();
        }

        private ulong GetULong()
        {
            if (!CheckBufferBoundaries(sizeof(ulong))) throw new InvalidOperationException("Failed to get ulong, reached end of buffer.");
            return _binaryReader.ReadUInt64();
        }

        private short GetShort()
        {
            if (!CheckBufferBoundaries(sizeof(short))) throw new InvalidOperationException("Failed to get short, reached end of buffer.");
            return _binaryReader.ReadInt16();
        }

        private ushort GetUShort()
        {
            if (!CheckBufferBoundaries(sizeof(ushort))) throw new InvalidOperationException("Failed to get short, reached end of buffer.");
            return _binaryReader.ReadUInt16();
        }

        //private string GetString()
        //{
        //    var localPosition = -1;
        //    var startPosition = _position;
        //    for (var i = _position; i <= _bufferSize; i += sizeof(char))
        //    {
        //        var test = GetChar();
        //        if (test != '\0') continue;
        //        localPosition = i;
        //        break;
        //    }
        //    _position = startPosition;

        //    if (localPosition == -1) throw new InvalidOperationException("Failed to get string, reached end of buffer.");

        //    var chars = new char[(localPosition - _position) / sizeof(char)];
        //    System.Buffer.BlockCopy(_bytes, _position, chars, 0, localPosition - _position);
        //    _position = localPosition += sizeof(char);
        //    return new string(chars);
        //}
        #endregion

        #region private boundary checks
        private bool CheckBufferBoundaries(byte[] bytesToCheck)
        {
            var roomLeft = _bytes.Length - _memoryStream.Position;
            return roomLeft >= bytesToCheck.Length;
        }

        private bool CheckBufferBoundaries(int numberOfBytes)
        {
            var roomLeft = _bytes.Length - _memoryStream.Position;
            return roomLeft >= numberOfBytes;
        }
        #endregion

        #region private misc methods
//        private static byte[] CreateCryptoKeyFromString(string stringToConvert, int keySize)
//        {
//            var initialConvert = ConvertToByteArray(stringToConvert);
//
//            byte[] sanitizedConvert;
//
//            if (initialConvert.Length > keySize)    //Truncate if greater than 256
//                sanitizedConvert = initialConvert.Take(keySize).ToArray();
//            else    //fill with repeats until 256
//                RotateBytes(initialConvert, keySize, out sanitizedConvert);
//
//            return sanitizedConvert;
//        }

        private static void RotateBytes(byte[] source, int keySize, out byte[] destination)
        {
            var localPostion = 0;
            destination = new byte[keySize / 8];
            do
            {
                foreach (var b in source)
                {
                    if (localPostion < destination.Length)
                        destination[localPostion] = b;
                    else
                        return;     //We have reached the end of the destination array and can short-circuit here
                    localPostion += 1;
                }
            } while (localPostion < destination.Length);
        }

//        private RijndaelManaged CreateRijndael(string encryptKey, string initVector, int keySize, int vectorSize)
//        {
//            return new RijndaelManaged
//            {
//                KeySize = keySize,
//                Key = CreateCryptoKeyFromString(encryptKey, keySize),
//                IV = CreateCryptoKeyFromString(initVector, vectorSize),
//                Padding = PaddingMode.None
//            };
//        }
        #endregion
    }
}