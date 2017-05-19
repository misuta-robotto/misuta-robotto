using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AL
{
    public class RobotConfiguration
    {
        public static string ADRESS = "127.0.0.1";
        public static int PORT = 1592;
      
        public static void setAdress(String adress)
        {
            ADRESS = adress;
        }

        public static void setPort(int port)
        {
            PORT = port;
        }
    }
}
