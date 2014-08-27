using System.Collections.Generic;
using System.Linq;
using Lunar;

namespace Solar.Filtering
{
	class FollowingFilterSource : FilterSource
	{
		IEnumerator<IEnumerable<Status>> statuses;
		int currentPage;

		public override bool Serializable
		{
			get
			{
				return false;
			}
		}

		public string UserName
		{
			get;
			set;
		}

		protected override IEnumerable<IEntry> GetStatuses(TwitterClient client, StatusRange range)
		{
			if (statuses == null ||
				range == null ||
				range.Page == 1 ||
				range.Page < currentPage)
			{
				statuses = client.Friends.List(this.UserName ?? client.Account.Name)
					.Select(p => p.Select(u => u.Status))
					.GetEnumerator();
				currentPage = 1;
			}

			for (int i = currentPage; i <= range.Page; i++)
				statuses.MoveNext();

			return statuses.Current;
		}

		protected override bool StreamEntryMatches(IEntry entry)
		{
			return false;
		}
	}
}
