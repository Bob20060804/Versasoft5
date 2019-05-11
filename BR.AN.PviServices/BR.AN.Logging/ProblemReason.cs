namespace BR.AN.Logging
{
	public class ProblemReason
	{
		private string problem;

		private string[] reasons;

		public ProblemReason(string s1, string[] s2)
		{
			problem = s1;
			reasons = s2;
		}

		public string GetProblem()
		{
			return problem;
		}

		public string[] GetReasons()
		{
			return reasons;
		}
	}
}
