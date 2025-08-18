$server = "localhost\SQLEXPRESS"
$database = "NBA Player Look-a-Likes"

Get-ChildItem -Filter *.sql | Sort-Object Name | ForEach-Object {
    Write-Host "Running $($_.FullName)..."
    sqlcmd -S $server -d $database -i $_.FullName
}
