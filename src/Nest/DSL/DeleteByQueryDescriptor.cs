﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nest.Resolvers.Converters;
using System.Linq.Expressions;
using Nest.Resolvers;

namespace Nest
{
	[JsonConverter(typeof(ActAsQueryConverter))]
	public partial class DeleteByQueryDescriptor<T> 
		: QueryPathDescriptorBase<DeleteByQueryDescriptor<T>, T, DeleteByQueryQueryString>
		, IActAsQuery
		, IPathInfo<DeleteByQueryQueryString> 
		where T : class
	{
		BaseQuery IActAsQuery._Query { get; set; }

		public DeleteByQueryDescriptor<T> MatchAll()
		{
			((IActAsQuery)this)._Query = new QueryDescriptor<T>().MatchAll();
			return this;
		}

		public DeleteByQueryDescriptor<T> Query(Func<QueryDescriptor<T>, BaseQuery> querySelector)
		{
			((IActAsQuery)this)._Query = querySelector(new QueryDescriptor<T>());
			return this;
		}

		ElasticsearchPathInfo<DeleteByQueryQueryString> IPathInfo<DeleteByQueryQueryString>.ToPathInfo(IConnectionSettings settings)
		{
			var pathInfo = this.ToPathInfo<DeleteByQueryQueryString>(settings, this._QueryString);
			pathInfo.HttpMethod = PathInfoHttpMethod.DELETE;
			return pathInfo;
		}
	}
}
