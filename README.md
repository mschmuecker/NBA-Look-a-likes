# NBA Look-a-Likes
PreReqs:
- Use windows
- Install SSMS and SQL Server
- Have dotnet installed 
    - check using dotnet --version - if you get an output then you are okay (you need version >8)
    - You can download the latest .NET SDK from: https://dotnet.microsoft.com/en-us/download

Steps
1. Setup database for the project - this can be found on the Google drive here: https://drive.google.com/drive/u/1/folders/1Mgnm3Qym-xRbh7Jczj7wodfDl6mlsufn

   - Download the Database (this can take awhile) and unzip it
  
2. Next run the scripts to create and populate the database (the script runs sqlcmd)
   - First script is in the Database folder called CreateDatabase.ps1 - this will create the database and populate the data tables (this takes about 20-30 minutes)
   - Second script is to populate the procedures in the Procedures folder run RunProcs.ps1
  
3.  Now, retrieve and download the code from this repo here: https://github.com/mschmuecker/NBA-Look-a-likes
   - you can either repo to it or just unzip the code to your machine
   - Once you have it unzipped to your machine double check that appsettings.json (in root folder) matches your server and database you created above.
        - the app will fail without this check
4.  Run the site locally using the command below in terminal/cmd:
   dotnet run -warnaslevel:0 --project "C:\EpicSource\School\NBA DB App\NBA Look-a-Likes\NBA Look-a-Likes\NBA Look-a-Likes.csproj"
     - Pay attention to the output for the listening website
     info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5133

5. Navigate to your browwser at the listening website and have fun!
