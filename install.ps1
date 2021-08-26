$dir = Get-Location
$path = $dir.ToString() + "\publish"
$binaryPath = $path + "\Mockingjay-Service.exe"

sc.exe create "Mockingjay Service" binpath=$binaryPath