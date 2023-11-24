using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySpot.tests.Unit.Framework
{
    public class ServiceCollectionTests
    {
        [Fact]
        public void test()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IUser, Admin>();
            serviceCollection.AddSingleton<IUser, Employee>();
            serviceCollection.AddSingleton<IUser, Manager>();
            serviceCollection.AddScoped(typeof(IMessanger<>), typeof(Messanger<>));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            var users = serviceProvider.GetServices<IUser>();

            users.Count().ShouldBe(3);
            //using (var scope1 = serviceProvider.CreateScope())
            //{
            //    var messenger1 = scope1.ServiceProvider.GetService<IMessanger>();
            //    var messenger2 = scope1.ServiceProvider.GetService<IMessanger>();

            //    messenger1.ShouldBe(messenger2);
            //}

            //using (var scope2 = serviceProvider.CreateScope())
            //{
            //    var messenger1 = scope2.ServiceProvider.GetService<IMessanger>();
            //    var messenger2 = scope2.ServiceProvider.GetService<IMessanger>();

            //    messenger1.ShouldBe(messenger2);
            //}
        }

        public interface IMessanger<T>
        {
            void Send(T message);
        }

        public class Messanger<T> : IMessanger<T>
        {
            private readonly Guid _id = Guid.NewGuid();

            public void Send(T message)
            {
                Console.WriteLine($"Sending a message ... [ID {_id}]");    
            }
        }

        public interface IUser
        {

        }

        public class Admin : IUser
        {
            private readonly IMessanger<string> _messager;

            public Admin(IMessanger<string> messanger)
            {
                _messager = messanger;
            }
        }

        public class Employee : IUser 
        {
        }
        
        public class Manager : IUser
        {

        }
    }
}
