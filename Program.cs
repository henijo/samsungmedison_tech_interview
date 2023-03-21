using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Windows.Forms;

namespace VideoRental
{
    class Program
    {
        static void Main(string[] args)
        {

            Controls.Instance.SetMovie();

            ConsoleKey key = ConsoleKey.NoName;

            while ((key = SelectMenu()) != ConsoleKey.Escape)
            {
                switch (key)
                {
                    case ConsoleKey.D0:
                        PrintDataView(Controls.Instance.View_VideoList());
                        break;

                    case ConsoleKey.D1:
                        InputRental();
                        Console.WriteLine("Continue? (Y/N)");
                        string isContinued = Console.ReadLine();
                        if (isContinued.ToUpper() == "Y")
                        {
                            InputRental();
                        }
                        else
                        {
                            SelectMenu();
                        }
                        break;

                    case ConsoleKey.D2:
                        InputReturn();
                        Console.WriteLine("Continue? (Y/N)");
                        isContinued = Console.ReadLine();
                        if (isContinued.ToUpper() == "Y")
                        {
                            InputReturn();
                        }
                        else
                        {
                            SelectMenu();
                        }
                        break;

                    case ConsoleKey.D3:
                        Controls.Instance.SaveRecipet();
                        Console.WriteLine("Save");
                        break;

                    case ConsoleKey.D4:
                        Console.WriteLine("Input customer ID :");
                        string customerId = Console.ReadLine();
                        Controls.Instance.PrintRecipt(customerId);
                        break;

                    case ConsoleKey.D5:
                        Environment.Exit(0);
                        break;            
                        

                    default:
                        Console.WriteLine("아무키나 누르세요.");
                        SelectMenu();
                        break;

                }

                
                Console.ReadKey();
            }
            

            /*
            Movie regular1 = new Movie("일반 1", Movie.REGULAR);
            Movie regular2 = new Movie( "일반 2", Movie.REGULAR);
            Movie newRelease1 = new Movie( "신작 1", Movie.NEW_RELEASE );
            Movie newRelease2 = new Movie( "신작 2",Movie.NEW_RELEASE );
            Movie children1 = new Movie( "어린이 1", Movie.CHILDRENS );
            Movie children2 = new Movie( "어린이 2", Movie.CHILDRENS );
            Customer customer = new Customer("고객");

            customer.addRental(new Rental( regular1, 2 ));
            customer.addRental(new Rental( regular2, 3 ));
            customer.addRental(new Rental( newRelease1, 1 ));
            customer.addRental(new Rental( newRelease2, 2 ));
            customer.addRental(new Rental( children1, 3 ));
            customer.addRental(new Rental( children2, 4 ));

            Console.Write(customer.statement());*/
            
        }

        private static ConsoleKey SelectMenu()
        {
            Console.Clear();
            Console.WriteLine("--Main Menu--");
            Console.WriteLine("0 : Print All Video Title");
            Console.WriteLine("1 : Rental	");
            Console.WriteLine("2 : Return");
            Console.WriteLine("3 : Save to File");
            Console.WriteLine("4 : Receipt");
            Console.WriteLine("5 : Exit");
            return Console.ReadKey().Key;
        }

        private static void InputRental()
        {
            Console.WriteLine("---Rental Menu-----");
            Console.WriteLine("Input customer ID :");
            string id = Console.ReadLine();
            Console.WriteLine("Input Video Title :");
            string title = Console.ReadLine();
            Console.WriteLine("Input Period : ");
            string period = Console.ReadLine();

            int i = 0;
            bool canConvert = int.TryParse(period, out i);
            
            if (canConvert == true)
            {
                Controls.Instance.Input_Rental(id, title, int.Parse(period));
                Console.WriteLine("Continue? (Y/N)");
            }
            else
            {
                Console.WriteLine("Period 는 숫자만 입력 가능합니다.");
                Console.WriteLine("Input Period : ");
                period = Console.ReadLine();
                Controls.Instance.Input_Rental(id, title, int.Parse(period));
                Console.WriteLine("Continue? (Y/N)");
            }
        }

        private static void InputReturn()
        {
            Console.WriteLine("---Return Menu-----");
            Console.WriteLine("Input customer ID :");
            string id = Console.ReadLine();
            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("Input customer ID :");
                id = Console.ReadLine();
            }
            Console.WriteLine("Input Video Title :");
            string title = Console.ReadLine();

            Controls.Instance.Input_Return(id, title);
            
        }


        private static void PrintDataView(DataView dv)
        {
            // Printing first DataRowView to demo that the row in the first index of the DataView changes depending on sort and filters
            Console.WriteLine("--Video Title List--");

            // Printing all DataRowViews
            foreach (DataRowView drv in dv)
            {
                Console.WriteLine("\t {0}", drv["TITLE"]);
            }
        }
    }
}
