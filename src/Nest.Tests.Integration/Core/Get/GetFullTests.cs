﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Nest;
using Newtonsoft.Json.Converters;
using Nest.Resolvers.Converters;
using Nest.Tests.MockData.Domain;
using FluentAssertions;
namespace Nest.Tests.Integration.Core.Get
{
	[TestFixture]
	public class GetFullTests : IntegrationTests
	{
		private void DefaultAssertations(IGetResponse<ElasticsearchProject> result)
		{
			result.IsValid.Should().BeTrue();
			result.Id.Should().Be("1");
			result.Index.Should().Be(ElasticsearchConfiguration.DefaultIndex);
			result.Type.Should().Be("elasticsearchprojects");
			result.Version.Should().Be("1");
			result.Exists.Should().BeTrue();
		}

		[Test]
		public void GetSimple()
		{
			var result = this._client.Get<ElasticsearchProject>(1);
			this.DefaultAssertations(result);
			
			
		}
		[Test]
		public void GetWithPathInfo()
		{
			var result = this._client.Get<ElasticsearchProject>(1, ElasticsearchConfiguration.DefaultIndex, "elasticsearchprojects");
			this.DefaultAssertations(result);
		}
		
		[Test]
		public void GetUsingDescriptorWithTypeAndFields()
		{
			var result = this._client.Get<ElasticsearchProject>(g => g
				.Index(ElasticsearchConfiguration.DefaultIndex)
				.Type("elasticsearchprojects")
				.Id(1)
				.Fields(p=>p.Content, p=>p.Name, p=>p.Id, p=>p.DoubleValue)
			);
			this.DefaultAssertations(result);

			result.Source.Should().BeNull();
			result.Fields.Should().NotBeNull();
			result.Fields.FieldValues.Should().NotBeNull().And.HaveCount(4);
			result.Fields.FieldValue<string>(p => p.Name).Should().Be("pyelasticsearch");
			result.Fields.FieldValue<int>(p => p.Id).Should().Be(1);
			result.Fields.FieldValue<double>(p => p.DoubleValue).Should().NotBe(default(double));

		}

		[Test]
		public void GetMissing()
		{
			int doesNotExistId = 1234567;
			var result = this._client.Get<ElasticsearchProject>(doesNotExistId);
			result.Exists.Should().BeFalse();
		}
	}
}
