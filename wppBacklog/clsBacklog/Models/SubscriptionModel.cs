using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBacklog.Models
{

    [Table("Subscriptions")]
    public class SubscriptionModel : SqlDbBaseModel
    {
        public SubscriptionModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    [Table("SubscriptionLogs")]
    public class SubscriptionLogModel : SqlDbBaseModel
    {
        public SubscriptionLogModel(string id, string subscriptionId,
            string transactionId, string transactionMethod,
            DateTime startFrom, DateTime endAt,
            int actualChargedAmount, string ownerId)
        {
            Id = id;
            SubscriptionId = subscriptionId;
            TransactionId = transactionId;
            TransactionMethod = transactionMethod;
            StartFrom = startFrom;
            EndAt = endAt;
            ActualChargedAmount = actualChargedAmount;
            OwnerId = ownerId;
        }

        [Key, MaxLength(64)]
        public string Id { get; set; }
        [MaxLength(64)]
        public string SubscriptionId { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime EndAt { get; set; }
        public string? Memo { get; set; }
        public string TransactionId { get; set; }
        public string TransactionMethod { get; set; }
        public DateTime TransactionTimeStamp { get; set; }
        public int ActualChargedAmount { get; set; }
    }
}
