using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Ersa.Global.Controls.Extensions
{
	public static class EDC_ICollectionViewExtensions
	{
		public static void SUB_GroupDescriptionErsetzen(this ICollectionView i_fdcCollectionView, params string[] ia_strPropertyNamen)
		{
			List<GroupDescription> list = i_fdcCollectionView.GroupDescriptions.ToList();
			foreach (string propertyName in ia_strPropertyNamen)
			{
				i_fdcCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(propertyName));
			}
			foreach (GroupDescription item in list)
			{
				i_fdcCollectionView.GroupDescriptions.Remove(item);
			}
		}

		public static void SUB_LiveSortingAktivieren(this ICollectionView i_fdcCollectionView, params string[] ia_strPropertyNamen)
		{
			ICollectionViewLiveShaping collectionViewLiveShaping = i_fdcCollectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveSorting)
			{
				foreach (string item in ia_strPropertyNamen)
				{
					collectionViewLiveShaping.LiveSortingProperties.Add(item);
				}
				collectionViewLiveShaping.IsLiveSorting = true;
			}
		}

		public static void SUB_LiveGroupingAktivieren(this ICollectionView i_fdcCollectionView, params string[] ia_strPropertyNamen)
		{
			ICollectionViewLiveShaping collectionViewLiveShaping = i_fdcCollectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveGrouping)
			{
				foreach (string item in ia_strPropertyNamen)
				{
					collectionViewLiveShaping.LiveGroupingProperties.Add(item);
				}
				collectionViewLiveShaping.IsLiveGrouping = true;
			}
		}

		public static void SUB_LiveFilteringAktivieren(this ICollectionView i_fdcCollectionView, params string[] ia_strPropertyNamen)
		{
			ICollectionViewLiveShaping collectionViewLiveShaping = i_fdcCollectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveFiltering)
			{
				foreach (string item in ia_strPropertyNamen)
				{
					collectionViewLiveShaping.LiveFilteringProperties.Add(item);
				}
				collectionViewLiveShaping.IsLiveFiltering = true;
			}
		}
	}
}
