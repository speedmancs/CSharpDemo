using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitDemo
{
    public interface ICommand
    {
        void Execute();
        event EventHandler Executed;
    }

    public class CommandWatcher
    {
        ICommand command;
        public CommandWatcher(ICommand command)
        {
            command.Executed += OnExecuted;
        }
        public bool DidStuff { get; private set; }
        public void OnExecuted(object o, EventArgs e) { DidStuff = true; }
    }


    public class CommandRepeater
    {
        ICommand command;
        int numberOfTimesToCall;
        public CommandRepeater(ICommand command, int numberOfTimesToCall)
        {
            this.command = command;
            this.numberOfTimesToCall = numberOfTimesToCall;
        }

        public void Execute()
        {
            for (var i = 0; i < numberOfTimesToCall; i++) command.Execute();
        }
    }

    public class SomethingThatNeedsACommand
    {
        ICommand command;
        public SomethingThatNeedsACommand(ICommand command)
        {
            this.command = command;
        }
        public void DoSomething() { command.Execute(); }
        public void DontDoAnything() { }
    }

    public class LowFuelWarningEventArgs : EventArgs
    {
        public int PercentLeft { get; }
        public LowFuelWarningEventArgs(int percentLeft)
        {
            PercentLeft = percentLeft;
        }
    }

    public class FuelManagement
    {
        public event EventHandler<LowFuelWarningEventArgs> LowFuelDetected;
        public void DoSomething()
        {
            LowFuelDetected?.Invoke(this, new LowFuelWarningEventArgs(15));
        }
    }

    class CommandTest
    {

        [Test]
        public void ShouldDoStuffWhenCommandExecutes()
        {
            var command = Substitute.For<ICommand>();
            var watcher = new CommandWatcher(command);

            command.Executed += Raise.Event();

            Assert.That(watcher.DidStuff);
        }

        [Test]
        public void MakeSureWatcherSubscribesToCommandExecuted()
        {
            var command = Substitute.For<ICommand>();
            var watcher = new CommandWatcher(command);

            // Not recommended. Favour testing behaviour over implementation specifics.
            // Can check subscription:
            command.Received().Executed += watcher.OnExecuted;
            // Or, if the handler is not accessible:
            command.Received().Executed += Arg.Any<EventHandler>();
        }

        [Test]
        public void Should_execute_command()
        {
            //Arrange
            var command = Substitute.For<ICommand>();
            var something = new SomethingThatNeedsACommand(command);
            //Act
            something.DoSomething();
            //Assert
            command.Received().Execute();
        }


        [Test]
        public void Should_execute_command_the_number_of_times_specified()
        {
            var command = Substitute.For<ICommand>();
            var repeater = new CommandRepeater(command, 3);
            //Act
            repeater.Execute();
            //Assert
            command.Received(3).Execute(); // << This will fail if 2 or 4 calls were received
        }

        // Often it is easiest to use a lambda for this, as shown in the following test:
        [Test]
        public void ShouldRaiseLowFuel_WithoutNSub()
        {
            var fuelManagement = new FuelManagement();
            var eventWasRaised = false;
            fuelManagement.LowFuelDetected += (o, e) => eventWasRaised = true;

            fuelManagement.DoSomething();

            Assert.That(eventWasRaised);
        }

        // We can also use NSubstitute for this if we want more involved argument matching logic.
        // NSubstitute also gives us a descriptive message if the assertion fails which may be helpful in some cases.
        // (For example, if the call was not received with the expected arguments, we'll get a list of the non-matching
        // calls made to that member.)
        //
        // Note we could still use lambdas and standard assertions for this, but a substitute may be worth considering
        // in some of these cases.
        [Test]
        public void ShouldRaiseLowFuel()
        {
            var fuelManagement = new FuelManagement();
            var handler = Substitute.For<EventHandler<LowFuelWarningEventArgs>>();
            fuelManagement.LowFuelDetected += handler;

            fuelManagement.DoSomething();

            handler
                .Received()
                .Invoke(fuelManagement, Arg.Is<LowFuelWarningEventArgs>(x => x.PercentLeft < 20));
        }
    }
}
