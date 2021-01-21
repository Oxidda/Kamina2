using System;
using System.Threading.Tasks;
using Discord.Commands;
using Kamina2.Commands.Info;
using Kamina2.Core.Common;
using NSubstitute;
using NUnit.Framework;

namespace Kamina2.Commands.Tests.Info
{
    [TestFixture]
    public class InfoModuleTest
    {
        [Test]
        public void Constructor_TimeServiceNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new InfoModule(null);

            // Assert
            Assert.That(call, Throws.TypeOf<ArgumentNullException>()
                .With.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("timeService"));
        }

        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            var service = Substitute.For<ITimeService>();

            // Call
            var module = new InfoModule(service);

            // Assert
            Assert.That(module, Is.InstanceOf<ModuleBase<ShardedCommandContext>>());
        }

        [Test]
        public void UserInfoAsyncWithoutMessageArgument_Always_CallsTimeService()
        {
            // Setup
            // Configure service to return expected values
            var service = Substitute.For<ITimeService>();
            service.StartTime.Returns(new DateTime(2020, 11, 1));
            service.GetCurrent().Returns(new DateTime(2020, 11, 2));

            var module = new InfoModule(service);

            // Call
            Task.Run(async () => module.UserInfoAsync())
                .GetAwaiter()
                .GetResult();

            // Assert
            // Assert that the following calls are made for the time service.
            var temp = service.Received(1).StartTime;
            service.Received(1).GetCurrent(); // to check properties, temp variables have to be introduced or this results in a compilation error
        }

        [Test]
        [TestCase(null)]
        [TestCase("Message")]
        [TestCase("  ")]
        public void UserInfoAsyncWithMessageArgument_WithVariousMessages_DoesNotCallTimeService(string message)
        {
            // Setup
            var service = Substitute.For<ITimeService>();
            var module = new InfoModule(service);

            // Call
            Task.Run(async () => module.UserInfoAsync(message))
                .GetAwaiter()
                .GetResult();

            // Assert
            service.DidNotReceiveWithAnyArgs().GetCurrent(); // Do not expect the service being called
            var temp = service.DidNotReceive().StartTime; // to check properties, temp variables have to be introduced or this results in a compilation error
        }
    }
}