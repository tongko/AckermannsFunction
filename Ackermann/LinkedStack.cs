using System.Collections.Generic;

namespace Ackermann
{
	class LinkedStack<T> : LinkedList<T>
	{
		public void Push(T item)
		{
			AddLast(item);
		}

		public T Pop()
		{
			var item = Last;
			RemoveLast();
			return item.Value;
		}

		public T Peek()
		{
			return Last.Value;
		}
	}
}
