
namespace Hotel.Data
{
    public static class WriteFiles
    {
        public static void WriteCustomers()
        {
            List<string> lines = new();
            foreach (var item in FileData.customers)
            {
                lines.Add(item.ToString());
            }
            File.WriteAllLines(FindFile("Customer.txt"), lines);
        }

        public static void WriteCoupons()
        {
            List<string> lines = new();
            foreach (var item in FileData.coupons)
            {
                lines.Add(item.ToString());
            }
            File.WriteAllLines(FindFile("Coupons.txt"), lines);
        }

        public static void WriteCouponsRedemption(CouponRedemption coupon)
        {
            File.AppendAllText(FindFile("CouponsRedemption.txt"), $"{coupon}{Environment.NewLine}");
        }

        public static void WriteRoomPrices()
        {
            List<string> lines = new();
            foreach (var item in FileData.roomPrices)
            {
                lines.Add(item.ToString());
            }
            File.WriteAllLines(FindFile("RoomPrices.txt"), lines);
        }

        public static void WriteRooms()
        {
            List<string> lines = new();
            foreach (var item in FileData.rooms)
            {
                lines.Add(item.ToString());
            }
            File.WriteAllLines(FindFile("Rooms.txt"), lines);
        }

        public static void WriteReservations()
        {
            List<string> lines = new();
            foreach (var item in FileData.reservations)
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
}
