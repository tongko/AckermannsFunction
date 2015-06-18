
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
			var item = RemoveLast();
			return item;
		}

		public T Peek()
		{
			return IsEmpty ? default(T) : Last.Value;
		}
	}
}
