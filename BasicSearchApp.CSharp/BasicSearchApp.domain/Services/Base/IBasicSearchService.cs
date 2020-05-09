
using BasicSearchApp.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BasicSearchApp.Services
{
    public interface IBasicSearchService
    {
        dynamic Filter(string query);
    }
}
