using Hotel.Logic;
using Hotel.Data;

class Program
{
    static void Main()
    {
        Logic.Init();
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
                    Logic.MakeRoom();
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("/---------------- Room Prices ----------------\\\n");
                    Logic.roomPrices.ForEach(r => r.Display());
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("/---------------- Available Room Search ----------------\\\n");

                    List<Room> roomsAvailability = Logic.GetAvailableRoomsByDate();
                    Console.WriteLine("");

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
        Console.WriteLine("2. Reservations Report");
        Console.WriteLine("3. Refund a Reservations");

        Console.Write("Enter your choice (1 - 3): ");
        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("/---------------- Make a Reservation ----------------\\\n");
                    Logic.MakeReservation();
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("/---------------- Reservations Report ----------------\\\n");
                    Logic.reservations.ForEach(r => { r.Display(); Console.WriteLine(""); });
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("/---------------- Refund a Reservations ----------------\\\n");
                    Logic.RefundReservation();
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
        Console.WriteLine("1. Create a Customer");
        Console.WriteLine("2. Customer Reservations Report");
        Console.WriteLine("3. Customer Prior Reservations");
        Console.WriteLine("4. Frequent Traveler Discount");

        Console.Write("Enter your choice (1 - 4): ");
        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("/---------------- Create a Customer ----------------\\\n");
                    Logic.CreateCustomer();
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("/---------------- Customer Reservations Report ----------------\\\n");

                    List<Reservation> customerReservations = Logic.GetCustomerReservationReport();

                    if (customerReservations.Count <= 0)
                    {
                        Console.WriteLine("No reservations for that customer.");
                    }
                    else
                    {
                        customerReservations.ForEach(r => { r.Display(); Console.WriteLine(""); });
                    }
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("/---------------- Customer Prior Reservations ----------------\\\n");

                    List<Reservation> customerPriorReservations = Logic.GetCustomerPriorReservation();

                    if (customerPriorReservations.Count <= 0)
                    {
                        Console.WriteLine("No reservations for that customer.");
                    }
                    else
                    {
                        customerPriorReservations.ForEach(r => { r.Display(); Console.WriteLine(""); });
                    }
                    break;

                case 4:
                    Console.Clear();
                    Console.WriteLine("/---------------- Frequent Traveler Discount ----------------\\\n");
                    Console.Write("Enter Customer Name: ");
                    string name = Console.ReadLine()?.ToLower() ?? "";

                    while (name == "")
                    {
                        Console.WriteLine("Please enter a valide name: ");
                        name = Console.ReadLine()?.ToLower() ?? "";
                    }

                    if (Logic.customers.Exists(c => c.Name == name))
                    {
                        Customer currentCustomer = Logic.customers.Find(c => c.Name == name)!;
                        Room currentRoom = Logic.rooms.Find(r => r.Number == currentCustomer.RoomNumber)!;
                        RoomPrice roomPrice = Logic.roomPrices.Find(r => r.Type == currentRoom.Type)!;

                        Console.WriteLine($"Customer Name: {currentCustomer.Name.ToUpper()}");
                        Console.WriteLine($"Room Daily Rate: {roomPrice.DailyRate}");
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

        Console.Write("Enter your choice (1 - 2): ");

        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("/---------------- Change Price For Room Type ----------------\\\n");
                    Logic.ChangeRoomPrice();
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("/---------------- Coupon Codes Report ----------------\\\n");
                    Logic.coupons.ForEach(c => c.Display());
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
}