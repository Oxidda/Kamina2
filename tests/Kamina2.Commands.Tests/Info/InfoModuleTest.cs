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
        [TestCase(null)]
        [TestCase("Message")]
        [TestCase("  ")]
        public void UserInfoAsync_WithVariousMessages_ReturnsExpectedResult(string message)
        {
            // Setup
            var service = Substitute.For<ITimeService>();
            var module = new InfoModule(service);
            
            // Call
            Task.Run(() => module.UserInfoAsync(message));

            // Assert
            service.DidNotReceiveWithAnyArgs().GetCurrent(); // Do not expect the service being called
            var temp = service.DidNotReceive().StartTime; // to check properties, temp variables have to be introduced
        }
    }
}