﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogicBlackJack.BusinessLogic.Enum;
using GameLogicBlackJack.GameLogic;
using GameLogicBlackJack.View;

namespace GameLogicBlackJack.Controllers
{
    public class GameController
    {
        Game game = new Game(2);
        
        public void ConsoleChoise()
        {
            game.AddBots();
            game.Ininialize();
            game.Deal();
            GameConsole.PlayerInfo();
            while (!game.blackJack)
            {
                GameConsole.PlayerMakeChoise();
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    game.Stand();
                    GameConsole.PlayerInfo();
                    Console.WriteLine("Balance: {0}", Game.Player.PlayerBalance);
                    GameConsole.DealerTakeCard();
                    break;
                }
                if (key.Key == ConsoleKey.Spacebar)
                {
                    game.Hit();
                    GameConsole.PlayerInfo();
                    GameConsole.PlayerTakeCard();
                    if(Game.Player.TotalValue > 21)
                    {
                        GameConsole.PlayerLose();
                        Console.WriteLine("Balance: {0}", Game.Player.PlayerBalance);
                        break;
                    }
                }
            }
        }
        public static Int32 ConsoleBotsChoise()
        {
            Int32 number;
            GameConsole.ConsolePlayerEnterNumberOfBots();
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out number);
            if (number < 0 || number > 5)
            {
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
            }
            return number;
        }

        public static String ConsolePlayerNickname()
        {
            GameConsole.ConsolePlayerEnterNickname();
            String inputLine = Console.ReadLine();
            inputLine.Trim().Replace(" ", "");
            if(string.IsNullOrEmpty(inputLine))
            {
                inputLine = Console.ReadLine();
                inputLine.Trim().Replace(" ", "");
            }
            return inputLine;
        }

        public static Decimal ConsolePlayerBalanse()
        {
            Int32 balance;
            GameConsole.PlayerEnterBalance();
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out balance);
            if ( balance <= 0 || balance >= 1000)
            {
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
            }
            return balance;
        }

       public static Decimal ConsolePlayerBet()
        {
            GameConsole.ConsolePlayerEnterBet();
            Int32 bet;
            String input = Console.ReadLine();
            input.Trim().Replace(" ", "");
            Int32.TryParse(input, out bet);
            if (bet <= 0 || bet < Game.Player.PlayerBalance)
            {
                input = Console.ReadLine();
                input.Trim().Replace(" ", "");
            }
            return bet;
        } 

    }
}
