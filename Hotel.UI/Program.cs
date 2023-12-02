using Hotel.Logic;
using Hotel.Data;

class Program
{
    static void Main()
    {
        bool exit = false;

        do
        {
            Console.Clear();

            Console.WriteLine("/---------------- Hotel Management System ----------------\\\n");
            Console.WriteLine("Main Menu:");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Room Management");
            Console.WriteLine("2. Reservation Management");
            Console.WriteLine("3. Customer Management");
            Console.WriteLine("4. Price Management");

            Console.Write("Enter your choice (0 - 4): ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 0:
                        exit = true;
                        Console.WriteLine("Exiting the program. Goodbye!");
                        break;

                    case 1:
                        RoomManagementMenu();
                        break;

                    case 2:
                        ReservationManagementMenu();
                        break;

                    case 3:
                        CustomerManagementMenu();
                        break;

                    case 4:
                        PriceManagementMenu();
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        } while (!exit);
    }

    static void RoomManagementMenu()
    {
        Console.Clear();
        Console.WriteLine("/---------------- Room Management Menu ----------------\\\n");
        Console.WriteLine("1. Make a Room");
        Console.WriteLine("2. Display Room Prices");
        Console.WriteLine("3. Available Room Search");
        Console.Write("Enter your choice (1 - 3): ");

        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("/---------------- Adding a new room ----------------\\\n");
                    
                    try
                    {
                        int roomNumber = GetInt("Enter the room's number: ");

                        while (FileData.rooms.Exists(r => r.Number == roomNumber))
                        {
                            roomNumber = GetInt("Room already exists. Enter a different room number: ");
                        }

                        Console.Write("Select Room Type (1. Single, 2. Double, 3. Suite): ");
                        RoomType roomType;
                        while (!Enum.TryParse(Console.ReadLine(), true, out roomType))
                        {
                            Console.Write("Select Room Type (1. Single, 2. Double, 3. Suite): ");
                        }

                        if (Logic.TestableMakeRoom(roomNumber, roomType))
                        {
                            Console.WriteLine("Room created with success");
                            WriteFiles.WriteRooms();
                        } else
                        {
                            Console.WriteLine("Something went wrong");
                        }
                    }
                    catch { Console.WriteLine("Something went wrong"); }
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("/---------------- Room Prices ----------------\\\n");
                    FileData.roomPrices.ForEach(r => r.Display());
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("/---------------- Available Room Search ----------------\\\n");

                    DateOnly reservationDateStart = GetDate("Enter a start date (mm/dd/yyyy): ");
                    DateOnly reservationDateStop = GetDate("Enter a stop date (mm/dd/yyyy): ");

                    while (!(reservationDateStart <= reservationDateStop))
                    {
                        reservationDateStart = GetDate("Enter a valid start date (mm/dd/yyyy): ");
                        reservationDateStop = GetDate("Enter a valid stop date (mm/dd/yyyy): ");
                    }

                    List<Room> roomsAvailability = FileData.rooms.FindAll(r => Logic.CanReserveRoom(r.Number, reservationDateStart, reservationDateStop));

                    if (roomsAvailability.Count <= 0)
                    {
                        Console.WriteLine("No available rooms for that date.");
                    }
                    else
                    {
                        roomsAvailability.ForEach(r => r.Display());
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    static void ReservationManagementMenu()
    {
        Console.Clear();
        Console.WriteLine("Reservation Management Menu:\n");
        Console.WriteLine("1. Make a Reservation");
        Console.WriteLine("2. Delete a Reservation");
        Console.WriteLine("3. Reservations Report");
        Console.WriteLine("4. Refund a Reservations");

        Console.Write("Enter your choice (1 - 4): ");
        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("/---------------- Make a Reservation ----------------\\\n");
                    
                    try
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

                        Console.Write("Enter Customer Name: ");
                        string customerName = Console.ReadLine()?.ToLower() ?? "";

                        while (customerName == "")
                        {
                            Console.WriteLine("Enter a Valid Customer Name: ");
                            customerName = Console.ReadLine()?.ToLower() ?? "";
                        }

                        if (Logic.TestableMakeReservation(roomNumber, reservationDateStart, reservationDateStop, customerName))
                        {
                            Console.WriteLine("Reservation made with success");
                            WriteFiles.WriteReservations();
                        } else
                        {
                            Console.WriteLine("Something went wrong");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong");
                    }
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("/---------------- Delete a Reservation ----------------\\\n");

                    try
                    {
                        Console.Write("Enter Reservation Number: ");

                        Guid reservationNumber;

                        while(!Guid.TryParse(Console.ReadLine(), out reservationNumber))
                        {
                            Console.Write("Enter Reservation Number: ");
                        }


                        if (Logic.TestableDeleteReservation(reservationNumber))
                        {
                            Console.WriteLine("Reservation deleted with success");
                            WriteFiles.WriteReservations();
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong");
                    }

                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("/---------------- Reservations Report ----------------\\\n");
                    FileData.reservations.ForEach(r => { r.Display(); Console.WriteLine(""); });
                    break;

                case 4:
                    Console.Clear();
                    Console.WriteLine("/---------------- Refund a Reservations ----------------\\\n");

                    try
                    {
                        Console.Write("Enter Reservation Number: ");
                        Guid reservationNumber;

                        while (!Guid.TryParse(Console.ReadLine(), out reservationNumber))
                        {
                            Console.Write("Enter Valid Reservation Number: ");
                        }

                        var deleted = Logic.TestableRefundReservation(reservationNumber);
                        
                        if (deleted != null){
                            Console.WriteLine("Refunded with success");
                            WriteFiles.WriteRefund(deleted);
                            WriteFiles.WriteReservations();
                        } else
                        {
                            Console.WriteLine("Something went wrong");
                        }
                    }
                    catch
                    {
                        Console.Write("Something went wrong");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    static void CustomerManagementMenu()
    {
        Console.Clear();
        Console.WriteLine("/---------------- Customer Management Menu ----------------\\\n");
        Console.WriteLine("1. Add a Customer");
        Console.WriteLine("2. Delete a Customer");
        Console.WriteLine("3. Customer Reservations Report");
        Console.WriteLine("4. Customer Prior Reservations");
        Console.WriteLine("5. Frequent Traveler Discount");

        Console.Write("Enter your choice (1 - 5): ");
        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("/---------------- Add a Customer ----------------\\\n");
                    try
                    {
                        Console.Write("Enter Customer Name: ");
                        string newCustomerName = Console.ReadLine()?.ToLower() ?? "";
                        int cardNumber = GetInt("Enter Customer Card Number: ");

                        while (newCustomerName == "")
                        {
                            Console.WriteLine("Please enter a valide name: ");
                            newCustomerName = Console.ReadLine()?.ToLower() ?? "";
                        }
                        
                        if (Logic.TestableCreateCustomer(newCustomerName, cardNumber))
                        {
                            Console.WriteLine("Customer Added With Success");
                            WriteFiles.WriteCustomers();
                        } else
                        {
                            Console.WriteLine("Something went wrong");
                        }
                    } catch
                    {
                        Console.WriteLine("Something went wrong");
                    }
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("/---------------- Delete a Customer ----------------\\\n");
                    try
                    {
                        Console.Write("Enter Customer Name: ");
                        string toDeleteCustomerName = Console.ReadLine()?.ToLower() ?? "";
                        int toDeleteCardNumber = GetInt("Enter Customer Card Number: ");

                        while (toDeleteCustomerName == "")
                        {
                            Console.WriteLine("Please enter a valide name: ");
                            toDeleteCustomerName = Console.ReadLine()?.ToLower() ?? "";
                        }

                        if (Logic.TestableDeleteCustomer(toDeleteCustomerName, toDeleteCardNumber))
                        {
                            Console.WriteLine("Customer Deleted With Success");
                            WriteFiles.WriteCustomers();
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong");
                    }
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("/---------------- Customer Reservations Report ----------------\\\n");

                    List<Reservation> customerReservations;

                    Console.Write("Enter Customer Name: ");
                    string name = Console.ReadLine()?.ToLower() ?? "";
                    while (name == "")
                    {
                        Console.Write("Please enter a valide name: ");
                        name = Console.ReadLine()?.ToLower() ?? "";
                    }

                    if (FileData.reservations.Exists(r => r.CustomerName == name))
                    {
                        customerReservations = FileData.reservations.FindAll(r => r.CustomerName.ToLower() == name && r.ReservationDateStart > DateOnly.FromDateTime(DateTime.Now));
                        if (customerReservations.Count <= 0)
                        {
                            Console.WriteLine("No reservations for that customer.");
                        }
                        else
                        {
                            customerReservations.ForEach(r => { r.Display(); Console.WriteLine(""); });
                        }
                    }
                    else
                    {
                        Console.WriteLine("Customer not found.");
                    }

                    
                    break;

                case 4:
                    Console.Clear();
                    Console.WriteLine("/---------------- Customer Prior Reservations ----------------\\\n");

                    List<Reservation> customerPriorReservations;

                    Console.Write("Enter Customer Name: ");
                    string customerName = Console.ReadLine()?.ToLower() ?? "";

                    while (customerName == "")
                    {
                        Console.Write("Please enter a valide name: ");
                        name = Console.ReadLine()?.ToLower() ?? "";
                    }

                    if (FileData.reservations.Exists(r => r.CustomerName.ToLower() == customerName))
                    {
                        customerPriorReservations = FileData.reservations.FindAll(r => r.CustomerName.ToLower() == customerName && r.ReservationDateStop < DateOnly.FromDateTime(DateTime.Now));
                        
                        if (customerPriorReservations.Count <= 0)
                        {
                            Console.WriteLine("No reservations for that customer.");
                        }
                        else
                        {
                            customerPriorReservations.ForEach(r => { r.Display(); Console.WriteLine(""); });
                        }
                    }
                    else
                    {
                        Console.WriteLine("Customer Not Found");
                    }
                    break;

                case 5:
                    Console.Clear();
                    Console.WriteLine("/---------------- Frequent Traveler Discount ----------------\\\n");
                    Console.Write("Enter Customer Name: ");
                    string frequentTravelerName = Console.ReadLine()?.ToLower() ?? "";

                    while (frequentTravelerName == "")
                    {
                        Console.WriteLine("Please enter a valide name: ");
                        frequentTravelerName = Console.ReadLine()?.ToLower() ?? "";
                    }

                    if (FileData.customers.Exists(c => c.Name == frequentTravelerName))
                    {
                        Customer currentCustomer = FileData.customers.Find(c => c.Name == frequentTravelerName)!;
                        Room currentRoom = FileData.rooms.Find(r => r.Number == currentCustomer.RoomNumber)!;
                        RoomPrice roomPrice = FileData.roomPrices.Find(r => r.Type == currentRoom.Type)!;

                        Console.WriteLine($"Customer Name: {currentCustomer.Name.ToUpper()}");
                        Console.WriteLine($"Room Daily Rate: ${roomPrice.DailyRate}");
                        Console.WriteLine($"Customer Room Daily Rate (discounted): ${roomPrice.DailyRate - (roomPrice.DailyRate * currentCustomer.Discount)}");
                        if (currentCustomer.Discount > 0)
                        {
                            Console.WriteLine($"Customer Frequent Traveler Discount: {currentCustomer.Discount * 100}%");
                        }
                        else
                        {
                            Console.WriteLine($"Customer doesn't qualify for Frequent Traveler Discount");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Customer not found");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    static void PriceManagementMenu()
    {
        Console.Clear();
        Console.WriteLine("/---------------- Price Management Menu ----------------\\\n");
        Console.WriteLine("1. Change Price For Room Type");
        Console.WriteLine("2. Coupon Codes Report");
        Console.WriteLine("3. Make a payment");

        Console.Write("Enter your choice (1 - 3): ");

        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("/---------------- Change Price For Room Type ----------------\\\n");
                    try
                    {
                        Console.Write("Select Room Type (1. Single, 2. Double, 3. Suite): ");

                        RoomType roomType;

                        while (!Enum.TryParse(Console.ReadLine(), true, out roomType))
                        {
                            Console.Write("Select Valid Room Type (1. Single, 2. Double, 3. Suite): ");
                        }

                        double dailyRate = GetInt("Enter Room Daily Rate: ");
                        if (Logic.TestableChangeRoomPrice(roomType, dailyRate))
                        {
                            Console.WriteLine("Room price changed with success");
                            WriteFiles.WriteRoomPrices();
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong");
                    }
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("/---------------- Coupon Codes Report ----------------\\\n");
                    FileData.coupons.ForEach(c => c.Display());
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("/---------------- Apply Coupon Code ----------------\\\n");
                    
                    try
                    {
                        Guid reservationNumber;

                        while (!Guid.TryParse(Console.ReadLine(), out reservationNumber))
                        {
                            Console.Write("Enter Reservation Number: ");
                        }

                        if (FileData.reservations.Exists(r => r.ReservationNumber == reservationNumber))
                        {
                            Console.Write("Enter Coupon Code: ");
                            string input = Console.ReadLine() ?? "";

                            if (FileData.coupons.Exists(c => c.Code == input.ToUpper()))
                            {
                                Reservation reservation = FileData.reservations.Find(r => r.ReservationNumber == reservationNumber)!;
                                Coupon coupon = FileData.coupons.Find(c => c.Code == input.ToUpper())!;
                                Customer customer = FileData.customers.Find(c => c.Name == reservation.CustomerName)!;
                                CouponRedemption redemptionCoupon = new CouponRedemption(coupon.Code, reservation.ReservationNumber);
                                WriteFiles.WriteCouponsRedemption(redemptionCoupon);
                                customer.Discount += coupon.Discount;
                                FileData.coupons.Remove(coupon);
                                WriteFiles.WriteCoupons();
                            } else
                            {
                                Console.WriteLine("Invalid Coupon");
                            }
                        }


                        if (FileData.reservations.Exists(r => r.ReservationNumber == reservationNumber))
                        {
                            Reservation reservation = FileData.reservations.Find(r => r.ReservationNumber == reservationNumber)!;
                            Customer customer = FileData.customers.Find(c => c.Name == reservation.CustomerName)!;
                            Room room = FileData.rooms.Find(r => r.Number == reservation.RoomNumber)!;
                            RoomPrice roomPrice = FileData.roomPrices.Find(r => r.Type == room.Type)!;

                            double toPay = roomPrice.DailyRate - (roomPrice.DailyRate * customer.Discount);

                        }
                        
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong");
                    }

                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    public static DateOnly GetDate(string message = "Enter a date (mm/dd/yyyy): ")
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

}