using NUnit.Framework;

namespace CursesSharp.LearnTest
{
    [TestFixture]
    class LearnTests
    {
        [SetUp]
        public void StartConsole() {
            ConsoleUtil.Start();
            Curses.InitScr();
        }

        [TearDown]
        public void EndConsole() {
            ConsoleUtil.End();
        }

        [Test]
        public void ShouldGreetTheWorld() {
            Stdscr.Add(0, 0, "Greetings World!");
            Stdscr.Refresh();

            var content = ConsoleUtil.ReadConsoleAsString();
            Assert.That(content, Is.StringContaining("Greetings World!"));
        }

    }

}
