using Pulumi;
using Pulumi.DigitalOcean;
using Pulumi.DigitalOcean.Inputs;
using Config = Pulumi.Config;

return await Deployment.RunAsync(() =>
{
    var config = new Config();

    var stackName = Deployment.Instance.StackName;

    var defaultRegion = Region.NYC1;

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
        Region = defaultRegion.ToString(),
        IpRange = "10.0.0.0/16"
    });

    var dbCluster = new DatabaseCluster(stackName + "-db-cluster", new DatabaseClusterArgs
    {
        Name = stackName + "-pg-cluster",
        Engine = "pg",
        Region = Region.NYC1,
        Size = DatabaseSlug.DB_1VPCU1GB,
        Version = "15",
        NodeCount = 1,
        PrivateNetworkUuid = vpc.Id
    });
    
    var k8sCluster = new KubernetesCluster(stackName + "-k8s-cluster", new KubernetesClusterArgs
    {
        Region = defaultRegion,
        Version = "1.26.3-do.0",
        NodePool = new KubernetesClusterNodePoolArgs
        {
            Name = stackName + "-k8s-" + DropletSlug.DropletS2VCPU4GB,
            Size = DropletSlug.DropletS2VCPU4GB.ToString(), 	//s-2vcpu-4gb
            NodeCount = 1,
        },
    });

    var dbFirewall = new DatabaseFirewall(stackName + "-db-firewall", new DatabaseFirewallArgs
    {
        ClusterId = dbCluster.Id,
        Rules = new InputList<DatabaseFirewallRuleArgs>
        {
            new DatabaseFirewallRuleArgs
            {
                Type = "k8s",
                Value = k8sCluster.Id
            }
        }
    });
});
