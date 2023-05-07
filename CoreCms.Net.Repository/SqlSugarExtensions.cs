using SqlSugar;

namespace CoreCms.Net.Repository
{
	internal static class SqlSugarExtensions
	{
		internal static ISugarQueryable<T> WithNoLockOrNot<T>(this ISugarQueryable<T> query,bool @lock = false)
		{
			if (@lock)
			{
				query = query.With(SqlWith.NoLock);
			}

			return query;
		}
	}
}
