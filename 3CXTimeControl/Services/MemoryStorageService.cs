using _3CXTimeControl.Models._3CX;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CXTimeControl.Services
{

    public class MemoryStorageService
    {
        private ConcurrentBag<NotificationRQ> _items = new();

        public void AddItem(NotificationRQ item)
        {
            _items.Add(item);
        }

        public List<NotificationRQ> GetItems(int id)
        {
            return _items.Where(x => x.id == id).ToList();
        }
        public void ClearItems(int id)
        {
            _items = new ConcurrentBag<NotificationRQ>(_items.Where(x => x.id != id).ToList());
            
        }

        //public Summary GenerateSummary()
        //{
        //    var items = _items.ToList();
        //    return new Summary
        //    {
        //        TotalItems = items.Count,
        //        LatestItemName = items.LastOrDefault()?.Name,
        //        GeneratedAt = DateTime.Now
        //    };
        //}
    }
}
