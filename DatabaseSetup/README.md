To build the images for testing or development:
1. Navigate to DatabaseSetup directory of this project
2. Execute the following command:
	* To build the image used in integration tests (that has just the schema):  
```docker build -f SchemaOnly/Dockerfile --build-arg password=<YourS3cure!Pwd> -t smwschemaonly:latest .```[^1]
	* To build the image with sample data (used for manual testing and preview):  
```docker build -f SchemaAndSampleData/Dockerfile --build-arg password=<YourS3cure!Pwd> -t smwsampledata:latest .```[^1]
[^1]: The password build arg is used for the SA account in SQL Server
