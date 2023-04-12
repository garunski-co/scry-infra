using Pulumi;
using Pulumi.DigitalOcean;
using Pulumi.DigitalOcean.Inputs;
using Config = Pulumi.Config;

return await Deployment.RunAsync(() =>
{
    var config = new Config();

    var stackName = Deployment.Instance.StackName;

    var domain = new Domain("scry-root", new()
    {
        Name = "scryapp.website"
    });

    var local = new DnsRecord("local", new()
    {
        Domain = domain.Id,
        Name = "local",
        Type = "A",
        Value = "192.168.1.200"
    });

    var wildCardLocal = new DnsRecord("wildcardlocal", new()
    {
        Domain = domain.Id,
        Name = "*.local",
        Type = "A",
        Value = "192.168.1.200"
    });

    var vpc = new Vpc("vpc", new VpcArgs
    {
        Name = stackName + "-vpc",
        Region = "nyc1",
        IpRange = "10.0.0.0/16"
    });

    var dbCluster = new DatabaseCluster(stackName + "-db-cluster", new DatabaseClusterArgs
    {
        Name = stackName + "-pg-cluster",
        Engine = "pg",
        Region = "nyc1",
        Size = "db-s-1vcpu-1gb",
        Version = "15",
        NodeCount = 1,
        PrivateNetworkUuid = vpc.Id
    });

    // var dbFirewall = new DatabaseFirewall(stackName + "-db-firewall", new DatabaseFirewallArgs
    // {
    //     ClusterId = dbCluster.Id,
    //     Rules = new InputList<DatabaseFirewallRuleArgs>
    //     {
    //         new DatabaseFirewallRuleArgs
    //         {
    //             Type = "vpc",
    //             Value = vpc.Id
    //         }
    //     }
    // });
});
