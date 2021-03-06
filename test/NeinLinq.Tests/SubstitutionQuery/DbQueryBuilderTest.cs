﻿#if NET461

using NeinLinq.EntityFramework;
using NeinLinq.Fakes.SubstitutionQuery;
using System;
using System.Linq;
using Xunit;

namespace NeinLinq.Tests.SubstitutionQuery
{
    public class DbQueryBuilderTest
    {
        readonly object query = Enumerable.Empty<Dummy>().AsQueryable().OrderBy(d => d.Id);

        [Fact]
        public void ShouldHandleInvalidArguments()
        {
            Assert.Throws<ArgumentNullException>(() => ((IQueryable)query).ToSubstitution(null, typeof(OtherFunctions)));
            Assert.Throws<ArgumentNullException>(() => ((IQueryable)query).ToSubstitution(typeof(Functions), null));
            Assert.Throws<ArgumentNullException>(() => ((IQueryable<Dummy>)query).ToSubstitution(null, typeof(OtherFunctions)));
            Assert.Throws<ArgumentNullException>(() => ((IQueryable<Dummy>)query).ToSubstitution(typeof(Functions), null));
            Assert.Throws<ArgumentNullException>(() => ((IOrderedQueryable)query).ToSubstitution(null, typeof(OtherFunctions)));
            Assert.Throws<ArgumentNullException>(() => ((IOrderedQueryable)query).ToSubstitution(typeof(Functions), null));
            Assert.Throws<ArgumentNullException>(() => ((IOrderedQueryable<Dummy>)query).ToSubstitution(null, typeof(OtherFunctions)));
            Assert.Throws<ArgumentNullException>(() => ((IOrderedQueryable<Dummy>)query).ToSubstitution(typeof(Functions), null));
        }

        [Fact]
        public void ShouldRewriteUntypedQueryable()
        {
            var actual = ((IQueryable)query).ToSubstitution(typeof(Functions), typeof(OtherFunctions));

            AssertQuery(actual);
        }

        [Fact]
        public void ShouldRewriteTypedQueryable()
        {
            var actual = ((IQueryable<Dummy>)query).ToSubstitution(typeof(Functions), typeof(OtherFunctions));

            AssertQuery(actual);
        }

        [Fact]
        public void ShouldRewriteUntypedOrderedQueryable()
        {
            var actual = ((IOrderedQueryable)query).ToSubstitution(typeof(Functions), typeof(OtherFunctions));

            AssertQuery(actual);
        }

        [Fact]
        public void ShouldRewriteTypedOrderedQueryable()
        {
            var actual = ((IOrderedQueryable<Dummy>)query).ToSubstitution(typeof(Functions), typeof(OtherFunctions));

            AssertQuery(actual);
        }

        static void AssertQuery(IQueryable actual)
        {
            Assert.IsType<RewriteDbQuery<Dummy>>(actual);
            Assert.IsType<RewriteDbQueryProvider>(actual.Provider);

            var actualProvider = (RewriteDbQueryProvider)actual.Provider;

            Assert.IsType<SubstitutionQueryRewriter>(actualProvider.Rewriter);
            Assert.IsType<EnumerableQuery<Dummy>>(actualProvider.Provider);
        }
    }
}

#endif
