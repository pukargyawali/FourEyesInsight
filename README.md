### Here is a quick installation guide:

Installing
Follow these steps to get your development environment set up: (Before Run Start the Docker Desktop)

#### You will need the following tools:

  Visual Studio 2019

  .Net Core 3.1 SDK or later

  Docker Desktop

  Clone the repository

  At the src directory, where folders for the projects are present run below command:

#### Docker File
  docker-compose -f docker-compose.yml up -d

####You can launch the app with below url:

  https://localhost:5001/swagger

### Turn on cache
  There is a configuration for turning the caching mechanism on or off based on the requirement.
  This setting lives in appsettings.json file

### Improvements:
 While building production version, Logging should be used for different log levels in application- ERROR, DEBUG, INFO
 Endpoint health must also be tracked with various tools like X-RAY/AppDynamics.
