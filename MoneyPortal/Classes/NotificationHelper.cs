using MoneyPortal.Models;
using System;
using System.Security.Cryptography.X509Certificates;

namespace MoneyPortal.Classes
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void TransactionNotification(BankAccount account, Transaction transaction)
        {
            if (account.CurrentBalance < 0)
            {
                var subject = "Overdraft Alert!";
                var message = "You or someone with access to your bank account has made a transaction that has overdrawn your account.";
                GenerateNotification(subject, message, account.HouseholdId);
            }
            else if (account.CurrentBalance < account.LowBalanceLevel)
            {
                var subject = "Low Balance Alert!";
                var message = "You or someone with access to your bank account has made a transaction that leaves your account below the low balance threshold.";
                GenerateNotification(subject, message, account.HouseholdId);
            }
            else
                return;
        }
        private void GenerateNotification(string subject, string message, int? householdId)
        {
            var notification = new Notification
            {
                Created = DateTime.Now,
                Subject = subject,
                Body = message,
                IsRead = false,
                HouseholdId = householdId == null ? null : householdId
            };

            db.Notifications.Add(notification);
            db.SaveChanges();
        }
        private void GenerateWarningNotification()
        {

        }
        private void GenerateOverdraftNotification()
        {

        }
    }
}