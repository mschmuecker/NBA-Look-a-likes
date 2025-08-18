$server = "localhost\SQLEXPRESS"
$database = "nba"
$files = Get-ChildItem "C:\EpicSource\School\NBA DB App\NBA Look-a-Likes\NBA Look-a-Likes\SQL\Procs\*.sql"

foreach ($file in $files) {
    Write-Host "Running $($file.FullName)..."
    sqlcmd -S $server -d $database -i $file.FullName
}
