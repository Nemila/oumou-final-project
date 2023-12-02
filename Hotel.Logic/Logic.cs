using Hotel.Data;
namespace Hotel.Logic;

public class Logic
{
    //public static List<Reservation> reservations = ReadFiles.ReadReservations();
    //public static List<Room> rooms = ReadFiles.ReadRooms();
    //public static List<Customer> customers = ReadFiles.ReadCustomers();
    //public static List<Coupon> coupons = ReadFiles.ReadCoupons();
    //public static List<RoomPrice> roomPrices = ReadFiles.ReadRoomPrices();
    //public static List<(string couponCode, Guid reservationNumber)> couponRedemption = new();

    public static bool CanReserveRoom(int roomNum, DateOnly startDate, DateOnly endDate)
    {
        try
        {
            if (!FileData.rooms.Exists(r => r.Number == roomNum)) return false;

            foreach (var r in FileData.reservations)
            {
                bool IsStartDateInvalid = startDate >= r.ReservationDateStart && r.ReservationDateStop >= startDate;
                bool IsStopDateInvalid = endDate >= r.ReservationDateStart && r.ReservationDateStop >= endDate;
                bool IsDateInvalid = (r.ReservationDateStart >= startDate && r.ReservationDateStop <= endDate) || IsStartDateInvalid || IsStopDateInvalid;
                if (r.RoomNumber == roomNum && IsDateInvalid) return false;
            }

            return true;
        } catch { return false; }
    }
    
    public static bool TestableMakeReservation(int roomNumber, DateOnly dateStart, DateOnly dateStop, string customerName)
    {
        try
        {
            Guid resNumber = Guid.NewGuid();
            
            if (!(dateStart <= dateStop)) return false;
            if (!CanReserveRoom(roomNumber, dateStart, dateStop)) return false;

            Room currentRoom = FileData.rooms.Find(r => r.Number == roomNumber)!;
            RoomPrice currentRoomPrice = FileData.roomPrices.Find(r => r.Type == currentRoom.Type)!;

            Reservation newReservation = new(resNumber, dateStart, dateStop, roomNumber, customerName, GeneratePaymentConfirmationNumber(), 0);
            FileData.reservations.Add(newReservation);
            return true;
        }
        catch { return false; }
    }

    public static bool TestableDeleteReservation(Guid reservationNumber)
    {
        try
        {
            if (!FileData.reservations.Exists(r => r.ReservationNumber == reservationNumber)) return false;
            Reservation toDelete = FileData.reservations.Find(r => r.ReservationNumber == reservationNumber)!;
            FileData.reservations.Remove(toDelete);
            return true;
        }
        catch { return false; }
    }

    public static bool TestableMakeRoom(int roomNumber, RoomType roomType)
    {
        if (FileData.rooms.Exists(r => r.Number == roomNumber)) return false;
        Room newRoom = new(roomNumber, roomType);
        FileData.rooms.Add(newRoom);
        return true;
    }

    public static bool TestableCreateCustomer(string name, int cardNumber)
    {
        try {
            if (FileData.customers.Exists(c => c.Name == name)) return false;
            Customer newCustomer = new(name, cardNumber, FrequentTravelerDiscount(name));
            FileData.customers.Add(newCustomer);
            return true;
        } catch
        {
            return false;
        }
    }

    public static bool TestableDeleteCustomer(string name, int cardNumber)
    {
        try
        {
            if (!FileData.customers.Exists(c => c.Name == name)) return false;
            Customer customer = FileData.customers.Find(c => c.Name == name && c.RoomNumber == cardNumber)!;
            FileData.customers.Add(customer);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static double FrequentTravelerDiscount(string customerName)
    {
        if (!FileData.customers.Exists(c => c.Name == customerName)) return 0;

        int customerFrequency = FileData.reservations.FindAll(r => r.CustomerName == customerName).Count;

        if (customerFrequency > 3 && customerFrequency < 6) return .15;
        if (customerFrequency > 8 && customerFrequency < 12) return .25;
        if (customerFrequency > 12) return .50;
        return 0;
    }

    public static bool TestableChangeRoomPrice(RoomType type, double dailyRate)
    {
        try {
            if (dailyRate < 0) { return false; }
            if (!FileData.roomPrices.Exists(r => r.Type == type)) { return false; }
            
            RoomPrice current = FileData.roomPrices.Find(r => r.Type == type)!;
            current.DailyRate = dailyRate;
            
            return true;
        } catch { 
            return false; 
        }
    }

    public static Reservation? TestableRefundReservation(Guid reservationNumber)
    {
        try
        {
            if (!FileData.reservations.Exists(r => r.ReservationNumber == reservationNumber)) return null;
            Reservation deletedReservation = FileData.reservations.Find(r => r.ReservationNumber == reservationNumber)!;
            FileData.reservations.RemoveAt(FileData.reservations.FindIndex(r => r.ReservationNumber == reservationNumber));
            return deletedReservation;
        } catch { return null; }
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

