using System.Collections.Generic;
using Pulumi.DigitalOcean;

return await Pulumi.Deployment.RunAsync(() =>
{
    var domain = new Domain("scry-root", new()
    {
        Name = "scryapp.website",
    });

    var local = new DnsRecord("local", new()
    {
        Domain = domain.Id,
        Name = "local",
        Type = "A",
        Value = "192.168.1.200",
    });

    var wildCardLocal = new DnsRecord("wildcardlocal", new()
    {
        Domain = domain.Id,
        Name = "*.local",
        Type = "A",
        Value = "192.168.1.200",
    });
});
