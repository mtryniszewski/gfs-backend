using System.Linq;
using System.Security.Claims;

namespace GFS.Web.Infrastructure
{
    public class GfsPrincipal
    {
        public GfsPrincipal(ClaimsPrincipal principal)
        {
            UserId = principal.Claims.ToList()[3].Value;
        }

        public string UserId { get; set; }
        public string Name { get; set; }
    }
}