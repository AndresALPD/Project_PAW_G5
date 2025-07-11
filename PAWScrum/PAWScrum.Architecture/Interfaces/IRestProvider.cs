using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Architecture.Interfaces
{
    public interface IRestProvider
    {
        Task<string> PostAsync(string url, string json);
    }
}
