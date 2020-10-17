﻿using System;
using System.IO;
using System.Text;

namespace Konata.Library.IO
{
    public class ByteBuffer
    {
        public enum Prefix
        {
            None = 0, // 前綴類型
            Uint8 = 1,
            Uint16 = 2,
            Uint32 = 4,

            LengthOnly = 64, // 只包括數據長度
            WithPrefix = 128,  // 包括數據長度和前綴長度
        }

        private static readonly IOException _exEob =
            new IOException("Insufficient buffer space.");
        private static int _minBufBase = 8;
        private static uint _minBufSize = 1U << _minBufBase;

        protected byte[] _buffer;
        protected uint _length;
        private uint _rPos;
        private uint _wPos;

        public ByteBuffer(byte[] data = null)
        {
            _buffer = null;
            _length = 0;
            _rPos = 0;
            _wPos = 0;
            if (data != null)
            {
                _length = (uint)data.Length;
                WriteData(data);
            }
        }

        #region PutMethods 放入數據 此方法組會增長緩衝區

        public void PutSbyte(sbyte value)
        {
            WriteData(ByteConverter.Int8ToBytes(value));
        }

        public void PutByte(byte value)
        {
            WriteData(ByteConverter.UInt8ToBytes(value));
        }

        /// <summary>
        /// 以 Big Endian 字节序, 放入 short 
        /// </summary>
        /// <param name="value">值</param>
        public void PutShortBE(short value)
        {
            WriteData(ByteConverter.Int16ToBytes(value, Endian.Big));
        }

        /// <summary>
        /// 以 Little Endian 字节序, 放入 short 
        /// </summary>
        /// <param name="value">值</param>
        public void PutShortLE(short value)
        {
            WriteData(ByteConverter.Int16ToBytes(value, Endian.Little));
        }

        /// <summary>
        /// 放入 short 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="endian">字节序</param>
        public void PutShort(short value, Endian endian)
        {
            WriteData(ByteConverter.Int16ToBytes(value, endian));
        }

        /// <summary>
        /// 以 Big Endian 字节序, 放入 ushort 
        /// </summary>
        /// <param name="value">值</param>
        public void PutUshortBE(ushort value)
        {
            WriteData(ByteConverter.UInt16ToBytes(value, Endian.Big));
        }

        /// <summary>
        /// 以 Little Endian 字节序, 放入 ushort 
        /// </summary>
        /// <param name="value">值</param>
        public void PutUshortLE(ushort value)
        {
            WriteData(ByteConverter.UInt16ToBytes(value, Endian.Little));
        }

        /// <summary>
        /// 放入 ushort 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="endian">字节序</param>
        public void PutUshort(ushort value, Endian endian)
        {
            WriteData(ByteConverter.UInt16ToBytes(value, endian));
        }

        /// <summary>
        /// 以 Big Endian 字节序, 放入 int 
        /// </summary>
        /// <param name="value">值</param>
        public void PutIntBE(int value)
        {
            WriteData(ByteConverter.Int32ToBytes(value, Endian.Big));
        }

        /// <summary>
        /// 以 Little Endian 字节序, 放入 int 
        /// </summary>
        /// <param name="value">值</param>
        public void PutIntLE(int value)
        {
            WriteData(ByteConverter.Int32ToBytes(value, Endian.Little));
        }

        /// <summary>
        /// 放入 int 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="endian">字节序</param>
        public void PutInt(int value, Endian endian)
        {
            WriteData(ByteConverter.Int32ToBytes(value, endian));
        }

        /// <summary>
        /// 以 Big Endian 字节序, 放入 uint 
        /// </summary>
        /// <param name="value">值</param>
        public void PutUintBE(uint value)
        {
            WriteData(ByteConverter.UInt32ToBytes(value, Endian.Big));
        }

        /// <summary>
        /// 以 Little Endian 字节序, 放入 uint 
        /// </summary>
        /// <param name="value">值</param>
        public void PutUintLE(uint value)
        {
            WriteData(ByteConverter.UInt32ToBytes(value, Endian.Little));
        }

        /// <summary>
        /// 放入 uint 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="endian">字节序</param>
        public void PutUint(uint value, Endian endian)
        {
            WriteData(ByteConverter.UInt32ToBytes(value, endian));
        }

        /// <summary>
        /// 以 Big Endian 字节序, 放入 long 
        /// </summary>
        /// <param name="value">值</param>
        public void PutLongBE(long value)
        {
            WriteData(ByteConverter.Int64ToBytes(value, Endian.Big));
        }

        /// <summary>
        /// 以 Little Endian 字节序, 放入 long 
        /// </summary>
        /// <param name="value">值</param>
        public void PutLongLE(long value)
        {
            WriteData(ByteConverter.Int64ToBytes(value, Endian.Little));
        }

        /// <summary>
        /// 放入 long 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="endian">字节序</param>
        public void PutLong(long value, Endian endian)
        {
            WriteData(ByteConverter.Int64ToBytes(value, endian));
        }

        /// <summary>
        /// 以 Big Endian 字节序, 放入 ulong 
        /// </summary>
        /// <param name="value">值</param>
        public void PutUlongBE(ulong value)
        {
            WriteData(ByteConverter.UInt64ToBytes(value, Endian.Big));
        }

        /// <summary>
        /// 以 Little Endian 字节序, 放入 ulong 
        /// </summary>
        /// <param name="value">值</param>
        public void PutUlongLE(ulong value)
        {
            WriteData(ByteConverter.UInt64ToBytes(value, Endian.Little));
        }

        /// <summary>
        /// 放入 ulong 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="endian">字节序</param>
        public void PutUlong(ulong value, Endian endian)
        {
            WriteData(ByteConverter.UInt64ToBytes(value, endian));
        }

        /// <summary>
        /// 以 Big Endian 字节序, 放入 float 
        /// </summary>
        /// <param name="value">值</param>
        public void PutFloatBE(float value)
        {
            WriteData(ByteConverter.SingleToBytes(value, Endian.Big));
        }

        /// <summary>
        /// 以 Little Endian 字节序, 放入 float 
        /// </summary>
        /// <param name="value">值</param>
        public void PutFloatLE(float value)
        {
            WriteData(ByteConverter.SingleToBytes(value, Endian.Little));
        }

        /// <summary>
        /// 放入 float 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="endian">字节序</param>
        public void PutFloat(float value, Endian endian)
        {
            WriteData(ByteConverter.SingleToBytes(value, endian));
        }

        /// <summary>
        /// 以 Big Endian 字节序, 放入 double 
        /// </summary>
        /// <param name="value">值</param>
        public void PutDoubleBE(double value)
        {
            WriteData(ByteConverter.DoubleToBytes(value, Endian.Big));
        }

        /// <summary>
        /// 以 Little Endian 字节序, 放入 double 
        /// </summary>
        /// <param name="value">值</param>
        public void PutDoubleLE(double value)
        {
            WriteData(ByteConverter.DoubleToBytes(value, Endian.Little));
        }

        /// <summary>
        /// 放入 double 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="endian">字节序</param>
        public void PutDouble(double value, Endian endian)
        {
            WriteData(ByteConverter.DoubleToBytes(value, endian));
        }

        /// <summary>
        /// 以 Big Endian 字节序, 放入 bool
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="length">占用长度</param>
        public void PutBoolBE(bool value, byte length)
        {
            WriteData(ByteConverter.BoolToBytes(value, length, Endian.Big));
        }

        /// <summary>
        /// 以 Little Endian 字节序, 放入 bool
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="length">占用长度</param>
        public void PutBoolLE(bool value, byte length)
        {
            WriteData(ByteConverter.BoolToBytes(value, length, Endian.Little));
        }

        /// <summary>
        /// 放入 bool 
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="endian">字节序</param>
        public void PutBool(bool value, byte length, Endian endian)
        {
            WriteData(ByteConverter.BoolToBytes(value, length, endian));
        }

        public void PutString(string value, Prefix prefixFlag = Prefix.None, byte limitedLength = 0)
        {
            PutBytes(Encoding.UTF8.GetBytes(value), prefixFlag, limitedLength); // 把字符串当作byte[]
        }

        public void PutBytes(byte[] value, Prefix prefixFlag = Prefix.None, byte limitedLength = 0)
        {
            prefixFlag &= (Prefix)7;
            bool limited = limitedLength > 0; // 是否限制长度
            byte[] array; // 处理后的数据
            if (limited) // 限制长度时，写入数据长度=前缀+限制
            {
                limitedLength = (byte)value.Length;
                array = new byte[(uint)prefixFlag + limitedLength];
                int len = value.Length > limitedLength ? limitedLength : value.Length;
                Buffer.BlockCopy(value, 0, array, (int)prefixFlag, len);
            }
            else if (prefixFlag > Prefix.None) // 不限制长度且有前缀时，写入数据长度=前缀+value长度
            {
                array = new byte[(uint)prefixFlag + value.Length];
                Buffer.BlockCopy(value, 0, array, (int)prefixFlag, value.Length);
            }
            else // 不限制又没有前缀，写入的就是value本身，不用处理，直接写入
            {
                WriteData(value);
                return;
            }
            if (prefixFlag > Prefix.None) // 添加前缀，使用大端序
            {
                if (!InsertPrefix(array, 0, (uint)value.Length, prefixFlag))
                {
                    throw new IOException("Given prefix length is too small for value bytes."); // 给定的prefix不够填充value.Length，终止写入
                }
            }
            WriteData(array);
        }

        public void PutEmpty(int count)
        {
            uint minLength = _wPos + (uint)count;
            ExtendBufferSize(minLength > _length ? minLength : _length);
            _wPos = minLength;
        }
        #endregion

        #region TakeMethods 拿走數據 此方法組會縮短緩衝區

        public sbyte TakeSbyte(out sbyte value)
        {
            if (CheckAvailable(1))
            {
                value = ByteConverter.BytesToInt8(_buffer, _rPos);
                ++_rPos;
                return value;
            }
            throw _exEob;
        }

        public byte TakeByte(out byte value)
        {
            if (CheckAvailable(1))
            {
                value = ByteConverter.BytesToUInt8(_buffer, _rPos);
                ++_rPos;
                return value;
            }
            throw _exEob;
        }

        public short TakeShortBE(out short value)
        {
            return TakeShort(out value, Endian.Big);
        }

        public short TakeShortLE(out short value)
        {
            return TakeShort(out value, Endian.Little);
        }

        public short TakeShort(out short value, Endian endian)
        {
            if (CheckAvailable(2))
            {
                value = ByteConverter.BytesToInt16(_buffer, _rPos, endian);
                _rPos += 2;
                return value;
            }
            throw _exEob;
        }

        public ushort TakeUshortBE(out ushort value)
        {
            return TakeUshort(out value, Endian.Big);
        }

        public ushort TakeUshortLE(out ushort value)
        {
            return TakeUshort(out value, Endian.Little);
        }

        public ushort TakeUshort(out ushort value, Endian endian)
        {
            if (CheckAvailable(2))
            {
                value = ByteConverter.BytesToUInt16(_buffer, _rPos, endian);
                _rPos += 2;
                return value;
            }
            throw _exEob;
        }

        public int TakeIntBE(out int value)
        {
            return TakeInt(out value, Endian.Big);
        }

        public int TakeIntLE(out int value)
        {
            return TakeInt(out value, Endian.Little);
        }

        public int TakeInt(out int value, Endian endian)
        {
            if (CheckAvailable(4))
            {
                value = ByteConverter.BytesToInt32(_buffer, _rPos, endian);
                _rPos += 4;
                return value;
            }
            throw _exEob;
        }

        public uint TakeUintBE(out uint value)
        {
            return TakeUint(out value, Endian.Big);
        }

        public uint TakeUintLE(out uint value)
        {
            return TakeUint(out value, Endian.Little);
        }

        public uint TakeUint(out uint value, Endian endian)
        {
            if (CheckAvailable(4))
            {
                value = ByteConverter.BytesToUInt32(_buffer, _rPos, endian);
                _rPos += 4;
                return value;
            }
            throw _exEob;
        }

        public long TakeLongBE(out long value)
        {
            return TakeLong(out value, Endian.Big);
        }

        public long TakeLongLE(out long value)
        {
            return TakeLong(out value, Endian.Little);
        }

        public long TakeLong(out long value, Endian endian)
        {
            if (CheckAvailable(8))
            {
                value = ByteConverter.BytesToInt64(_buffer, _rPos, endian);
                _rPos += 8;
                return value;
            }
            throw _exEob;
        }

        public ulong TakeUlongBE(out ulong value)
        {
            return TakeUlong(out value, Endian.Big);
        }

        public ulong TakeUlongLE(out ulong value)
        {
            return TakeUlong(out value, Endian.Little);
        }

        public ulong TakeUlong(out ulong value, Endian endian)
        {
            if (CheckAvailable(8))
            {
                value = ByteConverter.BytesToUInt64(_buffer, _rPos, endian);
                _rPos += 8;
                return value;
            }
            throw _exEob;
        }

        public float TakeFloatBE(out float value)
        {
            return TakeFloat(out value, Endian.Big);
        }

        public float TakeFloatLE(out float value)
        {
            return TakeFloat(out value, Endian.Little);
        }

        public float TakeFloat(out float value, Endian endian)
        {
            if (CheckAvailable(4))
            {
                value = ByteConverter.BytesToSingle(_buffer, _rPos, endian);
                _rPos += 4;
                return value;
            }
            throw _exEob;
        }

        public double TakeDoubleBE(out double value)
        {
            return TakeDouble(out value, Endian.Big);
        }

        public double TakeDoubleLE(out double value)
        {
            return TakeDouble(out value, Endian.Little);
        }

        public double TakeDouble(out double value, Endian endian)
        {
            if (CheckAvailable(8))
            {
                value = ByteConverter.BytesToDouble(_buffer, _rPos, endian);
                _rPos += 8;
                return value;
            }
            throw _exEob;
        }

        public bool TakeBoolBE(out bool value, byte length)
        {
            return TakeBool(out value, length, Endian.Big);
        }

        public bool TakeBoolLE(out bool value, byte length)
        {
            return TakeBool(out value, length, Endian.Little);
        }

        public bool TakeBool(out bool value, byte length, Endian endian)
        {
            if (CheckAvailable(length))
            {
                value = ByteConverter.BytesToBool(_buffer, _rPos, length, endian);
                _rPos += length;
                return value;
            }
            throw _exEob;
        }

        public string TakeString(out string value, Prefix prefixFlag)
        {
            return value = Encoding.UTF8.GetString(TakeBytes(out byte[] _, prefixFlag));
        }

        public byte[] TakeBytes(out byte[] value, Prefix prefixFlag)
        {
            uint length;
            bool reduce = (prefixFlag & Prefix.WithPrefix) > 0;
            uint preLen = ((uint)prefixFlag) & 15;
            switch (preLen)
            {
            case 0: // Read to end.
                length = _length - _rPos;
                break;
            case 1:
            case 2:
            case 4:
                if (CheckAvailable(preLen))
                {
                    length = preLen == 1 ? ByteConverter.BytesToUInt8(_buffer, _rPos) :
                             preLen == 2 ? ByteConverter.BytesToUInt16(_buffer, _rPos, Endian.Big) :
                                           ByteConverter.BytesToUInt32(_buffer, _rPos, Endian.Big);
                    _rPos += preLen;
                    if (reduce)
                    {
                        if (length < preLen)
                        {
                            throw new IOException("Data length is less than prefix length.");
                        }
                        length -= preLen;
                    }
                    break;
                }
                throw _exEob;
            default:
                throw new ArgumentOutOfRangeException("Invalid prefix flag.");
            }
            if (CheckAvailable(length))
            {
                value = new byte[length];
                Buffer.BlockCopy(_buffer, (int)_rPos, value, 0, (int)length);
                _rPos += length;
                return value;
            }
            throw _exEob;
        }

        public byte[] TakeBytes(out byte[] value, uint length)
        {
            if (CheckAvailable(length))
            {
                value = new byte[length];
                Buffer.BlockCopy(_buffer, (int)_rPos, value, 0, (int)length);
                _rPos += length;
                return value;
            }
            throw _exEob;
        }

        public byte[] TakeAllBytes(out byte[] value)
        {
            value = new byte[_length - _rPos];
            Buffer.BlockCopy(_buffer, (int)_rPos, value, 0, value.Length);
            _rPos = _length;
            return value;
        }

        /// <summary>
        /// 吃掉數據 φ(゜▽゜*)♪
        /// </summary>
        /// <param name="length"></param>
        public void EatBytes(uint length)
        {
            _rPos += length;
        }

        #endregion

        #region GetMethods 獲取數據 此方法組不會對緩衝區造成影響

        /// <summary>
        /// 獲取打包數據
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            if (_length > 0)
            {
                var data = new byte[_length];
                Buffer.BlockCopy(_buffer, 0, data, 0, (int)_length);
                return data;
            }
            return new byte[0];
        }

        /// <summary>
        /// 到字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return BitConverter.ToString(GetBytes()).Replace("-", " ");
        }

        #endregion

        public uint Length
        {
            get { return _length; }
        }

        public uint RemainLength
        {
            get { return _length - _rPos; }
        }

        public static uint MinBufferBase
        {
            get
            {
                return (uint)_minBufBase;
            }
            set
            {
                if (value > 30)
                {
                    throw new IOException("Minimum buffer base out of range: 0 <= MinBufferBase <= 30.");
                }
                _minBufBase = (int)value;
                _minBufSize = 1U << _minBufBase;
            }
        }

        public static uint MinBufferSize
        {
            get
            {
                return _minBufSize;
            }
            set
            {
                if (value == 0 || value > 0x40000000)
                {
                    throw new IOException("Minimum buffer size out of range: 0 < MinBufferSize <= 0x40000000.");
                }
                int b = 0;
                uint s = 1;
                while (s < value)
                {
                    ++b;
                    s <<= 1;
                }
                _minBufBase = b;
                _minBufSize = s;
            }
        }

        private void ExtendBufferSize(uint minLength)
        {
            uint size = minLength >> _minBufBase;
            if ((minLength & (_minBufSize - 1)) > 0)
            {
                ++size;
            }
            size <<= _minBufBase;
            if (_buffer == null)
            {
                _buffer = new byte[size];
            }
            else if (_buffer.Length < size)
            {
                Array.Resize(ref _buffer, (int)size);
            }
            if (_length < minLength)
            {
                _length = minLength;
            }
        }

        /// <summary>
        /// 寫入數據
        /// </summary>
        /// <param name="data">數據</param>
        protected void WriteData(byte[] data)
        {
            uint minLength = _wPos + (uint)data.Length;
            ExtendBufferSize(minLength > _length ? minLength : _length);
            Buffer.BlockCopy(data, 0, _buffer, (int)_wPos, data.Length);
            _wPos = minLength;
        }

        protected bool CheckAvailable(uint length = 0)
        {
            return _rPos + length <= _length;
        }

        protected static bool InsertPrefix(byte[] buffer, uint offset, uint value, Prefix prefixFlag, Endian endian = Endian.Big)
        {
            switch (prefixFlag)
            {
            case Prefix.Uint8:
                if (value <= byte.MaxValue)
                {
                    Buffer.BlockCopy(ByteConverter.UInt8ToBytes((byte)value), 0, buffer, (int)offset, 1);
                    return true;
                }
                break;
            case Prefix.Uint16:
                if (value <= ushort.MaxValue)
                {
                    Buffer.BlockCopy(ByteConverter.UInt16ToBytes((ushort)value, endian), 0, buffer, (int)offset, 2);
                    return true;
                }
                break;
            case Prefix.Uint32:
                Buffer.BlockCopy(ByteConverter.UInt32ToBytes(value, endian), 0, buffer, (int)offset, 4);
                return true;
            }
            return false;
        }

        #region Operators

        public static ByteBuffer operator +(ByteBuffer a, ByteBuffer b)
        {
            var buffer = new ByteBuffer();
            buffer.PutBytes(a.GetBytes());
            buffer.PutBytes(b.GetBytes());
            return buffer;
        }

        public static ByteBuffer operator +(byte[] a, ByteBuffer b)
        {
            var buffer = new ByteBuffer();
            buffer.PutBytes(a);
            buffer.PutBytes(b.GetBytes());
            return buffer;
        }

        public static ByteBuffer operator +(ByteBuffer a, byte[] b)
        {
            var buffer = new ByteBuffer();
            buffer.PutBytes(a.GetBytes());
            buffer.PutBytes(b);
            return buffer;
        }

        #endregion
    }
}
