namespace Hotel.Data
{
    public static class FileData
    {
        public static List<Reservation> reservations = ReadFiles.ReadReservations();

        public static List<Room> rooms = ReadFiles.ReadRooms();

        public static List<Customer> customers = ReadFiles.ReadCustomers();

        public static List<Coupon> coupons = ReadFiles.ReadCoupons();

        public static List<RoomPrice> roomPrices = ReadFiles.ReadRoomPrices();

        public static List<(string couponCode, Guid reservationNumber)> couponRedemption = new();
    }
}
