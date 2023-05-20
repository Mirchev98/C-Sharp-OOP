using System;
using NUnit.Framework;

namespace FootballTeam.Tests
{
    public class Tests
    {
        private FootballPlayer player;
        private FootballTeam team;
        [SetUp]
        public void Setup()
        {
            player = new FootballPlayer("Gosho", 15, "Goalkeeper");
            team = new FootballTeam("Ganchovci", 16);
        }

        [Test]
        public void TestPlyerName()
        {
            string expected = "Gosho";

            string actual = player.Name;
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestPlyerNumber()
        {
            int expected = 15;

            int actual = player.PlayerNumber;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestPlayerPosition()
        {
            string expected = "Goalkeeper";

            string actual = player.Position;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestPlayerScore()
        {
            int expected = 1;

            player.Score();

            Assert.AreEqual(expected, player.ScoredGoals);
        }

        [Test]
        public void PlayerNameShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FootballPlayer playerTwo = new FootballPlayer(null, 15, "Goalkeeper");
            }, "Name cannot be null or empty!");
        }

        [Test]
        public void PlayerNumberShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FootballPlayer playerTwo = new FootballPlayer("Pencho", 50, "Goalkeeper");
            }, "Player number must be in range [1,21]");
        }

        [Test]
        public void PlayerPositionShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FootballPlayer playerTwo = new FootballPlayer("Pencho", 1, "InvalidRole");
            }, "Invalid Position");
        }

        [Test]
        public void TestTeamName()
        {
            string expected = "Ganchovci";

            string actual = team.Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestTeamCapacity()
        {
            int expected = 16;

            int actual = team.Capacity;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAddPlayer()
        {
            string actual = team.AddNewPlayer(player);
            Assert.AreEqual(1, team.Players.Count);

            string expected = $"Added player {player.Name} in position {player.Position} with number {player.PlayerNumber}";
            Assert.AreEqual(expected, actual);
        }

        [Test]

        public void TestAddReturnValue()
        {
            string actual = team.AddNewPlayer(player);

            string expected = $"Added player {player.Name} in position {player.Position} with number {player.PlayerNumber}";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestPickPlayer()
        {
            team.AddNewPlayer(player);
            Assert.AreEqual(player, team.PickPlayer("Gosho"));
        }

        [Test]
        public void TestScoreTeam()
        {
            int expected = 1;
            team.AddNewPlayer(player);
            team.PlayerScore(15);

            Assert.AreEqual(expected, player.ScoredGoals);
        }

        [Test]
        public void TestScoreTeamReturnValue()
        {
            string expected = $"{player.Name} scored and now has 1 for this season!";
            team.AddNewPlayer(player);
            string actual = team.PlayerScore(15);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestWhenFullCountShouldThrow()
        {
            for (int i = 0; i < 16; i++)
            {
                team.AddNewPlayer(new FootballPlayer("Pencho", i + 1, "Goalkeeper"));
            }

            int expected = 16;

            team.AddNewPlayer(player);

            Assert.AreEqual(expected, team.Capacity);
        }

        [Test]
        public void TestFullCountReturnValue()
        {
            string expected = "No more positions available!";
            
            for (int i = 0; i < 16; i++)
            {
                team.AddNewPlayer(new FootballPlayer("Pencho", i + 1, "Goalkeeper"));
            }

            string actual = team.AddNewPlayer(player);

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void TestShouldThrowInvalidCapacity()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FootballTeam teamTwo = new FootballTeam("Name", 1);
            }, "Capacity min value = 15");
        }

        [Test]
        public void TestNullName()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FootballTeam teamTwo = new FootballTeam(null, 17);
            }, "Name cannot be null or empty!");
        }
    }
}