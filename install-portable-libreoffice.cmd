set ORIG = %CD%
mkdir .runtime
cd .runtime
mkdir portable
cd portable
 
if not exist "LibreOfficePortable_6.2.5_MultilingualStandard\" (
	mkdir LibreOfficePortable_6.2.5_MultilingualStandard
	cd LibreOfficePortable_6.2.5_MultilingualStandard
	curl -L -O "http://download.documentfoundation.org/libreoffice/portable/6.2.5/LibreOfficePortable_6.2.5_MultilingualStandard.paf.exe"
	start LibreOfficePortable_6.2.5_MultilingualStandard.paf
)