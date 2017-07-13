using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Security;

namespace Checkout.ShoppingService
{
    public interface IUserMapper
    {
        IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context);
    }
}
