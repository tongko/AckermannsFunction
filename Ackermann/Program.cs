using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace Ackermann
{
	class Program
	{
		private static byte _m;
		private static byte _n;
		private static bool _runTest;
		private static StreamWriter _output;
		private static bool _computeSingle;

		static void Main(string[] args)
		{
			if (!GetArguments(args))
				return;

			Console.WriteLine("Ackermann's Function Version {0} - Terry Liew (tongko.liew@gmail.com)\r\n",
				FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);
			if (_output != null)
				_output.WriteLine("Ackermann's Function Version {0} - Terry Liew (tongko.liew@gmail.com)\r\n",
				FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);

			if (_runTest)
				TestBigIntStack();
			else
			{
				if (_computeSingle)
				{
					var val = string.Format("Ackermann ({0}, {1}) is: {2}", _m - 1, _n - 1, Ackerman(_m - 1, _n - 1));
					Console.WriteLine(val);
					if (_output != null)
						_output.WriteLine(val);
				}
				else
					for (var i = 0; i < _m; i++)
					{
						for (var j = 0; j < _n; j++)
						{
							var val = string.Format("Ackermann ({0}, {1}) is: {2}", i, j, Ackerman(i, j));
							Console.WriteLine(val);
							if (_output != null)
								_output.WriteLine(val);
						}
					}
			}

			Console.Write("Press any key to exit . . .");
			Console.ReadKey(true);
		}

		static BigInteger Ackerman(BigInteger m, BigInteger n)
		{
			var stack = new BigIntStack();
			stack.Push(m);
			while (stack.Count > 0)
			{
				m = stack.Pop();
			Recurse:
				if (m == 0)
					n = n + 1;
				else if (m == 1)
					n = n + 2;
				else if (m == 2)
					n = 3 + 2 * n;
				else if (n == 0)
				{
					m--;
					n = 1;
					goto Recurse;
				}
				else
				{
					stack.Push(m - 1);
					n--;
					goto Recurse;
				}
			}

			stack.Dispose();
			return n;
		}

		static bool GetArguments(IList<string> args)
		{
			if (args == null || args.Count == 0)
			{
				PrintUsage();
				return false;
			}

			for (var i = 0; i < args.Count; i++)
			{
				var arg = args[i].ToLower();
				switch (arg[0])
				{
					case 't':
						if (!arg.Equals("test"))
						{
							PrintUsage();
							return false;
						}
						_runTest = true;
						break;
					case 'm':
						if (!(arg.Length > 2 && arg[1] == '=' && byte.TryParse(arg.Substring(2), out _m)))
						{
							PrintUsage();
							return false;
						}
						break;
					case 'n':
						if (!(arg.Length > 2 && arg[1] == '=' && byte.TryParse(arg.Substring(2), out _n)))
						{
							PrintUsage();
							return false;
						}
						break;
					case 'o':
						string temp = arg.Substring(2).Trim(new[] { '"' });
						var cki = new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false);
						if (File.Exists(temp))
						{
							do
							{
								Console.Write("\r\nOutput file already exists, overwrite ([y]/n)? ");
								cki = Console.ReadKey();
								Console.WriteLine();
							} while (cki.Key != ConsoleKey.Y && cki.Key != ConsoleKey.N);
						}

						if (cki.Key == ConsoleKey.Y)
							_output = new StreamWriter(temp, false, Encoding.UTF8);
						break;
					case 's':
						_computeSingle = true;
						break;
					default:
						PrintUsage();
						return false;
				}
			}

			return true;
		}

		static void PrintUsage()
		{
			Console.WriteLine(
@"
Usage: ack.exe [test] options

Where:
	test    Run BigIntStack tests.

	options:
	m=num   First number of Ackermann's function. This is ignored if [test] is specified.
	n=num   Second number of Ackermann's function. This is ignored if [test] is specified.
	s		Single computation, do not loop, compute m and n as is.
	o=file  Output file name.

Example:
	ack.exe m=5 n=6 o=""D:\Temp Folder\Result.txt""
");
		}

		static void TestBigIntStack()
		{
			var st = new BigIntStack();
			var num = new[] { 57, 64, 8992654378, long.MaxValue, 5, 791872 };
			foreach (var l in num)
			{
				st.Push(l);
			}

			for (var i = 0; i < 6; i++)
			{
				Console.WriteLine("{0}", st.Pop().ToString());
				if (_output != null)
					_output.WriteLine("{0}", st.Pop().ToString());
			}

			Console.ReadKey(true);
		}
	}
}
