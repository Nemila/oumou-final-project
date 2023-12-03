namespace Hotel.Data;

public static class ReadFiles
{
    public static List<Customer> ReadCustomers()
    {
        List<Customer> customers = new();
        var lines = File.ReadAllLines(FindFile("Customer.txt"));

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            int roomNumber = Convert.ToInt32(parts[0]);
            string name = parts[1];
            double discount = Convert.ToDouble(parts[2]);

            Customer newCustomer = new(name, roomNumber, discount);
            customers.Add(newCustomer);
        }

        return customers;
    }
    public static List<Coupon> ReadCoupons()
    {
        List<Coupon> coupons = new();
        var lines = File.ReadAllLines(FindFile("Coupons.txt"));

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            string code = parts[0];
            double discount = Convert.ToDouble(parts[1]);
            Coupon newCoupon = new(code, discount);
            coupons.Add(newCoupon);
        }

        return coupons;
    }

    public static List<RoomPrice> ReadRoomPrices()
    {
        List<RoomPrice> roomPrices = new();
        var lines = File.ReadAllLines(FindFile("Roomprices.txt"));

        foreach (string line in lines)
        {
            string[] parts = line.Split(",");
            string type = parts[0];
            double dailyRate = Convert.ToDouble(parts[1]);
            _ = Enum.TryParse(type, out RoomType roomType);

            RoomPrice newRoomPrice = new(roomType, dailyRate);
            roomPrices.Add(newRoomPrice);
        }

        return roomPrices;
    }

    public static List<Room> ReadRooms()
    {
        List<Room> rooms = new();
        var lines = File.ReadAllLines(FindFile("Rooms.txt"));

        foreach (string line in lines)
        {
            string[] parts = line.Split(",");
            int roomNumber = Convert.ToInt32(parts[0]);
            string type = parts[1];
            _ = Enum.TryParse(type, out RoomType roomType);

            Room newRoom = new(roomNumber, roomType);
            rooms.Add(newRoom);
        }

        return rooms;
    }

    public static List<Reservation> ReadReservations()
    {
        List<Reservation> reservations = new();
        var lines = File.ReadAllLines(FindFile("Reservation.txt"));

        foreach (string line in lines)
        {
            string[] parts = line.Split(",");
            string reservationNumber = parts[0];
            string dateStartString = parts[1];
            string dateStopString = parts[2];
            int roomNumber = Convert.ToInt32(parts[3]);
            string customerName = parts[4];
            string paymentConfirmation = parts[5];

            _ = DateOnly.TryParse(dateStartString, out DateOnly reservationDateStart);
            _ = DateOnly.TryParse(dateStopString, out DateOnly reservationDateStop);
            _ = Guid.TryParse(reservationNumber, out Guid formattedGuid);

            Reservation newReservation = new(formattedGuid, reservationDateStart, reservationDateStop, roomNumber, customerName, paymentConfirmation);
            reservations.Add(newReservation);
        }

        return reservations;
    }

    public static string FindFile(string fileName)
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory != null)
        {
            var testPath = Path.Combine(directory.FullName, fileName);
            if (File.Exists(testPath)) return testPath;
            directory = directory.Parent;
        }

        throw new FileNotFoundException($"The file {fileName} was not found in the current directory or any of its parent directories.");
    }
}

