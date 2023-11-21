namespace Hotel.Data;

public static class WriteFiles
{
    public static void WriteCustomers(List<Customer> customers)
    {
        List<string> lines = new();
        foreach (var item in customers)
        {
            lines.Add(item.ToString());
        }
        File.WriteAllLines(FindFile("Customer.txt"), lines);
    }

    public static void WriteCoupons(List<Coupon> coupons)
    {
        List<string> lines = new();
        foreach (var item in coupons)
        {
            lines.Add(item.ToString());
        }
        File.WriteAllLines(FindFile("Coupons.txt"), lines);
    }

    public static void WriteCouponsRedemption(CouponRedemption coupon)
    {
        File.AppendAllText(FindFile("CouponsRedemption.txt"), $"{coupon}{Environment.NewLine}");
    }

    public static void WriteRoomPrices(List<RoomPrice> roomPrices)
    {
        List<string> lines = new();
        foreach (var item in roomPrices)
        {
            lines.Add(item.ToString());
        }
        File.WriteAllLines(FindFile("RoomPrices.txt"), lines);
    }

    public static void WriteRooms(List<Room> rooms)
    {
        List<string> lines = new();
        foreach (var item in rooms)
        {
            lines.Add(item.ToString());
        }
        File.WriteAllLines(FindFile("Rooms.txt"), lines);
    }

    public static void WriteReservations(List<Reservation> reservations)
    {
        List<string> lines = new();
        foreach (var item in reservations)
        {
            lines.Add(item.ToString());
        }
        File.WriteAllLines(FindFile("Reservation.txt"), lines);
    }

    public static void WriteRefund(Reservation reservation)
    {
        File.AppendAllText(FindFile("Refunds.txt"), $"{reservation}{Environment.NewLine}");
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

