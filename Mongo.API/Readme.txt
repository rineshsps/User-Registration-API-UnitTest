// Get code coverage to run below code in text project 

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Exclude="[xunit*]\*" /p:CoverletOutput="./TestResults/"

ReportGenerator.exe -reports:testresults\coverage.cobertura.xml -targetdir:coveragereport



