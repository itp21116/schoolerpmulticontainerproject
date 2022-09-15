using SchoolERP.MVC.UI.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace SchoolERP.MVC.UI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
