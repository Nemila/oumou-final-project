using Hotel.Data;
using Hotel.Logic;

namespace LogicTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanReserveRoom()
        {
            // Arrange
            int roomNumber = 110;
            DateOnly startDate = new DateOnly(2030, 2, 12);
            DateOnly endDate = new DateOnly(2030, 2, 13);

            // Act
            bool result = Logic.CanReserveRoom(roomNumber, startDate, endDate);
            bool exists = FileData.reservations.Exists(r => roomNumber == r.RoomNumber && startDate == r.ReservationDateStart && endDate == r.ReservationDateStop);
            
            // Assert
            Assert.True(result);
            Assert.False(exists);
        }

        [Fact]
        public void CheckCannotReserveTakenRoom()
        {
            // Arrange
            int roomNumber = 110;
            DateOnly startDate = new DateOnly(2030, 2, 12);
            DateOnly endDate = new DateOnly(2030, 2, 13);

            // Act
            bool result = Logic.CanReserveRoom(roomNumber, startDate, endDate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckCustomerDoesNotHaveFrequentTravelerDiscount()
        {
            double expected = 0;
            double actual = Logic.FrequentTravelerDiscount("lamine diamoutene");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckCustomerHasFrequentTravelerDiscount()
        {
            double expected = .15;
            double actual = Logic.FrequentTravelerDiscount("oumou nimaga");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanChangeRoomPrice ()
        {
            Assert.True(Logic.TestableChangeRoomPrice(RoomType.Single, 29000));
        }

        [Fact]
        public void CannotChangeRoomPrice()
        {
            Assert.False(Logic.TestableChangeRoomPrice(RoomType.Single, -29000));
        }

        [Fact]
        public void CanMakeReservation()
        {
            // Arrange
            int room = 101;
            string name = "lamine diamoutene";
            DateOnly startDate = new DateOnly(2060, 04, 24);
            DateOnly endDate = new DateOnly(2060, 04, 27);

            // Act
            bool result = Logic.TestableMakeReservation(room, startDate, endDate, name);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanMakeNewRoom()
        {
            Assert.True(Logic.TestableMakeRoom(999, RoomType.Suite));
        }

        [Fact]
        public void CannotMakeAlreadyExistingRoom()
        {
            Assert.False(Logic.TestableMakeRoom(101, RoomType.Suite));
        }

        [Fact]
        public void CanDeleteCustomer()
        {
            int room = 101;
            string name = "lamine diamoutene";

            bool result = Logic.TestableDeleteCustomer(name, room);

            Assert.True(result);
        }

        [Fact]
        public void CannotDeleteUndefinedCustomer()
        {
            int room = 4512;
            string name = "does not exist";

            bool result = Logic.TestableDeleteCustomer(name, room);

            Assert.False(result);
        }

        [Fact]
        public void CanDeleteReservation()
        {
            bool result = Logic.TestableDeleteReservation(new Guid("2148af82-c7cb-4315-a38d-cefc29281e99"));
            Assert.True(result);
        }

        [Fact]
        public void CannotDeleteUndefinedReservation()
        {
            bool result = Logic.TestableDeleteReservation(new Guid("2148af82-c7cb-4315-a38d-cefc29281e00"));
            Assert.False(result);
        }

        [Fact] 
        public void CanAddCustomer()
        {
            int room = 650;
            string name = "james adame";
            
            bool result = Logic.TestableCreateCustomer(name, room);

            Assert.True(result);
        }

        [Fact]
        public void CannotAddExistingCustomer()
        {
            Logic.TestableCreateCustomer("james adame", 102);
            Assert.False(Logic.TestableCreateCustomer("james adame", 102));
        }

        /*[Fact]
        public void CanDeleteCustomer()
        {
            int room = 112;
            string name = "william smith";

            bool result = Logic.TestableDeleteCustomer(name, room);

            Assert.True(result);
        }*/

        /*[Fact]
        public void CannotDeleteNonExistingCustomer()
        {
            int room = 112;
            string name = "does not exist";

            bool result = Logic.TestableDeleteCustomer(name, room);

            Assert.False(result);
        }*/

        [Fact]
        public void TestableRefundReservation_ExistingReservation_ReturnsTrue()
        {
            // Arrange
            Guid existingReservationNumber = new Guid("2148af82-c7cb-4315-a38d-cefc29281e99");

            // Act
            var result = Logic.TestableRefundReservation(existingReservationNumber);
            
            // Assert
            Assert.Equal(existingReservationNumber, result?.ReservationNumber);
        }

        [Fact]
        public void TestableRefundReservation_NotFoundReservation_ReturnsTrue()
        {
            // Arrange
            Guid reservationNumber = new Guid("2148af82-c7cb-4315-a38d-cefc29281e00");

            // Act
            var result = Logic.TestableRefundReservation(reservationNumber);

            // Assert
            Assert.NotEqual(reservationNumber, result?.ReservationNumber);
        }

        /*[Fact]
        public void CannotReserve()
        {
            Assert.False(Logic.CanReserveRoom(101, new DateOnly(2024, 01, 10)));
        }*/

        /*[Fact]
        public void CanCreateCustumer()
        {
            // Add customer to the customer list
            Customer newCustomer = Logic.CreateCustomer();
            // Find the customer in the list
            bool isCustomerInTheList = false;
            foreach (var customer in Logic.customers)
            {
                if (customer.Equals(newCustomer))
                {
                    isCustomerInTheList = true;
                    return;
                }
            }
            // Throw error if the customer is not found
            Assert.True(isCustomerInTheList);
        }*/
    }
}
