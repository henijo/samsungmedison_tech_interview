using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace VideoRental
{
  class Customer
  {
    public Customer(string name)
    {
        customerName = name;
    }

    public void addRental(Rental arg) { customerRental.Add(arg); }

    public Rental getRental(Movie movie)
    {
        return customerRental.FindLast(c => c.getMovie() == movie);}
    public string getName() { return customerName; }

    public string statement()
    {
        double totalAmount = 0.0;
        int frequentRenterPoints = 0;
        StringBuilder result = new StringBuilder();

        result.AppendLine("Rental Record for" + getName());

        List<string> reciptList = new List<string>();

        IEnumerator<Rental> enumerator = customerRental.GetEnumerator();

        for (; enumerator.MoveNext();)
        {
            double thisAmount = 0.0;
            Rental each = enumerator.Current;

            switch (each.getMovie().getPriceCode())
            {
                case Movie.REGULAR:
                    thisAmount += 2.0;
                    if (each.getDaysRented() > 2)
                        thisAmount += (each.getDaysRented() - 2) * 1.5;
                    reciptList.Add(string.Format("{0}  {1}    {2}    {3}", "REGULAR", each.getMovie().getTitle(), each.getDaysRented(), thisAmount));
                    break;
                case Movie.NEW_RELEASE:
                    thisAmount += each.getDaysRented() * 3;
                    reciptList.Add(string.Format("{0}  {1}    {2}    {3}", "NEW_RELEASE", each.getMovie().getTitle(), each.getDaysRented(), thisAmount));
                        break;

                case Movie.CHILDRENS:
                    thisAmount += 1.5;
                    if (each.getDaysRented() > 3)
                        thisAmount += (each.getDaysRented() - 3) * 1.5;
                    reciptList.Add(string.Format("{0}  {1}    {2}    {3}", "CHILDRENS", each.getMovie().getTitle(), each.getDaysRented(), thisAmount));
                        break;
                case Movie.ROMANCE:
                    thisAmount += 3.0;
                    if(each.getDaysRented() > 2)
                        thisAmount += (each.getDaysRented() - 2) * 1.5;
                    reciptList.Add(string.Format("{0}  {1}    {2}    {3}", "ROMANCE", each.getMovie().getTitle(), each.getDaysRented(), thisAmount));
                        break;
            }

            // Add frequent renter points
            frequentRenterPoints++;

            // Add bonus for a two day new release rental
            if ((each.getMovie().getPriceCode() == Movie.NEW_RELEASE)
                    && each.getDaysRented() > 1) frequentRenterPoints++;

            // Show figures for this rental
            result.AppendLine("\t" + each.getMovie().getTitle() + "\t" + thisAmount.ToString());
            totalAmount += thisAmount;
        }

        result.AppendLine("Amount owed is " + totalAmount);
        result.AppendLine("You earned " + frequentRenterPoints + " frequent renter points");

        result.AppendLine("=====================================");
        result.AppendLine("장르   제목    대여기간     가격");
        foreach (string recipt in reciptList)
        {
            result.AppendLine(recipt);
        }

        return result.ToString();
    }

    public void ReturnRental(string videoTitle)
    {
        Rental rentalInfo = customerRental.FindLast(c => c.getMovie().getTitle() == videoTitle);

        rentalInfo.setIsRented(true);
    }

    private string customerName;
    private List<Rental> customerRental = new List<Rental>();
  }
}
