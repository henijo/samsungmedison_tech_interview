﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace VideoRental
{
    public class Controls
    {
        private static Controls _instance;

        public static Controls Instance {
            get
            {
                if (_instance == null) _instance = new Controls();
                return _instance;
            }
        }
        

        private List<Customer> customerList= new List<Customer>();
        private List<Rental> rentalList = new List<Rental>();
        private List<Movie> movieList = new List<Movie>();


        public void SetMovie()
        {
            movieList.Add(new Movie("일반 1", Movie.REGULAR));
            movieList.Add(new Movie("일반 2", Movie.REGULAR));
            movieList.Add(new Movie("신작 1", Movie.NEW_RELEASE));
            movieList.Add(new Movie("신작 2", Movie.NEW_RELEASE));
            movieList.Add(new Movie("어린이 1", Movie.CHILDRENS));
            movieList.Add(new Movie("어린이 2", Movie.CHILDRENS));
            movieList.Add(new Movie("로맨스", Movie.ROMANCE));
        }


        public DataView View_VideoList()
        {
            DataTable dtVideo = new DataTable();
            dtVideo.Columns.AddRange(new DataColumn[]
            {
                new DataColumn { ColumnName = "TITLE", Caption = "제목", DataType = typeof(string) },
                new DataColumn { ColumnName = "GENRE", Caption = "장르", DataType = typeof(string) },
                new DataColumn { ColumnName = "POOINT", Caption = "포인트", DataType = typeof(int) }
            });

            for (int i = 0; i < movieList.Count; i++)
            {
                switch (movieList[i].getPriceCode())
                {
                    case Movie.REGULAR:
                        dtVideo.Rows.Add(new object[] { movieList[i].getTitle(), "Regular", 100 });
                        break;
                    case Movie.CHILDRENS:
                        dtVideo.Rows.Add(new object[] { movieList[i].getTitle(), "Children", 200 });
                        break;
                    case Movie.NEW_RELEASE:
                        dtVideo.Rows.Add(new object[] { movieList[i].getTitle(), "New Release", 300 });
                        break;

                }
            }

            DataView returnView = new DataView(dtVideo);
            return returnView;
        }

        public void Input_Rental(string customerId, string videoTitle, int period)
        {
            Customer customer = new Customer(customerId);
            Movie selectedMovie = movieList.Find(x => x.getTitle() == videoTitle);
            Rental rental = new Rental(selectedMovie, period);
            customer.addRental(rental); 
            
            customerList.Add(customer);
            rentalList.Add(rental);
        }

        public void Input_Return(string customerId, string videoTitle)
        {
            Customer customer = customerList.FindLast(c => c.getName() == customerId);
            Movie movie = movieList.FindLast(c => c.getTitle() == videoTitle);

            if (rentalList.Count == 0)
            {
                return;
            }

            if (rentalList.Contains(customer.getRental(movie)))
            {
                Rental rentalInfo = rentalList.FindLast(c => c.getMovie() == movie);
                if (rentalInfo == customer.getRental(movie))
                {
                    customer.ReturnRental(videoTitle);
                }
            }
        }

        public void SaveRecipet()
        {
            string savePath = @"../save";
            string fullPath = @"../save/recipt.txt";
            StringBuilder textValue = new StringBuilder();

            for (int i = 0; i < customerList.Count; i++)
            {
                textValue.Append(customerList[i].statement());

            }

            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }

            if (!File.Exists(fullPath))
            {
                using (File.Create(fullPath))
                {
                    System.IO.File.WriteAllText(fullPath, textValue.ToString(), Encoding.UTF8);
                    Console.WriteLine("Save Complete! (file path : {0})", Path.GetFullPath(fullPath));
                }
            }
            else
            {
                System.IO.File.WriteAllText(fullPath, textValue.ToString(), Encoding.UTF8);
                Console.WriteLine("Save Complete! (file path : {0})", Path.GetFullPath(fullPath));
            }

            

        }

        public void PrintRecipt(string customerId)
        {
            Customer customer = customerList.FindLast(c => c.getName() == customerId);
            Console.WriteLine(customer.statement());
            
        }
    }
}
