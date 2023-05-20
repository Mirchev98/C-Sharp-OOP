using System;
using System.Collections.Generic;
using System.Text;
using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Utilities.Messages;

namespace BookingApp.Models.Bookings
{
    public class Booking : IBooking
    {
        private IRoom room;
        private int residenceDuration;
        private int adultCount;
        private int childrenCount;
        private int bookingNumber;

        public Booking(IRoom room, int residenceDuration, int adultCount, int childrenCount, int bookingNumber)
        {
            Room = room;
            ResidenceDuration = residenceDuration;
            AdultsCount = adultCount;
            ChildrenCount = childrenCount;
            BookingNumber = bookingNumber;
        }

        public IRoom Room
        {
            get => room;
            
            private set
            {
                room = value;
            }
        }

        public int ResidenceDuration
        {
            get => residenceDuration;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(ExceptionMessages.DurationZeroOrLess);
                }

                residenceDuration = value;
            }
        }

        public int AdultsCount
        {
            get => adultCount;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(ExceptionMessages.AdultsZeroOrLess);
                }

                adultCount = value;
            }
        }

        public int ChildrenCount
        {
            get => childrenCount;

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.ChildrenNegative);
                }

                childrenCount = value;
            }
        }

        public int BookingNumber
        {
            get => bookingNumber;

            private set => bookingNumber = value;
        }
        public string BookingSummary()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Booking number: {BookingNumber}");
            sb.AppendLine($"Room type: {Room.GetType().Name}");
            sb.AppendLine($"Adults: {AdultsCount} Children: {ChildrenCount}");

            

            sb.AppendLine($"Total amount paid: {totalPaid():f2} $");

            return sb.ToString().TrimEnd();
        }

        private double totalPaid() => Math.Round(ResidenceDuration * this.Room.PricePerNight, 2);
    }
}
