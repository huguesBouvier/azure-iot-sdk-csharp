using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Devices.Client.HsmAuthentication
{ 
    /// <summary>
    /// HSM token provider.
    /// </summary>
    internal interface ITokenProvider
    {
        Task<string> GenerateTokenAsync(string moduleId);
    }
}
