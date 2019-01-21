using System;
using Xunit;

namespace csharp_tutorial
{
    public class CompositionExamples
    {
        // Compare inheritance / strategy pattern / composition
        // In real life inheritance/composition cases are never this simple

        public class Account
        {
            public double Balance { get; set; }
        }

        public class Transaction
        {
            public string Target { get; set; }
            public Account Account { get; set; }
        }

        public abstract class Handler
        {
            public abstract bool Handle(Transaction transaction);
        }

        public class AllowRichHandler : Handler
        {
            public override bool Handle(Transaction transaction)
            {
                return transaction.Account.Balance > 100;
            }
        }

        public class AllowAllHandler : Handler
        {
            public override bool Handle(Transaction transaction)
            {
                return transaction.Account.Balance > 0;
            }
        }

        [Fact]
        public void Inheritance()
        {
            Handler suitabilityFactory(int customerType)
            {
                if (customerType == 0)
                    return new AllowAllHandler();
                else
                    return new AllowRichHandler();
            }

            var transactionput = new Transaction
            {
                Target = "GOOGL",
                Account = new Account { Balance = 50 }
            };

            Handler h1 = suitabilityFactory(0);
            var response = h1.Handle(transactionput);
            Assert.True(response);

            Handler h2 = suitabilityFactory(1);
            var response2 = h2.Handle(transactionput);
            Assert.False(response2);
        }

        public interface ISuitabilityStrategy
        {
            bool Handle(Transaction transaction);
        }

        public class AllowAllStrategy : ISuitabilityStrategy
        {
            public bool Handle(Transaction transaction) => transaction.Account.Balance > 0;
        }

        public class AllowOver100Strategy : ISuitabilityStrategy
        {
            public bool Handle(Transaction transaction) => transaction.Account.Balance > 100;
        }

        public class HandlerStrategy
        {
            public ISuitabilityStrategy SuitabilityStrategy { get; set; }

            public bool Handle(Transaction transaction)
            {
                return SuitabilityStrategy.Handle(transaction);
            }
        }

        [Fact]
        public void Strategy()
        {
            var handler = new HandlerStrategy();

            ISuitabilityStrategy getStrategy(int strategyType)
            {
                return strategyType == 0 ? new AllowAllStrategy() as ISuitabilityStrategy : new AllowOver100Strategy();
            }

            var transaction = new Transaction
            {
                Target = "GOOGL",
                Account = new Account { Balance = 50 }
            };

            handler.SuitabilityStrategy = getStrategy(0);

            var response = handler.Handle(transaction);
            Assert.True(response);

            handler.SuitabilityStrategy = getStrategy(1);
            var response2 = handler.Handle(transaction);
            Assert.False(response2);
        }

        public class HandlerComposition
        {
            private readonly Func<Transaction, bool> _handleFunc;

            public HandlerComposition(Func<Transaction, bool> handleFunc) => _handleFunc = handleFunc;

            public bool Handle(Transaction transaction) => _handleFunc(transaction);
        }

        [Fact]
        public void Composition()
        {
            HandlerComposition Builder(int type)
            {
                if (type == 0)
                    return new HandlerComposition((i) => i.Account.Balance > 0);
                else
                    return new HandlerComposition((i) => i.Account.Balance > 100);
            }

            var transaction = new Transaction
            {
                Target = "GOOGL",
                Account = new Account { Balance = 50 }
            };

            HandlerComposition h1 = Builder(0);
            var response = h1.Handle(transaction);
            Assert.True(response);

            HandlerComposition h2 = Builder(1);
            var response2 = h2.Handle(transaction);
            Assert.False(response2);
        }
    }
}