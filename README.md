# SocialMediaWebsite
## Setup Guide
1. Clone the repository
2. Open up the command line
3. Navigate to the project root
4. Run ```SA_PASSWORD=<Y0URSQLP@SSWORDHERE> docker-compose up --build``` where `<Y0URSQLP@SSWORDHERE>` represents your password of choice for SA user of the SQL Server database.  
You can alternatively set this variable up in a separate .env file as shown [here](https://docs.docker.com/compose/environment-variables/set-environment-variables/) in order to avoid constantly passing the password each time you build the app.
5. Wait for the app to fully startup
6. The website should now be accessible on localhost via port 8080
## Disclaimer
The application is made for learning purposes only. It does not use HTTPS.