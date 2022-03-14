# https://docs.microsoft.com/en-us/ef/core/cli/powershell
<#
	Secrets live in C:\Users\%USERNAME%\AppData\Roaming\Microsoft\UserSecrets
				AKA %APPDATA%\Microsoft\UserSecrets
#>

<# 
	Run this script in the Domain project
	Run in the console:
	Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope Process
	before starting the script
#>

# dotnet tool install --global dotnet-ef
# dotnet tool update --global dotnet-ef

#>

echo "Must run from the Domain Project"

dotnet-ef.exe dbcontext scaffold "Name=default" Oracle.EntityFrameworkCore  --startup-project ../WebUI --project ../Domain --context  OraEmpContextBase --configuration Debug --prefix-output `
 --context-dir ../Infrastructure/PersistenceGenSrc --context-namespace OraEmp.Infrastructure.Persistence `
-t COUNTRIES `
-t DEPARTMENTS `
-t EMPLOYEES `
-t JOBS `
-t JOB_HISTORY `
-t LOCATIONS `
-t REGIONS `
--output-dir EntitiesGenSrc --namespace OraEmp.Domain.Entities --no-pluralize --no-onconfiguring --force

