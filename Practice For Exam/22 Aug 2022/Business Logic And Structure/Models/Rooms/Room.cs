using System;
using System.Collections.Generic;
using System.Text;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Utilities.Messages;

namespace BookingApp.Models.Rooms
{
    public abstract class Room : IRoom
    {
        private int bedCapacity;
        private double pricePerNight;
        public Room(int bedCapacity) 
        {
            this.BedCapacity = bedCapacity;
            PricePerNight = 0;
        }
        public int BedCapacity
        {
            get => this.bedCapacity;
            private set
            {
                this.bedCapacity = value;
            }
        }
        public double PricePerNight 
        {   
            get => pricePerNight;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.PricePerNightNegative);
                }

                pricePerNight = value;
            }
        }
        public void SetPrice(double price)
        {
            PricePerNight = price;
        }
    }
}
