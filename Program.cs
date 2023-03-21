using System.Collections.Generic;
using Pulumi.DigitalOcean;

return await Pulumi.Deployment.RunAsync(() =>
{
    var domain = new Domain("scry-root", new()
    {
        Name = "scryapp.website",
    });
});
