/*
        PROPERTY OF @xhyabunny

        WARNING; THIS APPLICATION AND SOURCE CODE BELONGS TO @xhyabunny.orgTM
        
        WE DON'T CONDONE ANY TYPE OF LEAKS OR PIRATING OF OUR SOFTWARE
        ANY KIND OF THE MENTIONED ABOVE COULD RESULT ON A LAW SUIT.
        
        THIS OPEN SOURCE CODE IS AVAIABLE TO BE EDITED AND REPUBLISHED, AS LONG AS 
        YOU PUBLISH YOUR SOURCE CODE ALONG WITH THE EXECUTABLE FILE.

        DO NOT SELL. (this will result in legal issues)

        YOU HAVE BEEN WARNED.
        -SIGNED, @xhyabunny.orgTM 2022
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Principal;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;

namespace xhyaOne_
{
    class Program
    {

        public static class globalVariables //public variables to use throughout all the console
        {

            public static string helpFile = @"help file
you can add new lines
to help people use your console!

all the text will adapt itself to the text file 
"; //help file text, this file content is loaded into a txt file at the console start

            public static string version_ = "1.0";                  //version number
            public static string username_ = Environment.UserName;  //gets username
            public static string pcname_ = Environment.MachineName; //gets pc name
            public static string disk_ = "";                        //disk variable, its setted on the start of the app
            public static string docsdir_ = disk_ + "\\xhyaOne\\";  // gets the xhyaOne directory with the setted disk
            public static string method_ = "N/A";                   //method (you can use this for whatever you want)
            public static string linereader_;                       //main line reader
            
        }
            
        /* spinner, call with; var spin = new ConsoleSpinner();
         then do a for var:

            for (int i = 0; i < 8; ++i)
            {
                spin.Turn();
            }

        change "8" for how many times you want the spinner to spin (lol) */
            
        public class ConsoleSpinner
        {
            int counter;

            public void Turn()
            {
                counter++;
                switch (counter % 4)
                {   
                    case 0: Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("/"); counter = 0; break;
                    case 1: Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("-"); break;
                    case 2: Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("\\"); break;
                    case 3: Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("|"); break;
                }
                Thread.Sleep(100);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }

        // ------------------- ERRORS ------------------------
        
        public class error
        {
            public void errorNewLine_() //make new line after error
            {
                Console.WriteLine(Environment.NewLine);
                var mgmt = new mgmt();
                mgmt.NewLine();
            }

            public void errorMessage() //create error embed message
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("[ERROR]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" :: ");
            }

            public void value()
            {
                //use this function when someone goes out of the value limit on a command
                errorMessage();
                Console.Write("value error > out of limit || not a valid character");
                errorNewLine_();
            }

            public void syntax()
            {
                //use this function when someone commits a syntax error
                errorMessage();
                Console.Write("user input error > syntax");
                errorNewLine_();
            }

            public void HelpTip()
            {
                //use this function when someone types any command wrongly
                errorMessage();
                Console.Write("user input error > wrong command");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("yourCommand");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" ");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("h");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" for help");
                errorNewLine_();
            }

        }

        public class mgmt //management public class
        {

            public void consoleMainScreen() //shows the main screen of the console
            {
                        
                var mgmt = new mgmt(); //calling the management class

                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.Title = "xhyaOne";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("             xhyaOne");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("   -v"+ globalVariables.version_);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("   -by @xhyabunny");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(Environment.NewLine);
                
            }


            public void Cleanse() //clean console and add a new line
            {

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(globalVariables.username_ + "@" + globalVariables.pcname_);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(" method:" + globalVariables.method_); //method

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" / ");

                NewLine();

            }

            public void NewFullLine() //add a new full line 
            {

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(globalVariables.username_ + "@" + globalVariables.pcname_);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(" method" + globalVariables.method_); //method

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" / ");

                NewLine();

            }

            public void NewPrompt() //prefix of the prompt (you can change it to whatever you want)
            {
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.Write("$ ");

            }

            public void NewLine() //new line reader
            {
                
                var mgmt = new mgmt();
                       
                mgmt.NewPrompt();

                var readLine = Console.ReadLine();

                globalVariables.linereader_ = readLine.ToString().ToLower();

                mgmt.commandHandle();

            }

            // GET OS INFO
            public string HKLM_GetString(string path, string key)
            {
                try
                {
                    RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                    if (rk == null) return "";
                    return (string)rk.GetValue(key);
                }
                catch { return ""; }
            }

            // GET VERSION
            public string FriendlyName()
            {
                string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
                string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
                if (ProductName != "")
                {
                    return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                                (CSDVersion != "" ? " " + CSDVersion : "");
                }
                return "";
            }

            // ------------------------- COMMAND HANDLER --------------------------

            public void commandHandle()
            {

                var mgmt = new mgmt();
                var newLine = Environment.NewLine;
                var spin = new ConsoleSpinner();
                var error = new error();
                
                if (globalVariables.linereader_.StartsWith("test"))

                {

                    var readLine = globalVariables.linereader_.Split(' ');

                    if (readLine.Length == 2)
                    {

                        switch (readLine[1])
                        {
                            case "hello":

                                Console.WriteLine("hello!");

                                mgmt.NewLine();

                            break;

                            default:

                                error.value();

                            break;
                        }
                    }
                    else
                    {
                                 
                        Console.WriteLine("test!");

                        mgmt.NewLine();

                    }

                }

        // -------------------------------- ERROR PROMPT ----------------------------------

                else
                {

                    error.syntax();

                }
            
            }

        }

        // --------------------------------- APP START ----------------------------------

        static void Main(string[] args)
        {

            Console.Title = "made by @xhyabunny";

            //VARIABLE

            var mgmt = new mgmt(); //load management class
            var spin = new ConsoleSpinner(); //load spinenr
            var error = new error(); //load error class

            //LOAD DISKS

            DriveInfo[] drives = DriveInfo.GetDrives();

            globalVariables.disk_ = drives[0] + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
               
            Console.BackgroundColor = ConsoleColor.Green;

            Console.Write("Working... ", Console.BackgroundColor);

            Console.Title = "made by @xhyabunny [loading]";

            //FOLDERS CREATE

            if (Directory.Exists("C:\\xhyaOne\\config"))
            { 
                // nothing
            }
            else
            {
                Directory.CreateDirectory("C:\\xhyaOne\\config");
            }

            if (Directory.Exists("C:\\xhyaOne\\assets"))
            { 
                // nothing
            }
            else
            {
                Directory.CreateDirectory("C:\\xhyaOne\\assets");
            }

            if (!Directory.Exists(globalVariables.docsdir_))
            {
                Directory.CreateDirectory(globalVariables.docsdir_);
            }

            //dumb spinner thing
            for (int i = 0; i < 8; ++i)
            {
                spin.Turn();
            }

            Console.BackgroundColor = ConsoleColor.Black;

            Console.Clear();

            //MAKE SHORTCUT
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Console.Title = "xhyaOne OPEN-SOURCE [" + Environment.UserName + "]";

            mgmt.consoleMainScreen(); //intro

            Console.ReadKey();

            Console.ForegroundColor = ConsoleColor.White;
            
            mgmt.Cleanse(); //clear console + add new line

            mgmt.NewLine(); //new line
            
        }
    }
}
