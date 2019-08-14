namespace Prism.Regions
{
	public interface IRegionBehavior
	{
		IRegion Region
		{
			get;
			set;
		}

		void Attach();
	}
}
