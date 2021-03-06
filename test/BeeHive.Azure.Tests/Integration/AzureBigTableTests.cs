﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeHive.DataStructures;
using Xunit;

namespace BeeHive.Azure.Tests.Integration
{
    public class AzureBigTableTests
    {
        private const string ConnectionString = "UseDevelopmentStorage=true;";

        [Fact]
        public void CanInsertAndGet()
        {
            var store = new AzureBigTableStore<HasIdentityAndRange>(connectionString:ConnectionString);
            var item = new HasIdentityAndRange()
            {
                Id = Guid.NewGuid().ToString(),
                RangeKey = "1",
                ETag = "\"" + Guid.NewGuid().ToString() + "\"",
                LastModified = DateTimeOffset.Now,
                When = DateTimeOffset.Now,
                When2 = DateTime.Now,
                When4 = DateTimeOffset.Now,
                HowMany = 1,
                HowMany2 = 83248926438962,
                HowMuch = 32423.45243,
                Secret = Guid.NewGuid(),
                What = "chapooli",
                Really = true,
                Booboo = new byte[100]
            };
            store.InsertAsync(item).Wait();

            var storedItem = store.GetAsync(item.Id, item.RangeKey).Result;
            Assert.Equal(item.Id, storedItem.Id);
            Assert.Equal(item.RangeKey, storedItem.RangeKey);
            Assert.Equal(item.HowMany, storedItem.HowMany);
            Assert.Equal(item.HowMany2, storedItem.HowMany2);
            Assert.Equal(item.HowMany3, storedItem.HowMany3);
            Assert.Equal(item.HowMuch, storedItem.HowMuch);
            Assert.Equal(item.What, storedItem.What);
            Assert.Equal(item.Really, storedItem.Really);
            Assert.Equal(item.Really2, storedItem.Really2);
            Assert.Equal(item.Secret, storedItem.Secret);
            Assert.Equal(item.Secret2, storedItem.Secret2);
            Assert.Equal(item.Booboo, storedItem.Booboo);
            Assert.Equal(item.When, storedItem.When);
            Assert.Equal(item.When2.Value.ToUniversalTime(), storedItem.When2);
            Assert.Equal(item.When4, storedItem.When4);
            Assert.Equal(item.When3, storedItem.When3);
        }

        [Fact]
        public void CanInsertAndDelete()
        {
            var store = new AzureBigTableStore<HasIdentityAndRange>(connectionString: ConnectionString);
            var item = new HasIdentityAndRange()
            {
                Id = Guid.NewGuid().ToString(),
                RangeKey = "1",
                LastModified = DateTimeOffset.Now
            };
            store.InsertAsync(item).Wait();
            store.DeleteAsync(item).Wait();

            Assert.False(store.ExistsAsync(item.Id, item.RangeKey).Result);
        }


        [Fact]
        public void CanGetRange()
        {
            var store = new AzureBigTableStore<HasIdentityAndRange>(connectionString: ConnectionString);
            var item = new HasIdentityAndRange()
            {
                Id = Guid.NewGuid().ToString(),
                RangeKey = "1",
                ETag = "\"" + Guid.NewGuid().ToString() + "\"",
                LastModified = DateTimeOffset.Now
            }; 
            var item2 = new HasIdentityAndRange()
            {
                Id = item.Id,
                RangeKey = "2",
                ETag = "\"" + Guid.NewGuid().ToString() + "\"",
                LastModified = DateTimeOffset.Now
            };
            store.InsertAsync(item).Wait();
            store.InsertAsync(item2).Wait();

            Assert.Equal(2, store.GetRangeAsync(item.Id, "1", "2").Result.Count());
        }



    }


    public class HasIdentityAndRange : IHaveIdentityAndRange, IConcurrencyAware
    {
        public string Id { get; set; }
        public string RangeKey { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public string ETag { get; set; }

        public string What { get; set; }

        public DateTimeOffset When { get; set; }

        public DateTimeOffset? When4 { get; set; }

        public DateTime? When2 { get; set; }

        public DateTime? When3 { get; set; }

        public int HowMany { get; set; }

        public long HowMany2 { get; set; }

        public long HowMany3 { get; set; }

        public double HowMuch { get; set; }

        public bool? Really {get; set; }

        public bool? Really2 {get; set; }

        public Guid? Secret { get; set; }

        public Guid Secret2 { get; set; }

        public byte[] Booboo { get; set; }

    }

    public class FeedItem
    {

        public FeedItem()
        {
            PubDate = DateTimeOffset.Now;
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string UniqueId { get; set; }

        public DateTimeOffset PubDate { get; set; }

        public string[] Categories { get; set; }

        public string ParentChannelUrl { get; set; }

        public string Author { get; set; }

        public string Id { get; set; }

        public string ChannelCategory { get; set; }

        public bool HasOriginalPubDate { get; set; }

    }

    public class FeedItemEntity : FeedItem, IHaveIdentityAndRange
    {
        public string RangeKey { get; set; }
    }
}
