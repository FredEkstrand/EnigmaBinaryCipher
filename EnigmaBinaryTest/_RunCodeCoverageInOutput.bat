"..\..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -target:"..\..\..\packages\NUnit.ConsoleRunner.3.7.0\tools\nunit3-console.exe" -targetargs:"EnigmaBinaryTest.dll" -filter:"+[EnigmaBinaryTest]EnigmaBinaryTest* -[EnigmaBinaryTest]EnigmaBinaryTest.Properties" -excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" -excludebyattribute:"ExcludeFromCoverageAttribute" -excludebyfile:*\Fake*.cs -register:user -output:"_CodeCoverageResult.xml"

"..\..\..\packages\ReportGenerator.3.0.0\tools\ReportGenerator.exe" "-reports:_CodeCoverageResult.xml" "-targetdir:_CodeCoverageReport"

set "dummy="
set /p DUMMY=Hit ENTER to continue...
if defined dummy (echo not just ENTER was pressed) else (echo just ENTER was pressed)