using System.Collections;

namespace BR.AN.Logging
{
	public class DbgProblemCollection
	{
		private SortedList probs;

		public int Count => probs.Count;

		public ProblemReason this[DbgKey key]
		{
			get
			{
				return (ProblemReason)probs[key.Name];
			}
		}

		public DbgProblemCollection()
		{
			probs = new SortedList();
		}

		public void Add(DbgKey key, string problem, string[] reasonList)
		{
			probs.Add(key.Name, new ProblemReason(problem, reasonList));
		}

		public bool Contains(DbgKey key)
		{
			return probs.Contains(key.Name);
		}
	}
}
