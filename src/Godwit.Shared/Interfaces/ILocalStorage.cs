using Godwit.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godwit.Shared.Interfaces
{
    public interface ILocalStorage
    {
        public string GetItem(string key);
        public void SetItem(string key, string value);
        public void RemoveItem(string key);
        
    }
}
