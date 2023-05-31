# Why this branch exists
To test docker stuff 

## PostIt notes 

dotnet run --project PxWeb --configuration Release

dotnet run --urls http://localhost:5000


docker build -t pxapi2 .
docker build -t pxapi2 -f Dockerfile_linux .

docker run -dp 8080:8080 --name px_node1 pxapi2

