using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Models.Rooms;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories.Contracts;

namespace BookingApp.Repositories
{
    internal class RoomRepository : IRepository<IRoom>
    {
        private List<IRoom> rooms;

        public RoomRepository()
        {
            rooms = new List<IRoom>();
        }
        public void AddNew(IRoom model)
        {
            rooms.Add(model);
        }

        public IRoom Select(string criteria)
        {
            return this.rooms.FirstOrDefault(x => x.GetType().Name == criteria);
        }

        public IReadOnlyCollection<IRoom> All()
        {
            return rooms;
        }
    }
}
