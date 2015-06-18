namespace Ackermann
{
	class LinkedList<T>
	{
		public bool IsEmpty { get { return Count > 0; } }

		public LinkedNode<T> First { get; private set; }

		public LinkedNode<T> Last { get; private set; }

		public long Count { get; private set; }

		public void AddLast(T item)
		{
			var node = new LinkedNode<T> { Value = item };
			if (First == null)
			{
				First = Last = node;
				Count++;
				return;
			}

			var temp = Last;
			Last = node;
			node.Previous = temp;
			temp.Next = node;
			Count++;
		}

		public T RemoveLast()
		{
			if (Last == null) return default(T);

			var node = Last;
			if (Last.Previous == null)
			{
				Last = First = null;
				Count--;
				return node.Value;
			}

			Last = node.Previous;
			Count--;
			return node.Value;
		}
	}

	class LinkedNode<T>
	{
		public LinkedNode<T> Previous { get; set; }
		public LinkedNode<T> Next { get; set; }
		public T Value { get; set; }
	}
}
