namespace Hotel.Data;

public static class ReadFiles
{
    public static List<Customer> ReadCustomers()
    {
        List<Customer> customers = new();
        string data = File.ReadAllText(FindFile("Customer.txt"));

        foreach (string line in data.Split("#", StringSplitOptions.RemoveEmptyEntries))
        {
            int roomNumber = Convert.ToInt32(line.Split(",")[0].Trim());
            string name = line.Split(",")[1].Trim();
            double discount = Convert.ToDouble(line.Split(",")[2].Trim());

            Customer newCustomer = new(name, roomNumber, discount);
            customers.Add(newCustomer);
        }

        return customers;
    }
    public static List<Coupon> ReadCoupons()
    {
        List<Coupon> coupons = new();
        string data = File.ReadAllText(FindFile("Coupons.txt"));

        foreach (string line in data.Split("#", StringSplitOptions.RemoveEmptyEntries))
        {
            string code = line.Split(",")[0].Trim();
            double discount = Convert.ToDouble(line.Split(",")[1].Trim());

            Coupon newCoupon = new(code, discount);
            coupons.Add(newCoupon);
        }

        return coupons;
    }

    public static List<RoomPrice> ReadRoomPrices()
    {
        List<RoomPrice> roomPrices = new();
        string data = File.ReadAllText(FindFile("Roomprices.txt"));

        foreach (string line in data.Split("#", StringSplitOptions.RemoveEmptyEntries))
        {
            double dailyRate = Convert.ToDouble(line.Split(",")[1].Trim());
            string type = line.Split(",")[0].Trim();
            _ = Enum.TryParse(type, out RoomType roomType);

            RoomPrice newRoomPrice = new(roomType, dailyRate);
            roomPrices.Add(newRoomPrice);
        }

        return roomPrices;
    }

    public static List<Room> ReadRooms()
    {
        List<Room> rooms = new();
        string data = File.ReadAllText(FindFile("Rooms.txt"));

        foreach (string line in data.Split("#", StringSplitOptions.RemoveEmptyEntries))
        {
            int roomNumber = Convert.ToInt32(line.Split(",")[0].Trim());
            string type = line.Split(",")[1].Trim();
            _ = Enum.TryParse(type, out RoomType roomType);

            Room newRoom = new(roomNumber, roomType);
            rooms.Add(newRoom);
        }

        return rooms;
    }

    public static List<Reservation> ReadReservations()
    {
        List<Reservation> reservations = new();
        string data = File.ReadAllText(FindFile("Reservation.txt"));

        foreach (string line in data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
        {
            string reservationNumber = line.Split(",")[0].Trim();
            string dateStartString = line.Split(",")[1].Trim();
            string dateStopString = line.Split(",")[2].Trim();
            int roomNumber = Convert.ToInt32(line.Split(",")[3].Trim());
            string customerName = line.Split(",")[4].Trim();
            string paymentConfirmation = line.Split(",")[5].Trim();

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

