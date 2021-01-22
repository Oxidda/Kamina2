﻿using System;
using System.Threading.Tasks;
using Discord.Commands;
using Kamina2.Commands.Info;
using Kamina2.Core.Common;
using Kamina2.TestUtils;
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

        [Ignore("Test is currently failing due to a NullReferenceException as InfoModule.Context is NULL")]
        [Test]
        public async Task UserInfoAsyncWithoutMessageArgument_Always_CallsTimeService()
        {
            // Setup
            // Configure service to return expected values
            var service = Substitute.For<ITimeService>();
            service.StartTime.Returns(new DateTime(2020, 11, 1));
            service.GetCurrent().Returns(new DateTime(2020, 11, 2));

            var module = new InfoModule(service);

            // Call
            await module.UserInfoAsync();

            // Assert
            // Assert that the following calls are made for the time service.
            var temp = service.Received(1).StartTime;
            service.Received(1).GetCurrent(); // to check properties, temp variables have to be introduced or this results in a compilation error
        }

        [Ignore("Test is currently failing due to a NullReferenceException as InfoModule.Context is NULL")]
        [Test]
        [TestCase(null)]
        [TestCase("Message")]
        [TestCase("  ")]
        public async Task UserInfoAsyncWithMessageArgument_WithVariousMessages_DoesNotCallTimeService(string message)
        {
            // Setup
            var service = Substitute.For<ITimeService>();
            var module = new InfoModule(service);

            // Call
            await module.UserInfoAsync(message);

            // Assert
            service.DidNotReceiveWithAnyArgs().GetCurrent(); // Do not expect the service being called
            var temp = service.DidNotReceive().StartTime; // to check properties, temp variables have to be introduced or this results in a compilation error
        }

        [Test]
        public void UserInfoAsyncWithoutArguments_Always_ReturnsExpectedAttributes()
        {
            // Call
            var commandAttribute = ReflectionHelper.GetCustomAttribute<InfoModule, CommandAttribute>(nameof(InfoModule.UserInfoAsync));
            var summaryAttribute = ReflectionHelper.GetCustomAttribute<InfoModule, SummaryAttribute>(nameof(InfoModule.UserInfoAsync));

            // Assert
            Assert.That(commandAttribute, Is.Not.Null);
            Assert.That(commandAttribute.Text, Is.EqualTo("info"));

            const string expectedSummaryText = "Returns info about the current user, or the user parameter, if one passed.";
            Assert.That(summaryAttribute, Is.Not.Null);
            Assert.That(summaryAttribute.Text, Is.EqualTo(expectedSummaryText));
        }

        [Test]
        public void UserInfoAsyncWithMessageArgument_Always_ReturnsExpectedAttributes()
        {
            // Setup
            var argumentTypes = new[]
            {
                typeof(string)
            };

            // Call
            var commandAttribute = ReflectionHelper.GetCustomAttribute<InfoModule, CommandAttribute>(nameof(InfoModule.UserInfoAsync),
                                                                                                     argumentTypes);
            var aliasAttribute = ReflectionHelper.GetCustomAttribute<InfoModule, AliasAttribute>(nameof(InfoModule.UserInfoAsync),
                                                                                                 argumentTypes);

            // Assert
            Assert.That(commandAttribute, Is.Not.Null);
            Assert.That(commandAttribute.Text, Is.EqualTo("say"));

            Assert.That(aliasAttribute, Is.Not.Null);

            // Asserts a string collection:
            var aliasValues = aliasAttribute.Aliases;
            Assert.That(aliasValues, Has.Length.EqualTo(1));
            Assert.That(aliasValues[0], Is.EqualTo("respond"));

            // Or alternatively:
            CollectionAssert.AreEqual(new[]
            {
                "respond"
            }, aliasValues);
        }
    }
}