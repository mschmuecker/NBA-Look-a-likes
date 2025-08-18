# NBA Look-a-Likes
PreReqs:
- Install SSMS and SQL Server
- Have dotnet installed from command line at the very least
    - check using dotnet --version - if you get an output then you are okay (you need version >8)
    - You can download the latest .NET SDK from: https://dotnet.microsoft.com/en-us/download

Steps
1. Setup database for the project - this can be found on the Google drive here: https://drive.google.com/drive/u/1/folders/1Mgnm3Qym-xRbh7Jczj7wodfDl6mlsufn

   Download the Database (this can take awhile) and unzip it
   
   On your SQL server that you already have installed (typically localhost\SQLEXPRESS), create a new database called 'NBA Player Look-A-Likes'
   - here is an example cmd: sqlcmd -S localhost\SQLEXPRESS -E -Q "CREATE DATABASE NBA Player Look-A-Likes"
   - If you don't name it as above then the SQL scripts below will fail
     
   Use sqlcmd from terminal to run each of the database scripts to setup the tables
    - sqlcmd -S localhost\SQLEXPRESS -E -d "NBA Player Look-A-Likes" -i "Path\To\YourScript.sql"
    - There are the following sql scripts that will need to run using the command:
    - Team.sql
    - TeamStats.sql
    - Players.sql
    - PlayedFor.sql
    - ArenaTeam.sql
    - Game.sql
    - PlayedIn.sql
    - GameStats.sql
  
  Finally use sqlcmd from terminal run the stored procedure script which is called: Procedures.sql
  - sqlcmd -S localhost\SQLEXPRESS -E -i "C:\Path\To\Procedures.sql"

You should be setup with the database now and can query using SSMS if you choose!


2. Now, retrieve and download the code from this repo here: https://github.com/mschmuecker/NBA-Look-a-likes
   - you can either repo to it or just unzip the code to your machine
   - Once you have it unzipped to your machine double check that appsettings.json (in root folder) matches your server and database you created above.
        - the app will fail without this check
3. Run the site locally using the command below in terminal/cmd:
     - dotnet run -warnaslevel:0 --project "C:\EpicSource\School\NBA DB App\NBA Look-a-Likes\NBA Look-a-Likes\NBA Look-a-Likes.csproj"
     - Pay attention to the output for the listening website
     info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5133

4. Navigate to your browwser at the listening website and have fun!
