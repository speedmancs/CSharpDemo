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
    }
}
