using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Ackermann
{
	class BigMemoryMappedFile : IDisposable
	{
		private const int MappedSize = 1073741824;	//	1G
		private readonly Stack<MappedFileInfo> _mapped;
		private int _mappedIndex;
		private int _mappedPosition;
		private MemoryMappedViewStream _stream;

		public BigMemoryMappedFile()
		{
			_mapped = new Stack<MappedFileInfo>();
			_mappedIndex = 0;
			_mappedPosition = 0;
			_mapped.Push(CreateNewMappedFile(_mappedIndex));
		}

		public long Position { get; set; }

		public long Length { get; set; }

		public MappedFileInfo CurrentMapped { get { return _mapped.Peek(); } }

		public void Read(byte[] buffer, int offset, int count)
		{

		}

		public void Write(byte[] buffer, int offset, int count)
		{
			var mapped = _mapped.Peek();

		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				while (_mapped.Count > 0)
				{
					var m = _mapped.Pop();
					m.Dispose();
				}
			}
		}

		private void EnsureBufferAvailable()
		{
		}

		private MappedFileInfo CreateNewMappedFile(int index)
		{
			var mi = new MappedFileInfo { MappedFileName = Path.GetTempFileName(), MappedFileIndex = index };
			mi.MappedFile = MemoryMappedFile.CreateFromFile(mi.MappedFileName, FileMode.CreateNew, "F" + index, MappedSize);

			return mi;
		}
	}

	class MappedFileInfo : IDisposable
	{
		public string MappedFileName { get; set; }
		public MemoryMappedFile MappedFile { get; set; }
		public int MappedFileIndex { get; set; }
		public long Position { get; set; }

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (MappedFile != null)
					MappedFile.Dispose();
				if (File.Exists(MappedFileName))
					File.Delete(MappedFileName);

				GC.SuppressFinalize(this);
			}
		}

		public MemoryMappedViewStream GetViewStream()
		{
			return MappedFile.CreateViewStream(Position, 0);
		}
	}
}
