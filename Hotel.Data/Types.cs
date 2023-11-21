namespace Hotel.Data;

public enum RoomType { Single = 1, Double = 2, Suite = 3 };

public class Reservation
{
    public Guid ReservationNumber { get; set; }
    public DateOnly ReservationDateStart { get; set; }
    public DateOnly ReservationDateStop { get; set; }
    public int RoomNumber { get; set; }
    public string CustomerName { get; set; }
    public string PaymentConfirmation { get; set; }
    public double AmountPayed { get; set; }

    public Reservation(Guid reservationNumber, DateOnly reservationDateStart, DateOnly reservationDateStop, int roomNumber, string customerName, string paymentConfirmation, double amountPayed)
    {
        ReservationNumber = reservationNumber;
        ReservationDateStart = reservationDateStart;
        ReservationDateStop = reservationDateStop;
        RoomNumber = roomNumber;
        CustomerName = customerName;
        PaymentConfirmation = paymentConfirmation;
        AmountPayed = amountPayed;
    }
    public override string ToString()
    {
        return $"{ReservationNumber},{ReservationDateStart},{ReservationDateStop},{RoomNumber},{CustomerName},{PaymentConfirmation}";
    }
    public void Display()
    {
        Console.WriteLine("Reservation Information:");
        Console.WriteLine($"- Reservation ID: {ReservationNumber}");
        Console.WriteLine($"- From Date: {ReservationDateStart}");
        Console.WriteLine($"- To Date: {ReservationDateStop}");
        Console.WriteLine($"- Room Number: {RoomNumber}");
        Console.WriteLine($"- Guest Name: {CustomerName.ToUpper()}");
        Console.WriteLine($"- Confirmation Code: {PaymentConfirmation}");
        Console.WriteLine($"- Amount Paid: {AmountPayed}");
    }
}

public class Customer
{
    public string Name;
    public int RoomNumber;
    public double Discount;

    public Customer(string name, int roomNumber, double discount)
    {
        Name = name;
        RoomNumber = roomNumber;
        Discount = discount;
    }

    public override string ToString()
    {
        return $"{RoomNumber},{Name}";
    }
}

public class Coupon
{
    public string Code;
    public double Discount;

    public Coupon(string code, double discount)
    {
        Code = code;
        Discount = discount;
    }

    public override string ToString()
    {
        return $"{Code},{Discount}";
    }
}

public class CouponRedemption
{
    public string Code;
    public Guid ReservationNumber;

    public CouponRedemption(string code, Guid reservationNumber)
    {
        Code = code;
        ReservationNumber = reservationNumber;
    }

    public override string ToString()
    {
        return $"{Code},{ReservationNumber}";
    }
}

public class RoomPrice
{
    public RoomType Type { get; set; }
    public double DailyRate { get; set; }

    public RoomPrice(RoomType type, double dailyRate)
    {
        Type = type;
        DailyRate = dailyRate;
    }

    public override string ToString()
    {
        return $"{Type},{DailyRate}";
    }

    public void Display()
    {
        Console.WriteLine($"Room Type: {Type} - Daily Rate: {DailyRate}");
    }
}

public class Room
{
    public int Number { get; set; }
    public RoomType Type { get; set; }

    public Room(int number, RoomType type)
    {
        Number = number;
        Type = type;
    }

    public override string ToString()
    {
        return $"{Number},{Type}";
    }

    public void Display()
    {
        Console.WriteLine($"Room Number: {Number} - Room Type: {Type}");
    }
}