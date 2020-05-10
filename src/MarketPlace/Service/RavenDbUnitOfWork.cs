﻿using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace MarketPlace.Service
{
    public class RavenDbUnitOfWork : IUnitOfWork
    {
        private readonly IAsyncDocumentSession session;

        public RavenDbUnitOfWork(IAsyncDocumentSession session)
        {
            this.session = session;
        }

        public Task Commit() => session.SaveChangesAsync();
    }
}