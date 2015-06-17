using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Ackermann
{
	class Program
	{
		private static int _m;
		private static int _n;
		private static TextWriter _oldOut;
		private static StreamWriter _newOut;
		private static bool _runTest;

		static void Main(string[] args)
		{
			if (!GetArguments(args))
				return;

			if (_runTest)
				TestBigIntStack();
			else
			{
				for (int i = 0; i < _m; i++)
				{
					for (int j = 0; j < _n; j++)
					{
						Console.WriteLine("Ackermann ({0}, {1}) is: {2}", i, j, Ackerman(i, j));
					}
				}

				if (_newOut != null)
					_newOut.Close();
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

			return n;
		}

		static bool GetArguments(IList<string> args)
		{
			if (args == null || args.Count == 0)
			{
				PrintUsage();
				return false;
			}

			if (args.Count == 1)
			{
				if (args[0].ToLower().Equals("test"))
					_runTest = true;
				else
				{
					PrintUsage();
					return false;
				}
			}
			else if (args.Count == 2 || args.Count == 3)
			{
				int i;
				if (!int.TryParse(args[0], out i))
				{
					PrintUsage();
					return false;
				}
				_m = i;

				if (!int.TryParse(args[1], out i))
				{
					PrintUsage();
					return false;
				}
				_n = i;

				if (args.Count == 3)
				{
					var path = args[2];
					if (!File.Exists(path))
					{
						Console.WriteLine("\r\nOutput file not found.\r\n");
						PrintUsage();
						return false;
					}

					_oldOut = Console.Out;
					_newOut = new StreamWriter(path, false, Encoding.UTF8);
					Console.SetOut(_newOut);
				}
			}
			else
			{
				PrintUsage();
				return false;
			}

			return true;
		}

		static void PrintUsage()
		{
			Console.WriteLine(
	@"Usage: ack [test |m n [outfile]]

test	Run BigIntStack tests.
m		First number of Ackermann's function.
n		Second number of Ackermann's function.
outfile	Output file name.");
		}

		static void TestBigIntStack()
		{
			var st = new BigIntStack();
			var num = new[] { 57, 64, 8992654378, long.MaxValue, 5, 791872 };
			foreach (var l in num)
			{
				st.Push(l);
			}

			for (int i = 0; i < 6; i++)
			{
				Console.WriteLine("{0}", st.Pop().ToString());
			}

			Console.ReadKey(true);
		}
	}
}
