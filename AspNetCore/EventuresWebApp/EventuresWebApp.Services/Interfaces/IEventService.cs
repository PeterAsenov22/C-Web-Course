﻿namespace EventuresWebApp.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IEventService
    {
        IEnumerable<EventModel> All();

        void Create(string name, string place, DateTime start, DateTime end, int totalTickets, decimal pricePerTicket);

        EventModel Last();

        bool Exists(string id);

        int TicketsLeftById(string id);

        void ReduceTicketsLeftCount(string id, int boughtTicketsCount);
    }
}
