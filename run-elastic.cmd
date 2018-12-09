:: Run
:: ElasticSearch http://localhost:9200/
:: Kibana http://localhost:5601/

set JAVA_HOME=%cd%/.runtime/elastic/jdk-11.0.1+13
start %cd%/.runtime/elastic/elasticsearch-6.4.2/bin/elasticsearch.bat
start %cd%/.runtime/elastic/kibana-6.4.2-windows-x86_64/bin/kibana.bat