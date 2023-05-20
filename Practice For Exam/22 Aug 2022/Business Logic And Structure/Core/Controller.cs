using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using BookingApp.Core.Contracts;
using BookingApp.Models.Bookings;
using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Hotels;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using BookingApp.Utilities.Messages;

namespace BookingApp.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IHotel> hotels;

        public Controller()
        {
            hotels = new HotelRepository();
        }
        public string AddHotel(string hotelName, int category)
        {
            IHotel hotel = new Hotel(hotelName, category);

            if (hotels.Select(hotelName) == null)
            {
                hotels.AddNew(hotel);

                return string.Format(OutputMessages.HotelSuccessfullyRegistered, category, hotelName);
            }

            return string.Format(OutputMessages.HotelAlreadyRegistered, hotelName);
        }

        public string UploadRoomTypes(string hotelName, string roomTypeName)
        {
            IHotel hotel = hotels.Select(hotelName);

            if (hotel == null)
            {
                return string.Format(OutputMessages.HotelNameInvalid, hotelName);
            }

            if (hotel.Rooms.Select(roomTypeName) != null)
            {
                return string.Format(OutputMessages.RoomTypeAlreadyCreated);
            }

            IRoom room;

            if (roomTypeName == nameof(Apartment))
            {
                room = new Apartment();
            }
            else if (roomTypeName == nameof(DoubleBed))
            {
                room = new DoubleBed();
            }
            else if (roomTypeName == nameof(Studio))
            {
                room = new Studio();
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.RoomTypeIncorrect);
            }

            hotel.Rooms.AddNew(room);

            return string.Format(OutputMessages.RoomTypeAdded, roomTypeName, hotelName);
        }

        public string SetRoomPrices(string hotelName, string roomTypeName, double price)
        {
            IHotel hotel = hotels.Select(hotelName);

            if (hotel == null)
            {
                return string.Format(OutputMessages.HotelNameInvalid, hotelName);
            }

            IRoom room;

            if (roomTypeName != nameof(Apartment) && roomTypeName != nameof(DoubleBed) && roomTypeName != nameof(Studio))
            {
                throw new ArgumentException(ExceptionMessages.RoomTypeIncorrect);
            }

            room = hotel.Rooms.Select(roomTypeName);

            if (room == null)
            {
                return string.Format(OutputMessages.RoomTypeNotCreated);
            }

            if (room.PricePerNight != 0)
            {
                throw new InvalidOperationException(ExceptionMessages.PriceAlreadySet);
            }

            room.SetPrice(price);

            return string.Format(OutputMessages.PriceSetSuccessfully, roomTypeName, hotelName);
        }

        public string BookAvailableRoom(int adults, int children, int duration, int category)
        {
            if (hotels.All().OrderBy(x => x.FullName).FirstOrDefault(y => y.Category == category) == default)
            {
                return string.Format(OutputMessages.CategoryInvalid, category);
            }

            var orderedHotels = hotels.All().Where(x => x.Category == category).OrderBy(y => y.FullName);

            foreach (var hotel in orderedHotels)
            {
                var selectedRoom = hotel.Rooms.All().OrderBy(x => x.BedCapacity)
                    .FirstOrDefault(x => x.BedCapacity >= adults + children);

                if (selectedRoom != null && selectedRoom.PricePerNight > 0)
                {
                    int bookingNumber = hotel.Bookings.All().Count + 1;
                    IBooking booking = new Booking(selectedRoom, duration, adults, children, bookingNumber);

                    hotel.Bookings.AddNew(booking);

                    return string.Format(OutputMessages.BookingSuccessful, bookingNumber, hotel.FullName);
                }
            }

            return string.Format(OutputMessages.RoomNotAppropriate);
        }

        public string HotelReport(string hotelName)
        {
            var hotel = hotels.Select(hotelName);

            if (hotel == null)
            {
                return string.Format(OutputMessages.HotelNameInvalid, hotelName);
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Hotel name: {hotelName}");
            sb.AppendLine($"--{hotel.Category} star hotel");
            sb.AppendLine($"--Turnover: {hotel.Turnover:F2} $");
            sb.AppendLine("--Bookings:");

            if (hotel.Bookings.All().Count == 0)
            {
                sb.AppendLine();
                sb.AppendLine("none");
            }
            else
            {
                foreach (var booking in hotel.Bookings.All())
                {
                    sb.AppendLine();
                    sb.AppendLine(booking.BookingSummary());
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
