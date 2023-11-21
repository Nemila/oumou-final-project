using Hotel.Data;
namespace Hotel.Logic;

public class Logic
{
    public static List<Reservation> reservations = new();

    public static List<Room> rooms = new();

    public static List<Customer> customers = new();

    public static List<RoomPrice> roomPrices = new();

    // Methods
    public static void Init()
    {
        reservations = ReadFiles.ReadReservations();
        rooms = ReadFiles.ReadRooms();
        customers = ReadFiles.ReadCustomers();
        roomPrices = ReadFiles.ReadRoomPrices();
    }

    public static bool CanReserveRoom(int roomNum, DateOnly startDate, DateOnly endDate)
    {
        foreach (var r in reservations)
        {
            bool IsStartDateInvalid = startDate >= r.ReservationDateStart && r.ReservationDateStop >= startDate;
            bool IsStopDateInvalid = endDate >= r.ReservationDateStart && r.ReservationDateStop >= endDate;
            bool IsDateInvalid = (r.ReservationDateStart >= startDate && r.ReservationDateStop <= endDate) || IsStartDateInvalid || IsStopDateInvalid;
            if (r.RoomNumber == roomNum && IsDateInvalid) return false;
        }

        return true;
    }

    public static bool MakeReservation()
    {
        int roomNumber = GetInt("Enter Room Number: ");
        DateOnly reservationDateStart = GetDate("Enter a start date (mm/dd/yyyy): ");
        DateOnly reservationDateStop = GetDate("Enter a stop date (mm/dd/yyyy): ");

        while (!(reservationDateStart <= reservationDateStop))
        {
            reservationDateStart = GetDate("Enter a valid start date (mm/dd/yyyy): ");
            reservationDateStop = GetDate("Enter a valid stop date (mm/dd/yyyy): ");
        }

        Guid reservationNumber = Guid.NewGuid();

        if (!CanReserveRoom(roomNumber, reservationDateStart, reservationDateStop))
        {
            Console.WriteLine("Room is taken on that date");
            return false;
        }

        Console.Write("Enter Customer Name: ");
        string customerName = Console.ReadLine()?.ToLower() ?? "";

        while (customerName == "")
        {
            Console.WriteLine("Enter a Valid Customer Name: ");
            customerName = Console.ReadLine()?.ToLower() ?? "";
        }

        Reservation newReservation = new(reservationNumber, reservationDateStart, reservationDateStop, roomNumber, customerName, GeneratePaymentConfirmationNumber());

        Console.WriteLine("Reservation made with success");
        reservations.Add(newReservation);
        WriteFiles.WriteReservations(reservations);

        return true;
    }

    public static Room MakeRoom()
    {
        int roomNumber = GetInt("Enter the room's number: ");
        while (rooms.Exists(r => r.Number == roomNumber))
        {
            roomNumber = GetInt("Room already exists. Enter a different room number: ");
        }

        Console.Write("Select Room Type (1. Single, 2. Double, 3. Suite): ");
        RoomType roomType;
        while (!Enum.TryParse(Console.ReadLine(), true, out roomType))
        {
            Console.Write("Select Room Type (1. Single, 2. Double, 3. Suite): ");
        }

        Room newRoom = new(roomNumber, roomType);
        rooms.Add(newRoom);
        WriteFiles.WriteRooms(rooms);
        return newRoom;
    }

    public static List<Room> GetAvailableRoomsByDate()
    {
        DateOnly reservationDateStart = GetDate("Enter a start date (mm/dd/yyyy): ");
        DateOnly reservationDateStop = GetDate("Enter a stop date (mm/dd/yyyy): ");

        while (!(reservationDateStart <= reservationDateStop))
        {
            reservationDateStart = GetDate("Enter a valid start date (mm/dd/yyyy): ");
            reservationDateStop = GetDate("Enter a valid stop date (mm/dd/yyyy): ");
        }

        return rooms.FindAll(r => CanReserveRoom(r.Number, reservationDateStart, reservationDateStop));

        // DateOnly date = GetDate();
        // List<Reservation> reservationsOnDaDate = reservations.FindAll(r => r.ReservationDateStop < date && date < r.ReservationDateStart);
        // foreach (var reservation in reservationsOnDaDate) occupiedRoomsNumber.Add(reservation.RoomNumber);

        // List<Room> freeRooms = rooms.FindAll(r => !occupiedRoomsNumber.Contains(r.Number));
        // List<Room> occupiedRooms = rooms.FindAll(r => occupiedRoomsNumber.Contains(r.Number));

        // roomsAvailability[0] = freeRooms;
        // roomsAvailability[1] = occupiedRooms;

        // return roomsAvailability;
    }

    public static List<Reservation> GetReservationReport()
    {
        DateOnly reservationDateStart = GetDate("Enter a start date (mm/dd/yyyy): ");
        return reservations.FindAll(r => reservationDateStart < r.ReservationDateStart && reservationDateStart > r.ReservationDateStop);
    }

    public static List<Reservation> GetCustomerReservationReport()
    {
        Console.Write("Enter Customer Name: ");
        string name = Console.ReadLine()?.ToLower() ?? "";
        while (name == "")
        {
            Console.Write("Please enter a valide name: ");
            name = Console.ReadLine()?.ToLower() ?? "";
        }

        if (reservations.Exists(r => r.CustomerName == name))
        {
            return reservations.FindAll(r => r.CustomerName.ToLower() == name && r.ReservationDateStart > DateOnly.FromDateTime(DateTime.Now));
        }
        else
        {
            return new List<Reservation>();
        }
    }

    public static Customer CreateCustomer()
    {
        Console.Write("Enter Customer Name: ");
        string name = Console.ReadLine()?.ToLower() ?? "";
        int cardNumber = GetInt("Enter Customer Card Number: ");

        while (name == "")
        {
            Console.WriteLine("Please enter a valide name: ");
            name = Console.ReadLine()?.ToLower() ?? "";
        }

        Customer newCustomer = new(name, cardNumber);
        customers.Add(newCustomer);
        WriteFiles.WriteCustomers(customers);
        return newCustomer;
    }

    public static void ChangeRoomPrice()
    {
        Console.Write("Select Room Type (1. Single, 2. Double, 3. Suite): ");
        RoomType roomType;
        while (!Enum.TryParse(Console.ReadLine(), true, out roomType))
        {
            Console.Write("Select Valid Room Type (1. Single, 2. Double, 3. Suite): ");
        }

        if (roomPrices.Exists(r => r.Type == roomType))
        {
            RoomPrice selectedRoomPrice = roomPrices.Find(r => r.Type == roomType)!;
            selectedRoomPrice.DailyRate = GetInt("Enter Room Daily Rate: ");
            WriteFiles.WriteRoomPrices(roomPrices);
        }
        else
        {
            Console.WriteLine("Room Type Not Available");
        }
    }

    public static List<Reservation> GetCustomerPriorReservation()
    {
        Console.Write("Enter Customer Name: ");
        string name = Console.ReadLine()?.ToLower() ?? "";

        while (name == "")
        {
            Console.Write("Please enter a valide name: ");
            name = Console.ReadLine()?.ToLower() ?? "";
        }

        if (reservations.Exists(r => r.CustomerName == name))
        {
            return reservations.FindAll(r => r.CustomerName.ToLower() == name && r.ReservationDateStart < DateOnly.FromDateTime(DateTime.Now));
        }
        else
        {
            Console.WriteLine("Customer Not Found");
            return new List<Reservation>();
        }
    }

    public static void RefundReservation()
    {
        Console.Write("Enter Reservation Number: ");
        Guid reservationNumber;
        while (!Guid.TryParse(Console.ReadLine(), out reservationNumber))
        {
            Console.Write("Enter Valid Reservation Number: ");
        }

        if (reservations.Exists(r => r.ReservationNumber == reservationNumber))
        {
            Reservation deletedReservation = reservations.Find(r => r.ReservationNumber == reservationNumber);

            reservations.RemoveAt(reservations.FindIndex(r => r.ReservationNumber == reservationNumber));

            WriteFiles.WriteRefund(deletedReservation);
            WriteFiles.WriteReservations(reservations);
        }
        else
        {
            Console.Write("Reservation was not found.");
        }
    }

    public static int GetInt(string message)
    {
        Console.Write(message);
        string? response = Console.ReadLine();
        int result;

        while (!int.TryParse(response, out result))
        {
            Console.Write("Please enter a valid number: ");
            response = Console.ReadLine();
        }

        return result;
    }

    static DateOnly GetDate(string message = "Enter a date (mm/dd/yyyy): ")
    {
        while (true)
        {
            try
            {
                Console.Write(message);
                string dateString = Console.ReadLine() ?? "";
                string[] dateArray = dateString.Split("/");

                int month = Convert.ToInt32(dateArray[0]);
                int date = Convert.ToInt32(dateArray[1]);
                int year = Convert.ToInt32(dateArray[2]);

                return new DateOnly(year, month, date);
            }
            catch
            {
                Console.Write("Invalid date, please enter a valid date (mm/dd/yyyy): ");
            }
        }
    }

    static string GeneratePaymentConfirmationNumber()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        Random random = new();
        int randomNumber = random.Next(1000, 9999);

        string paymentConfirmationNumber = timestamp + randomNumber.ToString();
        return paymentConfirmationNumber;
    }
}

