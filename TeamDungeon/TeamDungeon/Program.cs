using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TeamDungeon
{
    internal class Program
    {
        private static Player player;
        static void Main(string[] args)
        {
            DisplayTitle();
            
        }

        static void DisplayTitle()
        {
            LoginScene();
        }

        static void LoginScene()
        {
            player = new Player();
            Console.WriteLine("안농");
        }
    }
}