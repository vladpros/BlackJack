using BlackJackDataBaseAccess.Entities;
using BlackJackDataBaseAccess.Repository.Interface;
using DataBaseControl.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            IPlayerRepository playerRepository = new DapPlayerRepository();

            var k = playerRepository.Create(new Player { Name = "fweweg" });
            playerRepository.Remove(new Player { Id = k });
            Console.WriteLine("Hello World!");
            Console.ReadKey(); 
        }
    }
}
