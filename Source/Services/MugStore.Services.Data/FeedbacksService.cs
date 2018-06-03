﻿using MugStore.Data;
using MugStore.Data.Models;
using System.Linq;

namespace MugStore.Services.Data
{
    public class FeedbacksService : IFeedbacksService
    {
        private readonly IDbRepository<Feedback> feedback;

        public FeedbacksService(IDbRepository<Feedback> feedback)
        {
            this.feedback = feedback;
        }

        public void Add(Feedback feedback)
        {
            this.feedback.Add(feedback);
            this.feedback.Save();
        }

        public IQueryable<Feedback> Get()
        {
            return this.feedback.All();
        }

        public void Save()
        {
            this.feedback.Save();
        }

        public void Delete(Feedback feedback)
        {
            this.feedback.Delete(feedback);
            this.feedback.Save();
        }
    }
}
