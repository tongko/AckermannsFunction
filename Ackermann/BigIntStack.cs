using System;
using System.IO;
using System.Numerics;

namespace Ackermann
{
	class BigIntStack
	{
		private readonly FileStream _stream;
		private readonly LinkedStack<long> _index;

		public BigIntStack()
		{
			_stream = new FileStream(Path.GetTempFileName(), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			_index = new LinkedStack<long>();
		}

		public int Count { get { return _index.Count; } }

		public void Push(BigInteger item)
		{
			_index.Push(_stream.Position);
			var buffer = item.ToByteArray();
			var index = 0;
			do
			{
				var c = buffer.Length - index > 4096 ? 4096 : buffer.Length;
				_stream.Write(buffer, index, c);
				index += c;
			} while (index < buffer.Length);
		}

		public BigInteger Pop()
		{
			var pos = _index.Pop();
			_stream.Position = pos;
			var len = _stream.Length - pos;
			var buffer = new byte[len];
			var index = 0;

			do
			{
				var temp = new byte[4096];
				var c = _stream.Read(temp, 0, 4096);
				if (c == 0) break;
				Array.Copy(temp, 0, buffer, index, c);
				index += c;
			} while (true);

			_stream.SetLength(pos);
			return new BigInteger(buffer);
		}
	}
}
