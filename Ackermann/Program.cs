using System;
using System.Collections.Generic;
using System.Numerics;

namespace Ackermann
{
	class Program
	{
		private static int _m;
		private static int _n;
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
			else if (args.Count == 2)
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
	"Usage: ack [test |m n]\r\n\r\ntest\tRun BigIntStack tests.\r\nm\tFirst number of Ackermann's function.\r\nn\tSecond number of Ackermann's function.");
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
