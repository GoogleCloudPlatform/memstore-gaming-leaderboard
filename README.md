# Gaming leaderboard application

This is the source code for the Cloud Memorystore for Redis based leaderboard application that accompanies the tutorial: [Using Cloud Memorystore for Redis as a game leaderboard](https://cloud.google.com/solutions/using-memorystore-for-redis-as-a-leaderboard). The application is built using ASP.NET Core and consists of a simple Web API for posting and retrieves scores. The Web API is implemented using [Cloud Memorystore for Redis](https://cloud.google.com/memorystore/), which is a fully managed Redis service available through Google Cloud Platform (GCP).

These are the two key methods that make up the Web API:

```csharp
Task<JsonResult> Post([FromBody] ScoreModel model)
Task<JsonResult> RetrieveScores(string centerKey, int offset, int numScores)
```

This Web API is called from a simple Vue.js game to post scores and retrieve the leaderboard.

There are two high-level steps involved in deploying the application:
- Upload Docker image to a container registry
- Deploy leaderboard application to a Kubernetes cluster

## Uploading Docker image for leaderboard application to Google Container Registry (GCR)

To upload the Docker image for the leaderboard application to your private registry with [GCR](https://cloud.google.com/container-registry/), run build.sh. This script also builds the ASP.NET application using [Cloud Build](https://cloud.google.com/cloud-build/) and then configures the image for deployment.

For more information about GCR, please see [Container Registry Documentation](https://cloud.google.com/container-registry/docs/).

For more information about Cloud Build, please see [Cloud Build Documentation](https://cloud.google.com/cloud-build/docs/).

## Deploying leaderboard application to Google Kubernetes Engine (GKE)

To set up a GKE cluster, please consult the [Google Kubernetes Engine Documentation](https://cloud.google.com/kubernetes-engine/docs/).


*NOTE*

*Within GKE, Alias IP addresses need to be turned **on** to allow your application pods to reach the Cloud Memorystore for Redis instances in the same VPC network. Please see [Creating VPC-native clusters using alias IP addresses](https://cloud.google.com/kubernetes-engine/docs/how-to/alias-ips) for more details.*

Once you have a cluster ready and have `kubectl` configured from your local client, you can deploy the application and service using the `appdeploy.yaml` file (in the leaderboardapp folder). First, open `appdeploy.yaml` in your favorite editor and replace `YOUR_PROJECT_ID` with your project id.

```yaml
image: "gcr.io/[YOUR_PROJECT_ID]/leaderboardapp"
```

And then deploy using `kubectl`:

```bash
kubectl create -f appdeploy.yaml
kubectl create -f gamingservice.yaml
```

Once the application and service have been deployed, the ingress component needs to be configured. With GKE, ingress is provided by the GCP HTTP(s) load balancer as described in [HTTP(s) load balancing with Ingress](https://cloud.google.com/kubernetes-engine/docs/concepts/ingress). To deploy ingress, use `kubectl` again:

```bash
kubectl create -f appingress.yaml
```

The ingress component takes anywhere from 2-20 minutes to become available. Once it's available, you can navigate to the front end IP address associated with HTTP(s) load balancer.
