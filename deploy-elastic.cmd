:: TODO: write script to deploy on mac, on docker, on stage, on prod
:: https://www.elastic.co/guide/en/elasticsearch/reference/current/_installation.html 
:: https://www.elastic.co/guide/en/kibana/current/install.html
::
:: ElasticSearch http://localhost:9200/
:: Kibana http://localhost:5601/

set ORIG = %CD%
mkdir .runtime
cd .runtime
mkdir elastic
cd elastic

:: JVM
IF NOT EXIST "jdk-11.0.1+13\" (
  curl -L -O https://github.com/AdoptOpenJDK/openjdk11-binaries/releases/download/jdk-11.0.1+13/OpenJDK11U-jdk_x64_windows_hotspot_11.0.1_13.zip
  tar -xkf OpenJDK11U-jdk_x64_windows_hotspot_11.0.1_13.zip
)

:: ES
if not exist "elasticsearch-6.4.2\" (
  curl -L -O https://artifacts.elastic.co/downloads/elasticsearch/elasticsearch-6.4.2.tar.gz
  tar -xkf elasticsearch-6.4.2.tar.gz
)

:: Kibana
if not exist "kibana-6.4.2-windows-x86_64\" (
curl -L -O https://artifacts.elastic.co/downloads/kibana/kibana-6.4.2-windows-x86_64.zip
tar -xkf kibana-6.4.2-windows-x86_64.zip
)

cd %ORIG%