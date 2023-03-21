// import * as k8s from "@pulumi/kubernetes";
// import * as kx from "@pulumi/kubernetesx";

// const appLabels = { app: "nginx" };
// const deployment = new k8s.apps.v1.Deployment("nginx", {
//   spec: {
//     selector: { matchLabels: appLabels },
//     replicas: 1,
//     template: {
//       metadata: { labels: appLabels },
//       spec: { containers: [{ name: "nginx", image: "nginx" }] },
//     },
//   },
// });
// export const name = deployment.metadata.name;

import * as pulumi from "@pulumi/pulumi";
import * as digitalocean from "@pulumi/digitalocean";

// Create a new domain
const _default = new digitalocean.Domain("scry-root", {
    name: "scryapp.website",
});


//172.20.0.0/16
