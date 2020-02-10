//Author: Justin Davis
//Date: 2/4/2020
//Purpose: the Purpose of this program is to transform Mark Forsters' to-do system into a console
//      based application.
//It's not about where you start, it's where you finish.

using System;
using System.Collections.Generic;
using System.IO;

//how did you start
//challenges you faced
//What would you do differently
namespace TaskTrack
{
    class Program
    {
        //used for navigating the list in MenuList()
        public static int index = 0;
        const int pageLength = 20;

        static void Main(string[] args)
        {
            //prompts user into program
            NewOrOld();
        }

        //NewOrOld() will ask the user if they want to start a new file or use an old one
        private static void NewOrOld()
        {
            //variables
            bool isValid = false;
            string answer = "";
            
            //prompts user to enter whether or not to start a new list
            do
            {
                try
                {
                    //input
                    Console.WriteLine("Do you want to start a new list? (Y or N): ");
                    //converts user input to upper
                    answer = Console.ReadLine();
                    answer = answer.ToUpper();
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Answer, enter Y or N: ");
                }
                //Forces user to enter "Y" or "N"
                if (answer == "Y" || answer == "N")
                {
                    isValid = true;
                }
            } while (!isValid);

            if (answer == "Y")
            {
                //CreateNewList() will create a list from user's input
                CreateNewList();
            }
            else
            {
                //UseOldList() will create a list from a text file
                UseOldList();
            }
        }

        //Creates list from user input only
        public static void CreateNewList()
        {
            Console.Clear();

            //variables
            string answer = "";
            List<string> toDoList = new List<string>();

            Console.WriteLine("Please enter your items to add to the list without numbers and separated" +
                " by a new line (enter 'done' to finish): ");
            //Will continue to prompt user for input until they enter 'done'
            do
            {          
                answer = Console.ReadLine();
                if (answer != "done")
                {
                    if (answer != "")
                    {
                        toDoList.Add(answer);
                    }
                }
            } while (answer != "done");

            Console.Clear();
            ScrollList(toDoList);
   

        }

        //uses txt file to create list
        public static void UseOldList()
        {
            Console.Clear();
            //variables
            List<string> toDoList = new List<string>();
            string docPath = "C:/Users/wwstudent/source/repos/TaskTrack/TaskTrack/todo.txt";

            using (StreamReader file = new StreamReader(docPath))
            {
                string line;
                // Read and display lines from the file until the end of the file is reached.
                while ((line = file.ReadLine()) != null)
                {
                    //deletes the number, period, and space created by SaveFile
                    line = line.Substring(3, (line.Length - 3));
                    toDoList.Add(line);
                }
            }
            ScrollList(toDoList);
        }
        
        public static void ScrollList(List<string> toDoList)
        {
            Console.CursorVisible = false;
            while (true)
            {
                //runs forever
                MenuList(toDoList);
            }
        }

        //used for navigating list
        public static string MenuList(List<string> toDoList)
        {
            
            //checks if first item in list is marked as worked
            for (int i = 0; i < toDoList.Count; i++)
            {
                string item = toDoList[0];
                if (item.Length >= 4 && ((toDoList[0])[item.Length - 2] == 'D') && (toDoList[0])[item.Length - 3] == '(' && (toDoList[0])[item.Length - 1] == ')')
                {
                    //removes the items in the list if they're worked and at the beginning of the display
                    toDoList.RemoveAt(0);
                }
            }

            Console.Clear();

            //prints to do list
            Console.WriteLine("Your to do list is: \n");
           
            var page =  index / pageLength;
            var startingPoint = page * pageLength;

            for (int i = startingPoint; (i < startingPoint + pageLength) && (i < toDoList.Count); i++)
            {
                //used to hide hidden characters
                string item = toDoList[i];
                
                if (i == index)
                {
                    //highlights specific item user is on
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;

                    //will hide hidden characters if item in list ends with (D)
                    if (item.Length >= 4 && item[item.Length - 2] == 'D' && item[item.Length - 3] == '(' && item[item.Length - 1] == ')')
                    {
                        item = toDoList[i].Substring(0, (toDoList[i].Length - 3));
                        Console.WriteLine(item);
                    }
                    else
                    {
                        Console.WriteLine(toDoList[i]);
                    }
                }

                //displays items marked as complete with a darkgray foreground
                else
                {
                    //checks item if it's marked as complete, if it does it only shows the item without the (D)
                    item = toDoList[i];
                                      
                    if (item.Length >= 4 && item[item.Length - 2] == 'D' && item[item.Length - 3] == '(' && item[item.Length - 1] == ')')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        item = toDoList[i].Substring(0, (toDoList[i].Length - 3));
                        Console.WriteLine(item, Console.ForegroundColor);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(toDoList[i]);
                    }
                    
                } 
                //Resets colors for next WriteLine statement
                Console.ResetColor();
            }

            //Shows users the options and takes input
            Console.WriteLine();
            Console.WriteLine("A: add task | S: Save and quit | W: action | Down Arrow: Down | Up arrow: Up");
            ConsoleKeyInfo userKey = Console.ReadKey();

            //brings user to new menu for working the highlighted object
            if (userKey.Key == ConsoleKey.W)
            {
                Console.Clear();
                Console.WriteLine(toDoList[index]);
                Console.WriteLine("C for complete | D for complete and re-add | ");
                Console.CursorVisible = true;
                userKey = Console.ReadKey();

                //marks item as complete
                if (userKey.Key == ConsoleKey.C)
                {
                    toDoList[index] = toDoList[index] + "(D)";
                    MenuList(toDoList);
                }
                //marks item as complete and re-adds the item to the list
                else if(userKey.Key == ConsoleKey.D)
                {
                    toDoList.Add(toDoList[index]);
                    toDoList[index] = toDoList[index] + "(D)";
                    MenuList(toDoList); 
                }
            }

            //allows user to add the new item
            if (userKey.Key == ConsoleKey.A)
            {
                Console.Clear();
                Console.WriteLine("What would you like to add? ");
                Console.CursorVisible = true;
                string item = Console.ReadLine();
                toDoList.Add(item);
                MenuList(toDoList);
                Console.CursorVisible = false;
            }
            
            //allows user to move highlighter.  Index changes based on keystrokes
            else if (userKey.Key == ConsoleKey.DownArrow)
            {
                if (index == toDoList.Count - 1)
                {
                    //brings user back to top of menu
                    index = 0;
                }
                //moves highlighter down
                else { index++; }
            }

            //similar to when DownArrow is pushed only the count goes up
            else if (userKey.Key == ConsoleKey.UpArrow)
            {
                if (index <= 0)
                {
                    index = toDoList.Count - 1;
                }
                else
                {
                    index--;
                }
            }

            //will call SaveFile()
            else if (userKey.Key == ConsoleKey.S)
            {
                Console.Clear();
                SaveFile(toDoList);          
            }

            //does nothing if user presses wrong key
            Console.Clear();
            return "";
        }

        //Saves file to txt document in path
        public static void SaveFile(List<string> toDoList)
        {
            //variables
            string docPath = "C:/Users/wwstudent/source/repos/TaskTrack/TaskTrack/todo.txt";
            StreamWriter file = File.CreateText(docPath);
            string line;

            //Adds lines to text file, i is one so that the count doesn't start at 0 on text file
            for (int i = 1; i <= toDoList.Count; i++)
            {
                line = i.ToString() + ". " + toDoList[i - 1];
                file.WriteLine(line);
            }
             
            //closes the text file, sends user a message, exits program
            file.Close();
            Console.WriteLine("Your data has been saved.");
            Environment.Exit(0);
        }
    }
}